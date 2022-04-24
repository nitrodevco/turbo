using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Turbo.Database.Entities.Security;
using Turbo.Database.Entities.Furniture;
using Turbo.Database.Entities.Room;
using Microsoft.EntityFrameworkCore;
using Turbo.Database.Attributes;
using Turbo.Core.Game.Rooms.Object.Constants;

namespace Turbo.Database.Entities.Players
{
    [Table("players"), Index(nameof(Name), IsUnique = true)]
    public class PlayerEntity : Entity
    {
        [Column("name"), Required]
        public string Name { get; set; }

        [Column("motto")]
        public string? Motto { get; set; }

        [Column("figure"), Required, DefaultValueSql("'hr-115-42.hd-195-19.ch-3030-82.lg-275-1408.fa-1201.ca-1804-64'")]
        public string Figure { get; set; }

        [Column("gender"), Required, DefaultValueSql("0")] // AvatarGender.Male
        public AvatarGender Gender { get; set; }

        public PlayerSettingsEntity PlayerSettings { get; set; }

        public List<FurnitureEntity> Furniture { get; set; }

        public List<SecurityTicketEntity> SecurityTickets { get; set; }

        public List<RoomEntity> Rooms { get; set; }

        public List<RoomBanEntity> RoomBans { get; set; }

        public List<RoomMuteEntity> RoomMutes { get; set; }

        public List<RoomRightEntity> RoomRights { get; set; }
    }
}
