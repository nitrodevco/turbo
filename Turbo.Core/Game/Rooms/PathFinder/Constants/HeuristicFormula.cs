using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.Core.Game.Rooms.PathFinder.Constants
{
    public enum HeuristicFormula
    {
        Manhattan,
        MaxDXDY,
        DiagonalShortCut,
        Euclidean,
        EuclideanNoSQR,
        ReverseDijkstra
    }
}
