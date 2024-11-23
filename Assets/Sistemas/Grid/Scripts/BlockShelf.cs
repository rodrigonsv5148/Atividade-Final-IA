using UnityEngine;

public class BlockShelf : MonoBehaviour
{
    [SerializeField] BlockLibrary blockLibrary;

    void Start()
    {
        Block.OnDropped += HandleBlockDropped;
    }

    private void HandleBlockDropped(Block block)
    {
        var prefab = blockLibrary.GetBlockPrefab(block.Profile);
        Block placedBlock = Instantiate
        (
            original: prefab,
            position: block.StartPosition,
            rotation: Quaternion.Euler(block.LookRotation.EulerAngle()),
            parent: transform
        );
        placedBlock.LookRotation = block.LookRotation;
    }
}
