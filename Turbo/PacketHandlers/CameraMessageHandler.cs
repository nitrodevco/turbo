using System;
using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Packets.Incoming.Camera;
using Turbo.Packets.Outgoing.Camera;

namespace Turbo.Main.PacketHandlers;
internal class CameraMessageHandler(
    IPacketMessageHub messageHub,
    ILogger<CameraMessageHandler> logger) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<RequestCameraConfigurationMessage>(this, OnInitCameraMessage);
    }

    private async void OnInitCameraMessage(RequestCameraConfigurationMessage message, ISession session)
    {
        if(session.Player == null) return;

        await session.Send(new InitCameraMessage
        {
            CostCredits = 10,
            CostCurrency = 0,
            CurrencyType = 20
        });
    }
}
