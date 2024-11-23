using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Flee : SteeringBehavior
{
    [SerializeField] StateParameter radius = new StateParameter("Radius", 10f, 200f, 25f);

    public StateParameter Radius => radius;

    public override Vector2 Steer(SteeringAgent agent)
    {
        var delta = agent.Position - agent.TargetPosition;
        var desiredVelocity = delta.magnitude < radius.CurrentValue ? delta.normalized * agent.MaxSpeed.CurrentValue : agent.Velocity;
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
        yield return Radius;
    }

    public Flee() : base("Flee", Behavior.Flee)
    {
    }
}
