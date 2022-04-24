using System;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Players
{
    public interface IPlayerDetails
    {
        public void Save();
        public int Id { get; }
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public AvatarGender Gender { get; }
        public DateTime DateCreated { get; }
        public DateTime DateUpdated { get; }
    }
}
