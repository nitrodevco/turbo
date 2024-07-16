using System;

namespace Turbo.Rooms.Managers;

public class FloodControlData
{
    public int MessageCount { get; set; }
    public DateTime FirstMessageTime { get; set; }
}