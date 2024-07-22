using System;
using System.Collections.Generic;
using Turbo.Core.Game.Furniture.Data;

namespace Turbo.Core.Game.Rooms.Object.Logic;

public interface IRoomObjectLogicFactory
{
    public IDictionary<string, Type> Logics { get; }
    public IRoomObjectLogic Create(string type);
    public Type GetLogicType(string type);
    public StuffDataKey GetStuffDataKeyForFurnitureType(string type);
}