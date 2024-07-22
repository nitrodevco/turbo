using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Core.Game.Rooms.Object;

public interface IRoomObjectAvatarHolder : IRoomObjectHolder<IRoomObjectAvatar>
{
    public string Name { get; }
    public string Motto { get; }
    public string Figure { get; }
    public AvatarGender Gender { get; }
}