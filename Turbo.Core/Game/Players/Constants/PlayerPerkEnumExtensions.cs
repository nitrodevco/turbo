using System;

namespace Turbo.Core.Game.Players.Constants;

public static class PlayerPerkExtensions
{
    public static string ToLegacyString(PlayerPerkEnum perk) => perk switch
    {
        PlayerPerkEnum.Citizen => "CITIZEN",
        PlayerPerkEnum.VoteInCompetitions => "VOTE_IN_COMPETITIONS",
        PlayerPerkEnum.Trade => "TRADE",
        PlayerPerkEnum.CallOnHelpers => "CALL_ON_HELPERS",
        PlayerPerkEnum.JudgeChatReviews => "JUDGE_CHAT_REVIEWS",
        PlayerPerkEnum.NavigatorRoomThumbnailCamera => "NAVIGATOR_ROOM_THUMBNAIL_CAMERA",
        PlayerPerkEnum.UseGuideTool => "USE_GUIDE_TOOL",
        PlayerPerkEnum.MouseZoom => "MOUSE_ZOOM",
        PlayerPerkEnum.HabboClubOfferBeta => "HABBO_CLUB_OFFER_BETA",
        PlayerPerkEnum.NavigatorPhaseTwo2014 => "NAVIGATOR_PHASE_TWO_2014",
        PlayerPerkEnum.UnityTrade => "UNITY_TRADE",
        PlayerPerkEnum.BuilderAtWork => "BUILDER_AT_WORK",
        PlayerPerkEnum.Camera => "CAMERA",
        _ => throw new ArgumentOutOfRangeException(nameof(perk), perk, null)
    };

    public static PlayerPerkEnum? FromLegacyString(string perkString) => perkString switch
    {
        "CITIZEN" => PlayerPerkEnum.Citizen,
        "VOTE_IN_COMPETITIONS" => PlayerPerkEnum.VoteInCompetitions,
        "TRADE" => PlayerPerkEnum.Trade,
        "CALL_ON_HELPERS" => PlayerPerkEnum.CallOnHelpers,
        "JUDGE_CHAT_REVIEWS" => PlayerPerkEnum.JudgeChatReviews,
        "NAVIGATOR_ROOM_THUMBNAIL_CAMERA" => PlayerPerkEnum.NavigatorRoomThumbnailCamera,
        "USE_GUIDE_TOOL" => PlayerPerkEnum.UseGuideTool,
        "MOUSE_ZOOM" => PlayerPerkEnum.MouseZoom,
        "HABBO_CLUB_OFFER_BETA" => PlayerPerkEnum.HabboClubOfferBeta,
        "NAVIGATOR_PHASE_TWO_2014" => PlayerPerkEnum.NavigatorPhaseTwo2014,
        "UNITY_TRADE" => PlayerPerkEnum.UnityTrade,
        "BUILDER_AT_WORK" => PlayerPerkEnum.BuilderAtWork,
        "CAMERA" => PlayerPerkEnum.Camera,
        _ => null
    };
}