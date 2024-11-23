using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandListener
{
    List<ICommandObserver> Observers { get; set; }
    void Subscribe(ICommandObserver addObserver);
    void UnSubscribe(ICommandObserver RemoveObserver);
    void Notify(ICommand command);
}