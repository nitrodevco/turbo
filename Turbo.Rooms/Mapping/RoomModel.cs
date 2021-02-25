using System;
using System.Collections.Generic;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public class RoomModel : IRoomModel
    {
        public string Model { get; private set; }

        public IPoint DoorLocation;
        public IPoint DoorDirection;

        public List<int> TileStates;
        public List<int> TileHeights;

        public RoomModel(string model)
        {
            Model = CleanModel(model);
        }

        public static string CleanModel(string model)
        {
            if (model == null) return null;

            return model.Trim().ToLower().Replace("/\r\n|\r|\n/g", "\r");
        }

        public void ResetModel(bool generate)
        {
            Model = null;

            DoorLocation = null;
            DoorDirection = null;

            TileStates = null;
            TileHeights = null;

            if (generate) Generate();
        }

        public void Generate()
        {
            string[] rows = Model.Split("\r");
        }
    }
}
