using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Turbo.Core.Database.Entities.Players;

[Table("player_perks")]
[Index(nameof(PlayerEntityId), IsUnique = true)]
public class PlayerPerksEntity : Entity
{
    [Column("player_id")] public int PlayerEntityId { get; set; }

    [Column("CITIZEN")] public bool Citizen { get; set; }

    [Column("VOTE_IN_COMPETITIONS")] public bool VoteInCompetitions { get; set; }

    [Column("TRADE")] public bool Trade { get; set; }

    [Column("CALL_ON_HELPERS")] public bool CallOnHelpers { get; set; }

    [Column("JUDGE_CHAT_REVIEWS")] public bool JudgeChatReviews { get; set; }

    [Column("NAVIGATOR_ROOM_THUMBNAIL_CAMERA")] public bool NavigatorRoomThumbnailCamera { get; set; }

    [Column("USE_GUIDE_TOOL")] public bool UseGuideTool { get; set; }

    [Column("MOUSE_ZOOM")] public bool MouseZoom { get; set; }

    [Column("HABBO_CLUB_OFFER_BETA")] public bool HabboClubOfferBeta { get; set; }

    [Column("NAVIGATOR_PHASE_TWO_2014")] public bool NavigatorPhaseTwo2014 { get; set; }

    [Column("UNITY_TRADE")] public bool UnityTrade { get; set; }

    [Column("BUILDER_AT_WORK")] public bool BuilderAtWork { get; set; }

    [Column("CAMERA")] public bool Camera { get; set; }

    [ForeignKey(nameof(PlayerEntityId))] public PlayerEntity PlayerEntity { get; set; }
}