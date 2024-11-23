using UnityEngine;

public abstract class BlockBase : MonoBehaviour
{
    public abstract void RegisterTriggerEnter(object emitter, Collider other);
    public abstract void RegisterTriggerExit(object emitter, Collider other);
}