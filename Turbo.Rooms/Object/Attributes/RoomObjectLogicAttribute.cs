using System;

namespace Turbo.Rooms.Object.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RoomObjectLogicAttribute : Attribute
{
    public readonly string Name;

    public RoomObjectLogicAttribute(string name)
    {
        Name = name;
    }
}