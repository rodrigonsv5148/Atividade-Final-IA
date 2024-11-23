using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class InputManager : MonoBehaviour, ICommandListener
{
    public static InputManager instance;

    public List<ICommandObserver> Observers { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Observers = new List<ICommandObserver>(); // O ideal seria receber através de injeção.
    }

    private InputManager() { }

    public void Subscribe(ICommandObserver addObserver)
    {
        Observers.Add(addObserver);
    }
    public void UnSubscribe(ICommandObserver removeObserver)
    {
        Observers.Remove(removeObserver);

    }
    public void Notify(ICommand command)
    {
        if (Time.timeScale < 1) return;
        foreach (ICommandObserver item in Observers)
        {
            item.ExecuteCommand(command);
        }
    }

}
