using Turbo.Core.Packets.Messages;

namespace Turbo.Packets.Outgoing.Perk;

public record PerkAllowancesMessage : IComposer
{
    public int TotalPerks { get; init; }
    public bool CITIZEN { get; init; }
    public bool VOTE_IN_COMPETITIONS { get; init; }
    public bool TRADE { get; init; }
    public bool CALL_ON_HELPERS { get; init; }
    public bool JUDGE_CHAT_REVIEWS { get; init; }
    public bool NAVIGATOR_ROOM_THUMBNAIL_CAMERA { get; init; }
    public bool USE_GUIDE_TOOL { get; init; }
    public bool MOUSE_ZOOM { get; init; }
    public bool HABBO_CLUB_OFFER_BETA { get; init; }
    public bool NAVIGATOR_PHASE_TWO_2014 { get; init; }
    public bool UNITY_TRADE { get; init; }
    public bool BUILDER_AT_WORK { get; init; }
    public bool CAMERA { get; init; }
}