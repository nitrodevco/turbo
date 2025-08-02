using System;

namespace Turbo.Core.Game.Players.Constants;

[Flags]
public enum PlayerPerkEnum
{
    None = 0,
    Citizen = 1 << 0,
    VoteInCompetitions = 1 << 1,
    Trade = 1 << 2,
    CallOnHelpers = 1 << 3,
    JudgeChatReviews = 1 << 4,
    NavigatorRoomThumbnailCamera = 1 << 5,
    UseGuideTool = 1 << 6,
    MouseZoom = 1 << 7,
    HabboClubOfferBeta = 1 << 8,
    NavigatorPhaseTwo2014 = 1 << 9,
    UnityTrade = 1 << 10,
    BuilderAtWork = 1 << 11,
    Camera = 1 << 12
}