using System;
using System.Threading.Tasks;
using Turbo.Core.Game.Players.Constants;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Players;

public interface IPlayerDetails
{
    public int Id { get; }
    public string Name { get; }
    public string Motto { get; set; }
    public string Figure { get; set; }
    public AvatarGender Gender { get; set; }
    public PlayerStatusEnum PlayerStatus { get; set; }
    public DateTime DateCreated { get; }
    public DateTime DateUpdated { get; }
    public Task DisposeAsync();
    public int GetValidChatStyleId(int styleId);
    public void SetPreferredChatStyleByClientId(int styleId);
}