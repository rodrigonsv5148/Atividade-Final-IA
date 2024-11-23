using System.Collections.Generic;

namespace Pathfinding
{
    public enum Status { Frontier, Visited }

    public class PathPoint<T> where T : INode<T>
    {
        public T Node { get; }
        public PathPoint<T> Parent { get; }
        public Status State { get; }
        public int CumulativeCost { get; }
        public int Heuristic { get; }

        public int Score => CumulativeCost + Heuristic;

        public PathPoint(T node, PathPoint<T> parent, Status state, int cumulativeCost, int heuristic)
        {
            Node = node;
            Parent = parent;
            State = state;
            CumulativeCost = cumulativeCost;
            Heuristic = heuristic;
        }

        public PathPoint<T> With(Status state)
        {
            return new PathPoint<T>
            (
                node: Node,
                parent: Parent,
                state: state,
                cumulativeCost: CumulativeCost,
                heuristic: Heuristic
            );
        }

        public static bool operator <(PathPoint<T> lhs, PathPoint<T> rhs)
        {
            if (lhs.Score == rhs.Score)
                return lhs.Heuristic < rhs.Heuristic;
            else
                return lhs.Score < rhs.Score;
        }

        public static bool operator >(PathPoint<T> lhs, PathPoint<T> rhs)
        {
            if (lhs.Score == rhs.Score)
                return lhs.Heuristic > rhs.Heuristic;
            else
                return lhs.Score > rhs.Score;
        }
    }

    public class PathPointComparer<T> : EqualityComparer<PathPoint<T>> where T : INode<T>
    {
        public override bool Equals(PathPoint<T> lhs, PathPoint<T> rhs)
        {
            return lhs.Node.Equals(rhs.Node);
        }

        public override int GetHashCode(PathPoint<T> pathPoint)
        {
            return pathPoint?.Node?.GetHashCode() ?? 0;
        }
    }
}
