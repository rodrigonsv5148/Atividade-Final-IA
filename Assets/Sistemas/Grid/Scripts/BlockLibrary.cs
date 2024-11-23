using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "block-library", menuName = "Block/BlockLibrary")]
public class BlockLibrary : ScriptableObject
{
    [SerializeField] BlockPrefabTuple[] blockPrefabTuples;
    [SerializeField] PlacedBlockPrefabTuple[] placedBlockPrefabTuples;

    public Block GetBlockPrefab(Block.Shape profile)
    {
        return blockPrefabTuples.First(t => t.Profile == profile).Prefab;
    }

    public PlacedBlock GetPlacedBlockPrefab(Block.Shape profile)
    {
        return placedBlockPrefabTuples.First(t => t.Profile == profile).Prefab;
    }

    [Serializable]
    public class BlockPrefabTuple
    {
        public Block.Shape Profile;
        public Block Prefab;
    }

    [Serializable]
    public class PlacedBlockPrefabTuple
    {
        public Block.Shape Profile;
        public PlacedBlock Prefab;
    }
}


