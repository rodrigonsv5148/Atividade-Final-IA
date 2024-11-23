using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCommandObserver : MonoBehaviour, ICommandObserver
{
    public List<Command> playerCommands;

    private void Start()
    {
        InputManager.instance.Subscribe(this);
    }
    private void OnDestroy()
    {
        InputManager.instance.UnSubscribe(this);
    }

    public void ExecuteCommand(ICommand command)
    {
        foreach (ICommand item in playerCommands)
        {
            item.Execute(command, this.transform);
        }
    }
}