using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Turbo.Core.Events;
using Turbo.Core.Game.Players;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Core.Security;
using Turbo.Events.Game.Security;
using Turbo.Networking.Game.Codec;
using Turbo.Packets.Incoming.Handshake;
using Turbo.Packets.Outgoing.Handshake;
using Turbo.Security;

namespace Turbo.Main.PacketHandlers;

public class AuthenticationMessageHandler : IAuthenticationMessageHandler
{
    private readonly IDiffieService _diffieService;
    private readonly ITurboEventHub _eventHub;
    private readonly ILogger<AuthenticationMessageHandler> _logger;
    private readonly IPacketMessageHub _messageHub;
    private readonly IPlayerManager _playerManager;
    private readonly IRsaService _rsaService;
    private readonly ISecurityManager _securityManager;

    public AuthenticationMessageHandler(
        IPacketMessageHub messageHub,
        ISecurityManager securityManager,
        IPlayerManager playerManager,
        ILogger<AuthenticationMessageHandler> logger,
        ITurboEventHub eventHub,
        IRsaService rsaService,
        IDiffieService diffieService
    )
    {
        _messageHub = messageHub;
        _securityManager = securityManager;
        _playerManager = playerManager;
        _logger = logger;
        _eventHub = eventHub;
        _rsaService = rsaService;
        _diffieService = diffieService;

        _messageHub.Subscribe<InitDiffieHandshakeMessage>(this, OnHandshake);
        _messageHub.Subscribe<CompleteDiffieHandshakeMessage>(this, OnCompleteHandshake);
        _messageHub.Subscribe<SSOTicketMessage>(this, OnSSOTicket);
        _messageHub.Subscribe<InfoRetrieveMessage>(this, OnInfoRetrieve);
    }

    private async void OnCompleteHandshake(CompleteDiffieHandshakeMessage message, ISession session)
    {
        var sharedKey = _diffieService.GetSharedKey(message.SharedKey);

        session.Rc4 = new Rc4Service(sharedKey);

        _logger.LogInformation("Diffie handshake completed for {0}", session.IPAddress);

        session.Channel.Pipeline.AddBefore("frameDecoder", "encryptionDecoder", new EncryptionDecoder(session));

        await session.Send(new CompleteDiffieHandshakeComposer
        {
            PublicKey = _diffieService.GetPublicKey()
        });
    }

    private async void OnHandshake(InitDiffieHandshakeMessage message, ISession session)
    {
        // rsa
        var prime = _diffieService.GetSignedPrime();
        var generator = _diffieService.GetSignedGenerator();

        await session.Send(new InitDiffieHandshakeComposer
        {
            Prime = prime,
            Generator = generator
        });
    }

    public async Task OnSSOTicket(SSOTicketMessage message, ISession session)
    {
        var userId = await _securityManager.GetPlayerIdFromTicket(message.SSO);

        if (userId <= 0)
        {
            await session.DisposeAsync();

            return;
        }

        var player = await _playerManager.CreatePlayer(userId, session);

        if (player == null)
        {
            await session.DisposeAsync();

            return;
        }

        // send required composers for hotel view
        await session.Send(new AuthenticationOKMessage
        {
            AccountId = session.Player.Id,
            SuggestedLoginActions = [],
            IdentityId = session.Player.Id
        });
        await session.Send(new UserRightsMessage
        {
            ClubLevel = ClubLevelEnum.Vip,
            SecurityLevel = SecurityLevelEnum.Moderator,
            IsAmbassador = false
        });


        var messager = _eventHub.Dispatch(new UserLoginEvent
        {
            Player = player
        });

        if (messager.IsCancelled) await player.DisposeAsync();
    }

    public async Task OnInfoRetrieve(InfoRetrieveMessage message, ISession session)
    {
        await session.Send(new UserObjectMessage
        {
            Player = session.Player
        });
    }
}