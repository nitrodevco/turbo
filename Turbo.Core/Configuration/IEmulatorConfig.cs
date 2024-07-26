using System.Collections.Generic;

namespace Turbo.Core.Configuration;

public class IGameConfig 
{
    public IHostConfig Tcp { get; init; }
    public IWhiteListedHostConfig WebSocket { get; init; }
    public IWhiteListedHostConfig Rcon { get; init; }
}

public class IHostConfig 
{
    public bool Enabled { get; init; }
    public string Host { get; init; }
    public int Port { get; init; }
}

public class IWhiteListedHostConfig 
{
    public bool Enabled { get; init; }
    public string Host { get; init; }
    public int Port { get; init; }
    public List<string> Whitelist { get; init; }
}

public interface IEmulatorConfig
{
    public IGameConfig Game { get; }
    public bool DatabaseLoggingEnabled { get; }
    public int NetworkWorkerThreads { get; }
    public List<string> PluginOrder { get; }
    public int FloodMessageLimit { get; }
    public int FloodTimeFrameSeconds { get; }
    public int FloodMuteDurationSeconds { get; }
}