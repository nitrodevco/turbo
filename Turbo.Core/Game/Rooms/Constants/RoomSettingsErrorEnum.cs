using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.Constants
{
    public enum RoomSettingsErrorEnum
    {
        InvalidPassword = 5,
        InvalidName = 7,
        BadName = 8,
        BadDescription = 10,
        BadTags = 11,
        InvalidTags = 12,
        ToManyTags = 13
    }
}
