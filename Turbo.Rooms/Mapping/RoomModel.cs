using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Database.Entities.Room;
using Turbo.Rooms.Utils;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Turbo.Rooms.Mapping
{
    public class RoomModel : IRoomModel
    {
        private static readonly Regex regex = new(@"\r\n|\r|\n");

        private readonly RoomModelEntity _modelEntity;
        public string Model { get; private set; }

        public int TotalX { get; private set; }
        public int TotalY { get; private set; }
        public int TotalSize { get; private set; }

        public IPoint DoorLocation { get; private set; }

        private IList<IList<RoomTileState>> _tileStates;
        private IList<IList<int>> _tileHeights;

        private static readonly Regex _regex = new(@"\r\n|\r|\n");

        public bool DidGenerate { get; private set; }

        public RoomModel(RoomModelEntity modelEntity)
        {
            _modelEntity = modelEntity;

            Model = CleanModel(modelEntity.Model);

            ResetModel();
        }

        public static string CleanModel(string model)
        {
            if (model == null) return null;

            return regex.Replace(model.ToLower(), "\r").Trim();
        }

        public void ResetModel(bool generate = true)
        {
            TotalX = 0;
            TotalY = 0;
            TotalSize = 0;

            DoorLocation = null;

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

                if ((row == null) || row.Equals("\r")) continue;

                int rowLength = row.Length;

                if (rowLength == 0) continue;

                for (int x = 0; x < totalX; x++)
                {
                    if (rowLength != totalX)
                    {
                        ResetModel(false);

                        return;
                    }

                    string square = rows[y].Substring(x, 1).Trim();

                    if (_tileStates.Count - 1 < x) _tileStates.Add(new List<RoomTileState>());
                    if (_tileHeights.Count - 1 < x) _tileHeights.Add(new List<int>());

                    if (_tileStates[x].Count - 1 < y) _tileStates[x].Add(RoomTileState.Open);
                    if (_tileHeights[x].Count - 1 < y) _tileHeights[x].Add(0);

                    if (square.Equals("x"))
                    {
                        _tileStates[x][y] = RoomTileState.Closed;
                        _tileHeights[x][y] = 0;
                    }
                    else
                    {
                        int index = "abcdefghijklmnopqrstuvwxyz".IndexOf(square);

                        _tileStates[x][y] = RoomTileState.Open;
                        _tileHeights[x][y] = (index == -1) ? Int32.Parse(square) : (index + 10);
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

            IRoomModel roomModel = this;

            RoomTileState doorTileState = GetTileState(_modelEntity.DoorX, _modelEntity.DoorY);
            int doorTileHeight = GetTileHeight(_modelEntity.DoorX, _modelEntity.DoorY);

            if (doorTileState == RoomTileState.Closed)
            {
                ResetModel(false);

                return;
            }

            DoorLocation = new Point(_modelEntity.DoorX, _modelEntity.DoorY, (double)doorTileHeight, (Rotation)_modelEntity.DoorDirection, (Rotation)_modelEntity.DoorDirection);
            DidGenerate = true;
        }

        public RoomTileState GetTileState(int x, int y)
        {
            IList<RoomTileState> rowStates = _tileStates.ElementAtOrDefault(x);

            if (rowStates == null) return RoomTileState.Closed;

            if (rowStates.ElementAtOrDefault(y) != RoomTileState.Open) return RoomTileState.Closed;

            return RoomTileState.Open;
        }

        public int GetTileHeight(int x, int y)
        {
            IList<int> rowHeights = _tileHeights.ElementAtOrDefault(x);

            if (rowHeights == null) return 0;

            return rowHeights.ElementAtOrDefault(y);
        }

        public int Id => _modelEntity.Id;

        public string Name => _modelEntity.Name;
    }
}
