using System;
using System.Collections.Generic;
using Turbo.Core.Game.Rooms.Mapping;
using Turbo.Core.Game.Rooms.Object;
using Turbo.Core.Game.Rooms.Utils;
using Turbo.Rooms.Utils;

namespace Turbo.Rooms.Mapping
{
    public class PathFinder : IPathFinder
    {
        private static bool ALLOW_DIAGONALS = true;
        private static double MAX_WALKING_HEIGHT = 2;

        private readonly IRoomMap _roomMap;

        public PathFinder(IRoomMap roomMap)
        {
            _roomMap = roomMap;
        }

        public IList<IPoint> MakePath(IRoomObject roomObject, IPoint location)
        {
            List<IPoint> points = new List<IPoint>();

            IPathFinderNode node = CalculatePathFinderNode(roomObject, location);

            if ((node == null) || (node.NextNode == null)) return points;

            while (node.NextNode != null)
            {
                points.Add(node.Location);

                node = node.NextNode;
            }

            points.Reverse();

            return points;
        }

        private IPathFinderNode CalculatePathFinderNode(IRoomObject roomObject, IPoint location)
        {
            if ((roomObject == null) || (_roomMap == null) || (location == null)) return null;

            location = location.Clone();

            IRoomTile validTile = _roomMap.GetValidTile(roomObject, location);

            if (validTile == null) return null;

            IList<IPathFinderNode> nodes = new List<IPathFinderNode>();
            IDictionary<int, IDictionary<int, IPathFinderNode>> nodeMap = new Dictionary<int, IDictionary<int, IPathFinderNode>>();
            IPathFinderNode nodeGoal = new PathFinderNode(location);

            IPathFinderNode currentNode;
            IPathFinderNode tempNode;
            IPoint tempPoint;

            int cost;
            int difference;

            currentNode = new PathFinderNode(roomObject.Location)
            {
                Cost = 0
            };

            if (!nodeMap.ContainsKey(currentNode.Location.X)) nodeMap.Add(currentNode.Location.X, new Dictionary<int, IPathFinderNode>());

            nodeMap[currentNode.Location.X].Add(currentNode.Location.Y, currentNode);

            nodes.Add(currentNode);

            IReadOnlyCollection<IPoint> walkingPoints = ALLOW_DIAGONALS ? MovePoints.DiagonalPoints : MovePoints.StandardPoints;

            if (walkingPoints.Count == 0) return null;

            while (nodes.Count > 0)
            {
                currentNode = nodes[0];

                nodes.Remove(currentNode);

                currentNode.IsClosed = true;

                foreach (IPoint walkingPoint in walkingPoints)
                {
                    tempPoint = currentNode.Location.AddPoint(walkingPoint);

                    if (!IsValidStep(roomObject, currentNode.Location, tempPoint, nodeGoal.Location)) continue;

                    if (!nodeMap.ContainsKey(tempPoint.X)) nodeMap.Add(tempPoint.X, new Dictionary<int, IPathFinderNode>());

                    if (!nodeMap[tempPoint.X].ContainsKey(tempPoint.Y))
                    {
                        tempNode = new PathFinderNode(tempPoint);
                        nodeMap[tempPoint.X].Add(tempPoint.Y, tempNode);
                    }
                    else
                    {
                        tempNode = nodeMap[tempPoint.X][tempPoint.Y];
                    }

                    if (tempNode.IsClosed) continue;

                    difference = 0;

                    if (currentNode.Location.X != tempNode.Location.X) difference += 2;
                    if (currentNode.Location.Y != tempNode.Location.Y) difference += 2;

                    cost = ((currentNode.Cost + difference) + tempNode.Location.GetDistanceAround(nodeGoal.Location));

                    if (tempNode.IsOpen) continue;

                    if (cost < tempNode.Cost)
                    {
                        tempNode.Cost = cost;
                        tempNode.NextNode = currentNode;
                    }

                    if (tempNode.Location.Compare(nodeGoal.Location))
                    {
                        tempNode.NextNode = currentNode;

                        return tempNode;
                    }

                    tempNode.IsOpen = true;

                    nodes.Add(tempNode);
                }
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

            if (Math.Abs(nextHeight - currentHeight) > Math.Abs(MAX_WALKING_HEIGHT)) return false;

            if (ALLOW_DIAGONALS && !location.Compare(nextLocation))
            {
                bool isSideValid = (_roomMap.GetValidDiagonalTile(roomObject, new Point(nextLocation.X, location.Y)) != null);
                bool isOtherSideValid = (_roomMap.GetValidDiagonalTile(roomObject, new Point(location.X, nextLocation.Y)) != null);

                if (!isSideValid && !isOtherSideValid) return false;
            }

            if (nextTile.Location.Compare(roomObject.Location)) return true;

            if (!isGoal && (nextTile.CanSit() || nextTile.CanLay())) return false;

            return true;
        }
    }
}
