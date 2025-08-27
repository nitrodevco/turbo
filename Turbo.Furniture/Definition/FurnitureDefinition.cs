using System;
using System.Collections.Generic;
using System.Globalization;
using Turbo.Core.Game;
using Turbo.Core.Game.Furniture.Constants;
using Turbo.Core.Game.Furniture.Definition;
using Turbo.Database.Entities.Furniture;

namespace Turbo.Furniture.Definition;

public class FurnitureDefinition(FurnitureDefinitionEntity _entity) : IFurnitureDefinition
{
    public int Id => _entity.Id;
    public int SpriteId => _entity.SpriteId;
    public string PublicName => _entity.PublicName;
    public string ProductName => _entity.ProductName;
    public string Type => _entity.Type;
    public string Logic => _entity.Logic;
    public int TotalStates => _entity.TotalStates;
    public int X => _entity.X;
    public int Y => _entity.Y;

    public double Z
    {
        get
        {
            if (_entity.Z == 0) return DefaultSettings.MinimumStackHeight;

            return _entity.Z;
        }
    }

    private IDictionary<int, double> _multiHeightsCache;

    public IDictionary<int, double> MultiHeights
    {
        get
        {
            if (_multiHeightsCache is not null) return _multiHeightsCache;

            var result = new Dictionary<int, double>();
            var raw = _entity.MultiHeights;

            if (string.IsNullOrWhiteSpace(raw))
            {
                _multiHeightsCache = result;
                return _multiHeightsCache;
            }

            var parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            for (var i = 0; i < parts.Length; i++)
            {
                var token = parts[i];

                if (!double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                    continue;

                value = Math.Round(value, 3);
                if (value < 0) value = 0;
                if (value > DefaultSettings.MaximumFurnitureHeight) value = DefaultSettings.MaximumFurnitureHeight;

                if (value == 0) value = DefaultSettings.MinimumStackHeight;

                result[i] = value;
            }

            _multiHeightsCache = result;
            return _multiHeightsCache;
        }
    }

    public bool CanStack => (bool)_entity.CanStack;
    public bool CanWalk => (bool)_entity.CanWalk;
    public bool CanSit => (bool)_entity.CanSit;
    public bool CanLay => (bool)_entity.CanLay;
    public bool CanRecycle => (bool)_entity.CanRecycle;
    public bool CanTrade => (bool)_entity.CanTrade;
    public bool CanGroup => (bool)_entity.CanGroup;
    public bool CanSell => (bool)_entity.CanSell;
    public FurniUsagePolicy UsagePolicy => _entity.UsagePolicy;
    public string ExtraData => _entity.ExtraData;
}