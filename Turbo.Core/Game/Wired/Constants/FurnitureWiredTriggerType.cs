namespace Turbo.Core.Game.Wired.Constants;

public enum FurnitureWiredTriggerType
{
    AvatarSaysSomething = 0,
    AvatarWalksOnFurni = 1,
    AvatarWalksOffFurni = 2,
    TriggerOnce = 3,
    ToggleFurni = 4,
    TriggerPeriodically = 6,
    AvatarEntersRoom = 7,
    GameStarts = 8,
    GameEnds = 9,
    ScoreAchieved = 10,
    Collision = 11,
    TriggerPeriodicallyLong = 12,
    BotReachedStuff = 13,
    BotReachedAvatar = 14
}