using System.Collections.Generic;

namespace Pathfinding
{
    public interface INode<T>
    {
        IEnumerable<(T node, int cost)> Neighbors { get; }
        int GetHeuristicCostTo(T target);
    }
}