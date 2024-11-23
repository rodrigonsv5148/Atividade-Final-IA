using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Seek : SteeringBehavior
{
    public override Vector2 Steer(SteeringAgent agent)
    {
        var delta = agent.TargetPosition - agent.Position;
        var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
    }

    public Seek() : base("Seek", Behavior.Seek)
    {
    }
}
