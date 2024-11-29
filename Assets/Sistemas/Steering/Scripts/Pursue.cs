using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pursue : SteeringBehavior
{
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f); // Isso aqui é o da hud
    
    public StateParameter Future => future;
    public float time;

    public override Vector2 Steer(SteeringAgent agent)
    {
        var delta = agent.TargetPosition - agent.Position;
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta += target.Velocity * future.CurrentValue;
        //Debug.DrawLine(agent.Position, agent.Position + delta, Color.white);
        var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        
        if (GameManager.line && agent.Strategy.Behaviors[1].IsActive == true)
        {
            agent.lrp.enabled = true;
            agent.lrp.SetPosition(0, agent.Position);
            agent.lrp.SetPosition(1, agent.Position + delta);

        }
        else
        {
            agent.lrp.enabled = false;
        }
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
