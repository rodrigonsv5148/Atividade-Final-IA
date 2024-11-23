using UnityEngine;

public class BlockTrigger : MonoBehaviour
{
    [SerializeField] BlockBase block;

    public BlockBase Block => block;

    public void OnTriggerEnter(Collider other)
    {
        block.RegisterTriggerEnter(this, other);
    }

    public void OnTriggerExit(Collider other)
    {
        block.RegisterTriggerExit(this, other);
    }

    void OnValidate()
    {
        block = transform.parent.GetComponent<BlockBase>();
    }
}
