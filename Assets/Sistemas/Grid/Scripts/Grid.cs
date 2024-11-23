using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;

public class Grid : MonoBehaviour
{
    [SerializeField] BlockLibrary blockLibrary;
    [SerializeField] Node nodePrefab;
    [SerializeField] int gridSize = 10;
    [SerializeField, HideInInspector] LineRenderer line;
    Node[,] grid;
    Vector3[] linePath;

    void Start()
    {
        Block.OnDropped += HandleBlockDropped;

        grid = new Node[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y] = Instantiate
                (
                    original: nodePrefab,
                    position: new Vector3(x, 0, y),
                    rotation: Quaternion.identity,
                    parent: transform
                );
                grid[x, y].Setup(new Vector2Int(x, y));
            }
        }
    }

    public void UpdateGrid(Node[] path, PathPoint<Node>[] points)
    {
        foreach (var node in grid)
            node.Display.Reset();

        foreach (var point in points)
            point.Node.Display.UpdateInformation(point);

        linePath = path.Select(n => n.transform.position).ToArray();
        line.positionCount = linePath.Length;
        line.SetPositions(linePath);
        line.enabled = linePath.Length != 0;
    }

    public void ClearGrid()
    {
        foreach (var node in grid)
            node.Display.Reset();

        line.enabled = false;
    }

    void HandleBlockDropped(Block block)
    {
        var prefab = blockLibrary.GetPlacedBlockPrefab(block.Profile);
        PlacedBlock placedBlock = Instantiate
        (
            original: prefab,
            position: new Vector3(block.TargetPosition.x, 0f, block.TargetPosition.z),
            rotation: Quaternion.Euler(block.LookRotation.EulerAngle()),
            parent: transform
        );
    }

    void OnDestroy()
    {
        Block.OnDropped -= HandleBlockDropped;
    }

    void OnValidate()
    {
        line = GetComponent<LineRenderer>();
    }
}
