﻿namespace Turbo.Core.Game.Rooms.Object
{
    public interface IRoomObjectUserHolder : IRoomObjectHolder
    {
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public string Gender { get; }
    }
}