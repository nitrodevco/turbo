using System;
using System.Collections;
using System.Collections.Generic;
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
    public IList<PlayerPerkEnum> PlayerPerks { get; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public DateTime? DeletedAt { get; }
    public Task DisposeAsync();
    public int GetValidChatStyleId(int styleId);
    public void SetPreferredChatStyleByClientId(int styleId);
}