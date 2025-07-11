using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Advertising;
using Turbo.Packets.Outgoing.Advertising;

namespace Turbo.Main.PacketHandlers;

public class AdvertisementMessageHandler(
    IPacketMessageHub messageHub,
    ILogger<AdvertisementMessageHandler> logger) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<InterstitialShownMessage>(this, OnInterstitialShownMessage);
        messageHub.Subscribe<GetInterstitialMessage>(this, OnInterstitialMessage);
    }

    private async void OnInterstitialMessage(GetInterstitialMessage message, ISession session)
    {
        if (session.Player == null) return;

        await session.Send(new InterstitialMessage()
            {
                imageUrl = "https://images.habbo.com/c_images/AdWarningsUK/ad_warning_L.png",
                clickUrl = "https://habbo.com"
            }
        );
    }
    
    private async void OnInterstitialShownMessage(InterstitialShownMessage message, ISession session)
    {
        logger.LogInformation("Received InterstitialShownMessage");
    }
}