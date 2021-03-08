using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Database.Entities.Room
{
    [Table("rooms")]
    public class RoomEntity : Entity
    {
        [Column("name")]
        public string Name { get; set; }

        public bool AllowWalkThrough { get; set; }

        public List<RoomBanEntity> RoomBans { get; set; }

        [Column("model_id")]
        public int RoomModelEntityId { get; set; }

        public RoomModelEntity RoomModelEntity { get; set; }

        public List<RoomMuteEntity> RoomMutes { get; set; }

        public List<RoomRightEntity> RoomRights { get; set; }
    }
}
