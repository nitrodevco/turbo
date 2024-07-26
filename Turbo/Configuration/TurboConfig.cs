using System.Collections.Generic;
using Turbo.Core.Configuration;

namespace Turbo.Main.Configuration;

public class TurboConfig : IEmulatorConfig
{
    public const string Turbo = "Turbo";
    public IGameConfig Game { get; init; }
    public bool DatabaseLoggingEnabled { get; init; }
    public int NetworkWorkerThreads { get; init; }
    public List<string> PluginOrder { get; init; }
    public int FloodMessageLimit { get; init; }
    public int FloodTimeFrameSeconds { get; init; }
    public int FloodMuteDurationSeconds { get; init; }
}