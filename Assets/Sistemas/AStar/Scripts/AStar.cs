using System.Collections.Generic;
using System.Linq;

namespace Pathfinding
{
    public static class AStar<T> where T : INode<T>
    {
        public static List<T> FindPath(T origin, T destination)
        {
            var path = new List<T>();
            _ = FindPathIterator(origin, destination, path).ToList();
            return path;
        }

        public static IEnumerable<PathPoint<T>> FindPathInteractively(T origin, T destination, out List<T> path)
        {
            path = new List<T>();
            return FindPathIterator(origin, destination, path);
        }

        public static (List<T[]> paths, List<PathPoint<T>[]> points) FindPathHistory(T origin, T destination)
        {
            var currentPath = new List<T>();
            var pointSet = new HashSet<PathPoint<T>>(new PathPointComparer<T>());

            var paths = new List<T[]>() { currentPath.ToArray() };
            var points = new List<PathPoint<T>[]>() { pointSet.ToArray() };

            foreach (var point in FindPathIterator(origin, destination, currentPath))
            {
                pointSet.Remove(point);
                pointSet.Add(point);

                paths.Add(currentPath.ToArray());
                points.Add(pointSet.ToArray());
            }

            return (paths, points);
        }

        static IEnumerable<PathPoint<T>> FindPathIterator(T origin, T destination, List<T> path)
        {
            var frontier = new HashSet<PathPoint<T>>();
            var visited = new HashSet<PathPoint<T>>();

            var startingPoint = new PathPoint<T>
            (
                node: origin,
                parent: null,
                state: Status.Frontier,
                cumulativeCost: 0,
                heuristic: origin.GetHeuristicCostTo(destination)
            );

            frontier.Add(startingPoint);

            while (frontier.Count > 0)
            {
                PathPoint<T> current = frontier.First();
                foreach (var pathpoint in frontier)
                {
                    if (pathpoint < current)
                        current = pathpoint;
                }

                frontier.Remove(current);
                current = current.With(Status.Visited);
                visited.Add(current);

                {
                    path.Clear();
                    var pathCurrent = current;
                    while (pathCurrent.Parent != null)
                    {
                        path.Add(pathCurrent.Node);
                        pathCurrent = pathCurrent.Parent;
                    }
                    path.Add(origin);
                    path.Reverse();
                }

                yield return current;

                if (current.Node.Equals(destination))
                    break;

                foreach (var (neighbor, movementCost) in current.Node.Neighbors)
                {
                    if (visited.Any(p => p.Node.Equals(neighbor)))
                        continue;

                    var neighborPoint = new PathPoint<T>
                    (
                        node: neighbor,
                        parent: current,
                        state: Status.Frontier,
                        cumulativeCost: current.CumulativeCost + movementCost,
                        heuristic: neighbor.GetHeuristicCostTo(destination)
                    );

                    var existingNeighborPoint = frontier.FirstOrDefault(p => p.Node.Equals(neighbor));

                    PathPoint<T> bestNeighbor;
                    switch (neighborPoint, existingNeighborPoint)
                    {
                        case ({ }, { }) when existingNeighborPoint < neighborPoint:
                            bestNeighbor = existingNeighborPoint;
                            break;
                        case ({ }, { }) when existingNeighborPoint > neighborPoint:
                            bestNeighbor = neighborPoint;
                            frontier.Remove(existingNeighborPoint);
                            break;
                        default:
                            bestNeighbor = neighborPoint;
                            break;
                    }

                    frontier.Add(bestNeighbor);
                    yield return bestNeighbor;
                }
            }
        }
    }
}
