using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.PathFinder.Constants;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.PathFinder
{
    public class PathFinder : IPathFinder
    {
        private readonly IRoomMap _roomMap;

        private HeuristicFormula _heuristicFormula = HeuristicFormula.Manhattan;
        private bool _allowDiagonals = true;
        private bool _heavyDiagonals = true;
        private int _heuristicEstimate = 2;
        private double _maxHeight = 2;
        private bool _tieBreaker;

        public PathFinder(IRoomMap roomMap)
        {
            _roomMap = roomMap;
        }

        public IList<IPoint> MakePath(IRoomObject roomObject, IPoint location)
        {
            List<IPoint> points = new();

            IList<PathFinderNode> nodes = CalculatePathFinderNode(roomObject, location);

            if(nodes != null)
            {
                if(nodes.Count > 0)
                {
                    nodes.RemoveAt(0);

                    foreach (PathFinderNode node in nodes)
                    {
                        points.Add(new Point(node.Location.X, node.Location.Y));
                    }
                }
            }

            return points;
        }

        private IList<PathFinderNode> CalculatePathFinderNode(IRoomObject roomObject, IPoint location)
        {
            if ((roomObject == null) || (_roomMap == null) || (location == null)) return null;

            location = location.Clone();

            IRoomTile validTile = _roomMap.GetValidTile(roomObject, location);

            if (validTile == null) return null;

            PriorityQueue<PathFinderNode> openNodes = new(new NodeComparer());
            List<PathFinderNode> closedNodes = new();

            PathFinderNode currentNode;

            currentNode.G = 0;
            currentNode.H = _heuristicEstimate;
            currentNode.F = currentNode.G + currentNode.H;
            currentNode.Location = new Point(roomObject.Location);
            currentNode.LocationParent = new Point(currentNode.Location);

            openNodes.Push(currentNode);

            IReadOnlyCollection<IPoint> walkingPoints = _allowDiagonals ? MovePoints.DiagonalPoints : MovePoints.StandardPoints;

            if (walkingPoints.Count == 0) return null;

            while (openNodes.Count > 0)
            {
                currentNode = openNodes.Pop();

                if(currentNode.Location.X == location.X && currentNode.Location.Y == location.Y)
                {
                    closedNodes.Add(currentNode);

                    PathFinderNode fNode = closedNodes[closedNodes.Count - 1];

                    for (int i = closedNodes.Count - 1; i >= 0; i--)
                    {
                        if (fNode.LocationParent.X == closedNodes[i].Location.X && fNode.LocationParent.Y == closedNodes[i].Location.Y || i == closedNodes.Count - 1)
                        {
                            fNode = closedNodes[i];
                        }
                        else closedNodes.RemoveAt(i);
                    }

                    return closedNodes;
                }

                foreach (IPoint walkingPoint in walkingPoints)
                {
                    PathFinderNode tempNode;

                    tempNode.Location = currentNode.Location.AddPoint(walkingPoint);

                    if (!IsValidStep(roomObject, currentNode.Location, tempNode.Location, location)) continue;

                    int gCost;

                    if(_heavyDiagonals && (walkingPoint.X != 0 && walkingPoint.Y != 0))
                    {
                        gCost = currentNode.G + (int)(_roomMap.Map[tempNode.Location.X, tempNode.Location.Y] * 2.41);
                    }
                    else
                    {
                        gCost = currentNode.G + _roomMap.Map[tempNode.Location.X, tempNode.Location.Y];
                    }

                    if (gCost == currentNode.G) continue;

                    int foundInOpenIndex = -1;

                    for(int i = 0; i < openNodes.Count; i++)
                    {
                        if(openNodes[i].Location.X == tempNode.Location.X && openNodes[i].Location.Y == tempNode.Location.Y)
                        {
                            foundInOpenIndex = i;

                            break;
                        }
                    }

                    if (foundInOpenIndex > -1 && openNodes[foundInOpenIndex].G <= gCost) continue;

                    int foundInClosedIndex = -1;

                    for (int i = 0; i < closedNodes.Count; i++)
                    {
                        if (closedNodes[i].Location.X == tempNode.Location.X && closedNodes[i].Location.Y == tempNode.Location.Y)
                        {
                            foundInClosedIndex = i;

                            break;
                        }
                    }

                    if (foundInClosedIndex > -1 && closedNodes[foundInClosedIndex].G <= gCost) continue;

                    tempNode.LocationParent = currentNode.Location;
                    tempNode.G = gCost;

                    switch (_heuristicFormula)
                    {
                        default:
                        case HeuristicFormula.Manhattan:
                            tempNode.H = _heuristicEstimate * (Math.Abs(tempNode.Location.X - location.X) + Math.Abs(tempNode.Location.Y - location.Y));
                            break;
                        case HeuristicFormula.MaxDXDY:
                            tempNode.H = _heuristicEstimate * (Math.Max(Math.Abs(tempNode.Location.X - location.X), Math.Abs(tempNode.Location.Y - location.Y)));
                            break;
                        case HeuristicFormula.DiagonalShortCut:
                            int h_diagonal = Math.Min(Math.Abs(tempNode.Location.X - location.X), Math.Abs(tempNode.Location.Y - location.Y));
                            int h_straight = (Math.Abs(tempNode.Location.X - location.X) + Math.Abs(tempNode.Location.Y - location.Y));
                            tempNode.H = (_heuristicEstimate * 2) * h_diagonal + _heuristicEstimate * (h_straight - 2 * h_diagonal);
                            break;
                        case HeuristicFormula.Euclidean:
                            tempNode.H = (int)(_heuristicEstimate * Math.Sqrt(Math.Pow((tempNode.Location.X - location.X), 2) + Math.Pow((tempNode.Location.Y - location.Y), 2)));
                            break;
                        case HeuristicFormula.EuclideanNoSQR:
                            tempNode.H = (int)(_heuristicEstimate * (Math.Pow((tempNode.Location.X - location.X), 2) + Math.Pow((tempNode.Location.Y - location.Y), 2)));
                            break;
                    }

                    if (_tieBreaker)
                    {
                        int dx1 = currentNode.Location.X - location.X;
                        int dy1 = currentNode.Location.Y - location.Y;
                        int dx2 = roomObject.Location.X - location.X;
                        int dy2 = roomObject.Location.Y - location.Y;
                        int cross = Math.Abs(dx1 * dy2 - dx2 * dy1);
                        tempNode.H = (int)(tempNode.H + cross * 0.001);
                    }

                    tempNode.F = tempNode.G + tempNode.H;

                    openNodes.Push(tempNode);
                }

                closedNodes.Add(currentNode);
            }

            return null;
        }

        public bool IsValidStep(IRoomObject roomObject, IPoint location, IPoint nextLocation, IPoint goalLocation)
        {
            if ((_roomMap == null) || (roomObject == null) || (location == null) || (nextLocation == null) || (goalLocation == null)) return false;

            bool isGoal = nextLocation.Compare(goalLocation);
            IRoomTile currentTile = _roomMap.GetValidTile(roomObject, location, false);
            IRoomTile nextTile = _roomMap.GetValidTile(roomObject, nextLocation, isGoal);

            if (((currentTile == null) || (nextTile == null)) || (nextTile.IsDoor && !isGoal)) return false;

            double currentHeight = currentTile.GetWalkingHeight();
            double nextHeight = nextTile.GetWalkingHeight();

            if (Math.Abs(nextHeight - currentHeight) > Math.Abs(_maxHeight)) return false;

            if (_allowDiagonals && !location.Compare(nextLocation))
            {
                bool isSideValid = (_roomMap.GetValidDiagonalTile(roomObject, new Point(nextLocation.X, location.Y)) != null);
                bool isOtherSideValid = (_roomMap.GetValidDiagonalTile(roomObject, new Point(location.X, nextLocation.Y)) != null);

                if (!isSideValid && !isOtherSideValid) return false;
            }

            if (nextTile.Location.Compare(roomObject.Location)) return true;

            if (!isGoal && (nextTile.CanSit(roomObject) || nextTile.CanLay(roomObject))) return false;

            return true;
        }
    }
}
