using UnityEngine;

[CreateAssetMenu(menuName = "Command/Generic")]
public class GenericCommand : Command
{
    public override void Execute(ICommand command, Transform observerTransform)
    {
        if (command.GetType() != this.GetType()) return;

        DoSomething(observerTransform);
    }

    void DoSomething(Transform transformToMove)
    {

    }
}