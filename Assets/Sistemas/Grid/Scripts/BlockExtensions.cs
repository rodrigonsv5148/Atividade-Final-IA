using UnityEngine;

public static class BlockExtensions
{
    public static Vector3 EulerAngle(this Block.Rotation rotation) => rotation switch
    {
        Block.Rotation.Default => Vector3.zero,
        Block.Rotation.Left => -90 * Vector3.up,
        Block.Rotation.Down => -180 * Vector3.up,
        Block.Rotation.Right => -270 * Vector3.up,
        _ => Vector3.zero,
    };

    public static Block.Rotation Rotate(this Block.Rotation rotation, Block.Shape shape) => (shape, rotation) switch
    {
        (Block.Shape.L, Block.Rotation.Default) => Block.Rotation.Left,
        (Block.Shape.L, Block.Rotation.Left) => Block.Rotation.Down,
        (Block.Shape.L, Block.Rotation.Down) => Block.Rotation.Right,
        (Block.Shape.L, Block.Rotation.Right) => Block.Rotation.Default,
        (Block.Shape.I, Block.Rotation.Default) => Block.Rotation.Right,
        (Block.Shape.I, Block.Rotation.Right) => Block.Rotation.Default,
        (Block.Shape.Corner, Block.Rotation.Default) => Block.Rotation.Left,
        (Block.Shape.Corner, Block.Rotation.Left) => Block.Rotation.Down,
        (Block.Shape.Corner, Block.Rotation.Down) => Block.Rotation.Right,
        (Block.Shape.Corner, Block.Rotation.Right) => Block.Rotation.Default,
        (_, _) => Block.Rotation.Default
    };
}