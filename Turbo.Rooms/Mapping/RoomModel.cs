using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Database.Entities.Room;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping;

public class RoomModel : IRoomModel
{
    private static readonly Regex _regex = new(@"\r\n|\r|\n");

    private readonly RoomModelEntity _modelEntity;
    private IList<IList<int>> _tileHeights;

    private IList<IList<RoomTileState>> _tileStates;

    public RoomModel(RoomModelEntity modelEntity)
    {
        _modelEntity = modelEntity;

        Model = CleanModel(modelEntity.Model);

        ResetModel();
    }

    public string Model { get; }

    public int TotalX { get; private set; }
    public int TotalY { get; private set; }
    public int TotalSize { get; private set; }

    public IPoint DoorLocation { get; private set; }

    public bool IsValid { get; private set; }

    public void Generate()
    {
        var rows = Model.Split("\r");
        var totalX = rows[0].Length;
        var totalY = rows.Length;

        if (rows.Length <= 0 || totalX <= 0 || totalY <= 0)
        {
            ResetModel(false);

            return;
        }

        for (var y = 0; y < totalY; y++)
        {
            var row = rows[y];

            if (row == null || row.Equals("\r")) continue;

            var rowLength = row.Length;

            if (rowLength == 0) continue;

            for (var x = 0; x < totalX; x++)
            {
                if (rowLength != totalX)
                {
                    ResetModel(false);

                    return;
                }

                var square = rows[y].Substring(x, 1).Trim();

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
                    var index = "abcdefghijklmnopqrstuvwxyz".IndexOf(square);

                    _tileStates[x][y] = RoomTileState.Open;
                    _tileHeights[x][y] = index == -1 ? int.Parse(square) : index + 10;
                }

                TotalSize++;
            }
        }

        if (TotalSize != totalX * totalY)
        {
            ResetModel(false);

            return;
        }

        TotalX = totalX;
        TotalY = totalY;

        var doorTileState = GetTileState(_modelEntity.DoorX, _modelEntity.DoorY);
        var doorTileHeight = GetTileHeight(_modelEntity.DoorX, _modelEntity.DoorY);

        if (doorTileState == RoomTileState.Closed)
        {
            ResetModel(false);

            return;
        }

        DoorLocation = new Point(_modelEntity.DoorX, _modelEntity.DoorY, doorTileHeight, _modelEntity.DoorRotation,
            _modelEntity.DoorRotation);
        IsValid = true;
    }

    public RoomTileState GetTileState(int x, int y)
    {
        var rowStates = _tileStates.ElementAtOrDefault(x);

        if (rowStates == null) return RoomTileState.Closed;

        if (rowStates.ElementAtOrDefault(y) != RoomTileState.Open) return RoomTileState.Closed;

        return RoomTileState.Open;
    }

    public int GetTileHeight(int x, int y)
    {
        var rowHeights = _tileHeights.ElementAtOrDefault(x);

        if (rowHeights == null) return 0;

        return rowHeights.ElementAtOrDefault(y);
    }

    public int Id => _modelEntity.Id;

    public string Name => _modelEntity.Name;

    public static string CleanModel(string model)
    {
        if (model == null) return null;

        return _regex.Replace(model.ToLower(), "\r").Trim();
    }

    public void ResetModel(bool generate = true)
    {
        TotalX = 0;
        TotalY = 0;
        TotalSize = 0;

        DoorLocation = null;

        _tileStates = new List<IList<RoomTileState>>();
        _tileHeights = new List<IList<int>>();

        IsValid = false;

        if (generate) Generate();
    }
}