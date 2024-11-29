using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Arrive : SteeringBehavior
{
    [SerializeField] StateParameter slowRadius = new StateParameter("Slow Radius", 10f, 100f, 50f);
    [SerializeField] StateParameter arriveRadius = new StateParameter("Arrive Radius", 1f, 50f, 15f);

    public StateParameter SlowRadius => slowRadius;
    public StateParameter ArriveRadius => arriveRadius;

    public override Vector2 Steer(SteeringAgent agent)
    {
        var delta = agent.Position - agent.TargetPosition;
        var brake = 1f - Mathf.Clamp01((delta.magnitude - arriveRadius.CurrentValue) / (slowRadius.CurrentValue - arriveRadius.CurrentValue));
        var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        var desiredAcceleration = brake * (desiredVelocity - agent.Velocity);
        if (GameManager.line && agent.Strategy.Behaviors[5].IsActive == true)
        {
            agent.lrar.enabled = true;
            agent.lrar.SetPosition(0, agent.Position);
            agent.lrar.SetPosition(1, agent.Position + desiredAcceleration);
        }
        else
        {
            agent.lrar.enabled = false;
        }
        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
        yield return SlowRadius;
        yield return ArriveRadius;
    }

    public Arrive() : base("Arrive", Behavior.Arrive)
    {
    }
}
