using UnityEngine;

public interface ICommand
{
    void Execute(ICommand command, Transform observerTransform);
}