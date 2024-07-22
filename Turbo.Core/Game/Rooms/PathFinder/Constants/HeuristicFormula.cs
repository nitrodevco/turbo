namespace Turbo.Core.Game.Rooms.PathFinder.Constants;

public enum HeuristicFormula
{
    Manhattan,
    MaxDXDY,
    DiagonalShortCut,
    Euclidean,
    EuclideanNoSQR,
    ReverseDijkstra
}