using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Advertising;
using Turbo.Packets.Outgoing.Advertising;

namespace Turbo.Main.PacketHandlers;

public class AdvertisingHandler : IAdvertisingHandler
{
    private readonly ILogger<AuthenticationMessageHandler> _logger;
    private readonly IPacketMessageHub _messageHub;

    public AdvertisingHandler(

        IPacketMessageHub messageHub,
        ILogger<AuthenticationMessageHandler> logger
    )
    {
        _messageHub = messageHub;
        _logger = logger;
        
        
        _messageHub.Subscribe<InterstitialShownMessage>(this, OnInterstitialShownMessage);
        _messageHub.Subscribe<GetInterstitialMessage>(this, OnInterstitialMessage);
    }

    private async void OnInterstitialMessage(GetInterstitialMessage message, ISession session)
    {
        _logger.LogInformation("Received InterstitialMessage");

        await session.Send(new InterstitialMessage()
            {
                imageUrl = "https://images.habbo.com/c_images/AdWarningsUK/ad_warning_L.png",
                clickUrl = "https://habbo.com"
            }
        );
    }
    
    private async void OnInterstitialShownMessage(InterstitialShownMessage message, ISession session)
    {
        _logger.LogInformation("Received InterstitialShownMessage");
    }
}