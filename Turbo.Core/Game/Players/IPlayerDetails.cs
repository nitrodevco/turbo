using System;

namespace Turbo.Core.Game.Players
{
    public interface IPlayerDetails
    {
        public void SaveNow();
        public int Id { get; }
        public string Name { get; }
        public string Motto { get; }
        public DateTime DateCreated { get; }
        public DateTime DateUpdated { get; }
    }
}
