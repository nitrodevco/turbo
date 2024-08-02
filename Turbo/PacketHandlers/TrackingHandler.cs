using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Turbo.Core.Networking.Game.Clients;
using Turbo.Core.PacketHandlers;
using Turbo.Core.Packets;
using Turbo.Database.Entities.Tracking;
using Turbo.Database.Repositories.Tracking;
using Turbo.Packets.Incoming.Tracking;
using Turbo.Packets.Outgoing.Tracking;

namespace Turbo.Main.PacketHandlers;

public class TrackingHandler(
    IPacketMessageHub messageHub,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<TrackingHandler> logger) : IPacketHandlerManager
{
    public void Register()
    {
        messageHub.Subscribe<LatencyPingReportMessage>(this, OnLatencyPingReport);
        messageHub.Subscribe<LatencyPingRequestMessage>(this, OnLatencyPingRequest);
        messageHub.Subscribe<PerformanceLogMessage>(this, OnPerformanceTracker);
        messageHub.Subscribe<LagWarningReportMessage>(this, OnLagWarningReport);
    }
    
    private async void OnLatencyPingReport(LatencyPingReportMessage message, ISession session)
    {
        logger.LogInformation("Latency Ping Report: {0} {1} {2} from {3}", message.AverageLatency, message.ValidPingAverage, message.NumPings, session.IPAddress);
    }
    
    private async void OnLatencyPingRequest(LatencyPingRequestMessage message, ISession session)
    {
        logger.LogInformation("Latency Ping Request: {0} from {1}", message.ID, session.IPAddress);
        
        await session.Send(new LatencyPingResponseMessage()
        {
            ID = message.ID
        });
    }
    
    private async Task OnPerformanceTracker(PerformanceLogMessage message, ISession session)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var performanceLogRepository = scope.ServiceProvider.GetRequiredService<IPerformanceLogRepository>();

        var performanceLog = new PerformanceLogEntity
        {
            ElapsedTime = message.ElapsedTime,
            UserAgent = message.UserAgent,
            FlashVersion = message.FlashVersion,
            OS = message.OS,
            Browser = message.Browser,
            IsDebugger = message.IsDebugger,
            MemoryUsage = message.MemoryUsage,
            GarbageCollections = message.GarbageCollections,
            AverageFrameRate = message.AverageFrameRate
        };

        await performanceLogRepository.AddAsync(performanceLog);
    }
    
    public async void OnLagWarningReport(LagWarningReportMessage message, ISession session)
    {
        logger.LogInformation("Lag Warning Report: {0} from {1}", message.WarningCount, session.IPAddress);
    }
}