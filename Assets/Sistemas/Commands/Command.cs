using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command : ScriptableObject, ICommand
{
    public float amount;
    public abstract void Execute(ICommand command, Transform observerTransform);
}
