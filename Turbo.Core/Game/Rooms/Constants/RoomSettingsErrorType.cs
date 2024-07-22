namespace Turbo.Core.Game.Rooms.Constants;

public enum RoomSettingsErrorType
{
    None = 0,
    RoomNotFound = 1,
    NotOwner = 2,
    InvalidDoorMode = 3,
    InvalidUserLimit = 4,
    InvalidPassword = 5,
    InvalidCategory = 6,
    InvalidName = 7,
    BadName = 8,
    InvalidDescription = 9,
    BadDescription = 10,
    InvalidTag = 11,
    BadTag = 12,
    TagTooLong = 13
}