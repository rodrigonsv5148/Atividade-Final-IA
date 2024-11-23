using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandObserver
{
    void ExecuteCommand(ICommand command);
}