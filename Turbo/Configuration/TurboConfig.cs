using System.Collections.Generic;
using Turbo.Core.Configuration;

namespace Turbo.Main.Configuration;

public class TurboConfig : IEmulatorConfig
{
    public const string Turbo = "Turbo";

    public string GameHost { get; set; }

    public bool GameTCPEnabled { get; set; }

    public int GameTCPPort { get; set; }

    public bool GameWSEnabled { get; set; }

    public int GameWSPort { get; set; }

    public List<string> GameWSWhitelist { get; set; }

    public string RCONHost { get; set; }

    public int RCONPort { get; set; }

    public List<string> RCONWhitelist { get; set; }

    public string DatabaseHost { get; set; }

    public string DatabaseUser { get; set; }

    public string DatabasePassword { get; set; }

    public string DatabaseName { get; set; }

    public bool DatabaseLoggingEnabled { get; set; }

    public int NetworkWorkerThreads { get; set; }

    public List<string> PluginOrder { get; set; }

    public int FloodMessageLimit { get; set; }

    public int FloodTimeFrameSeconds { get; set; }

    public int FloodMuteDurationSeconds { get; set; }
}