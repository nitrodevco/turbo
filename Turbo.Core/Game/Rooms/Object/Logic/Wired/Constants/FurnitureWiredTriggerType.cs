namespace Turbo.Core.Game.Rooms.Object.Logic.Wired.Constants
{
    public enum FurnitureWiredTriggerType
    {
        AVATAR_SAYS_SOMETHING     = 0,
        AVATAR_WALKS_ON_FURNI     = 1,
        AVATAR_WALKS_OFF_FURNI    = 2,
        TRIGGER_ONCE              = 3,
        TOGGLE_FURNI              = 4,
        TRIGGER_PERIODICALLY      = 6,
        AVATAR_ENTERS_ROOM        = 7,
        GAME_STARTS               = 8,
        GAME_ENDS                 = 9,
        SCORE_ACHIEVED            = 10,
        COLLISION                 = 11,
        TRIGGER_PERIODICALLY_LONG = 12,
        BOT_REACHED_STUFF         = 13,
        BOT_REACHED_AVATAR        = 14
    }
}
