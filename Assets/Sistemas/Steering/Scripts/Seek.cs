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
        if (GameManager.line && agent.Strategy.Behaviors[0].IsActive == true)
        {
            agent.lr.enabled = true;
            agent.lr.SetPosition(0, agent.Position);
            agent.lr.SetPosition(1, agent.Position + delta);
        }
        else
        {
            agent.lr.enabled = false;
        }


        //DrawLine(agent.Position, agent.Position + delta, Color.white);
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
