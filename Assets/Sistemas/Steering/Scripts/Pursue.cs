using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pursue : SteeringBehavior
{
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f);

    public StateParameter Future => future;

    public override Vector2 Steer(SteeringAgent agent)
    {
        var delta = agent.TargetPosition - agent.Position;
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta -= target.Velocity * future.CurrentValue;
        Debug.DrawLine(agent.Position, agent.Position + delta, Color.white);
        var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
        yield return Future;
    }

    public Pursue() : base("Pursue", Behavior.Pursue)
    {
    }
}
