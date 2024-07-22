using System.Collections.Generic;

namespace Turbo.Core.Configuration;

public interface IEmulatorConfig
{
    public string GameHost { get; }

    public bool GameTCPEnabled { get; }
    public int GameTCPPort { get; }

    public bool GameWSEnabled { get; }
    public int GameWSPort { get; }
    public List<string> GameWSWhitelist { get; }

    public string RCONHost { get; }
    public int RCONPort { get; }
    public List<string> RCONWhitelist { get; }
    public string DatabaseHost { get; }
    public string DatabaseUser { get; }
    public string DatabasePassword { get; }
    public string DatabaseName { get; }
    public bool DatabaseLoggingEnabled { get; }

    public int NetworkWorkerThreads { get; }

    public List<string> PluginOrder { get; }

    public int FloodMessageLimit { get; }
    public int FloodTimeFrameSeconds { get; }
    public int FloodMuteDurationSeconds { get; }
}