using System;

namespace Turbo.Core.Game.Players
{
    public interface IPlayerDetails
    {
        public void SaveNow();
        public int Id { get; }
        public string Name { get; }
        public string Motto { get; }
        public string Figure { get; }
        public string Gender { get; }
        public int Rank { get; }
        public DateTime DateCreated { get; }
        public DateTime DateUpdated { get; }
    }
}
