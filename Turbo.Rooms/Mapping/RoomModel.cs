using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Database.Entities.Room;
using Turbo.Rooms.Utils;
using System.Linq;

namespace Turbo.Rooms.Mapping
{
    public class RoomModel : IRoomModel
    {
        private readonly RoomModelEntity _modelEntity;
        public string Model { get; private set; }

        public int TotalX { get; private set; }
        public int TotalY { get; private set; }
        public int TotalSize { get; private set; }

        public IPoint DoorLocation { get; private set; }
        public IPoint DoorDirection { get; private set; }

        private IList<IList<RoomTileState>> _tileStates;
        private IList<IList<int>> _tileHeights;

        public bool DidGenerate { get; private set; }

        public RoomModel(RoomModelEntity modelEntity)
        {
            _modelEntity = modelEntity;

            Model = CleanModel(modelEntity.Model);
        }

        public static string CleanModel(string model)
        {
            if (model == null) return null;

            return model.Trim().ToLower().Replace("/\r\n|\r|\n/g", "\r");
        }

        public void ResetModel(bool generate)
        {
            Model = null;

            TotalX = 0;
            TotalY = 0;
            TotalSize = 0;

            DoorLocation = null;
            DoorDirection = null;

            _tileStates = new List<IList<RoomTileState>>();
            _tileHeights = new List<IList<int>>();

            DidGenerate = false;

            if (generate) Generate();
        }

        public void Generate()
        {
            string[] rows = Model.Split("\r");
            int totalX = rows[0].Length;
            int totalY = rows.Length;

            if ((rows.Length <= 0) || (totalX <= 0) || (totalY <= 0))
            {
                ResetModel(false);

                return;
            }

            for (int y = 0; y < totalY; y++)
            {
                string row = rows[y];

                if ((row == null) || (row == "\r")) continue;

                int rowLength = row.Length;

                if (rowLength == 0) continue;

                for (int x = 0; x < totalX; x++)
                {
                    if (rowLength != totalX)
                    {
                        ResetModel(false);

                        return;
                    }

                    string square = rows[y].Substring(x, (x + 1)).Trim();

                    if (_tileStates[x] == null) _tileStates[x] = new List<RoomTileState>();
                    if (_tileHeights[x] == null) _tileHeights[x] = new List<int>();

                    if (square.Equals("x"))
                    {
                        _tileStates[x][y] = RoomTileState.Closed;
                        _tileHeights[x][y] = 0;
                    }
                    else
                    {
                        int index = "abcdefghijklmnopqrstuvwxyz".IndexOf(square);

                        _tileStates[x][y] = RoomTileState.Open;
                        _tileHeights[x][y] = ((index == -1) ? Int32.Parse(square) : (index + 10));
                    }

                    TotalSize++;
                }
            }

            if (TotalSize != (totalX * totalY))
            {
                ResetModel(false);

                return;
            }

            TotalX = totalX;
            TotalY = totalY;

            RoomTileState doorTileState = GetTileState(_modelEntity.DoorX, _modelEntity.DoorY);
            int doorTileHeight = GetTileHeight(_modelEntity.DoorX, _modelEntity.DoorY);

            if (doorTileState == RoomTileState.Closed)
            {
                ResetModel(false);

                return;
            }

            DoorLocation = new Point(_modelEntity.DoorX, _modelEntity.DoorY, (double)doorTileHeight);
            DoorDirection = new Point(_modelEntity.DoorDirection);

            DidGenerate = true;
        }

        public RoomTileState GetTileState(int x, int y)
        {
            IList<RoomTileState> rowStates = _tileStates.ElementAtOrDefault(x);

            if (rowStates == null) return RoomTileState.Closed;

            if (rowStates[y] != RoomTileState.Open) return RoomTileState.Closed;

            return RoomTileState.Open;
        }

        public int GetTileHeight(int x, int y)
        {
            IList<int> rowHeights = _tileHeights.ElementAtOrDefault(x);

            if ((rowHeights == null) || (rowHeights[y] <= 0)) return 0;

            return rowHeights[y];
        }

        public int Id
        {
            get => _modelEntity.Id;
        }

        public string Name
        {
            get => _modelEntity.Name;
        }
    }
}
