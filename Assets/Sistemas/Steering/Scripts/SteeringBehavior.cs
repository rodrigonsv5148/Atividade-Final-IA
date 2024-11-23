using System;
using System.Collections.Generic;
using UnityEngine;

public enum Behavior { None, Seek, Arrive, Avoid, Evade, Pursue, Flee }

[Serializable]
public abstract class SteeringBehavior
{
    [SerializeField] bool isActive = true;
    [SerializeField] StateParameter weight = new StateParameter("Weight", 0.1f, 2f, 1f);
    [SerializeField] Behavior type;
    [SerializeField, HideInInspector] string name;

    public string Name => name;
    public StateParameter Weight => weight;
    public bool IsActive { get => isActive; set => isActive = value; }
    public Behavior Type => type;

    protected SteeringBehavior(string name, Behavior type)
    {
        this.name = name;
        this.type = type;
    }

    public abstract Vector2 Steer(SteeringAgent agent);

    public abstract IEnumerable<StateParameter> GetParameters();
}
