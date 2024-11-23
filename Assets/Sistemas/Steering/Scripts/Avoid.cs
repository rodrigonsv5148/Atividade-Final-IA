using System.Collections.Generic;
using UnityEngine;

public class Avoid : SteeringBehavior
{
    [SerializeField] StateParameter trackingRadius = new StateParameter("Tracking Radius", 1f, 60f, 25f);
    [SerializeField] StateParameter avoidFactor = new StateParameter("Separation", 1f, 2f, 1f);
    [SerializeField] StateParameter avoidWindow = new StateParameter("Time Window", 1f, 10f, 2f);

    public StateParameter ObstacleTrackingRadius => trackingRadius;
    public StateParameter AvoidFactor => avoidFactor;
    public StateParameter AvoidWindow => avoidWindow;

    Collider2D[] overlap = new Collider2D[32];
    public override Vector2 Steer(SteeringAgent agent)
    {
        int found = Physics2D.OverlapCircleNonAlloc
        (
            point: agent.Position,
            radius: trackingRadius.CurrentValue,
            results: overlap
        );

        (IObstacle obstacle, float timeToCollision, Vector2 relativePosition, Vector2 relativeVelocity) highestPriority = (null, float.PositiveInfinity, Vector2.positiveInfinity, Vector2.positiveInfinity);

        foreach (var ufo in overlap[..found])
        {
            if (ufo.gameObject == agent.gameObject || ufo.gameObject == agent.Target.gameObject)
                continue;

            if (!ufo.TryGetComponent<IObstacle>(out var obstacle))
                continue;

            var relativePosition = obstacle.Position - agent.Position;
            var distance = relativePosition.magnitude;
            var relativeVelocity = obstacle.Velocity - agent.Velocity;
            var relativeSpeed = relativeVelocity.magnitude;
            var combinedRadius = agent.Radius + obstacle.Radius;

            var timeToClosestApproach = -Vector2.Dot(relativePosition, relativeVelocity) / (relativeSpeed * relativeSpeed);
            var timeToCollision = (distance - combinedRadius) / relativeSpeed;
            var minimumSeparation = distance - relativeSpeed * timeToClosestApproach;
            var necessarySeparation = avoidFactor.CurrentValue * combinedRadius;

            if (minimumSeparation > necessarySeparation)
                continue;

            if (timeToClosestApproach > 0f && timeToClosestApproach < highestPriority.timeToCollision)
                highestPriority = (obstacle, timeToCollision, relativePosition, relativeVelocity);
        }

        if (highestPriority.obstacle == null)
            return Vector2.zero;

        var futureRelativePosition = highestPriority.relativePosition + highestPriority.relativeVelocity * highestPriority.timeToCollision;
        var closenessFactor = 1f - Mathf.Clamp01(highestPriority.timeToCollision / avoidWindow.CurrentValue);
        var desiredVelocity = -futureRelativePosition.normalized * closenessFactor * agent.MaxSpeed.CurrentValue;
        var desiredAcceleration = desiredVelocity - agent.Velocity;

        Debug.DrawLine(agent.Position, highestPriority.obstacle.Position, Color.blue);
        Debug.DrawLine(agent.Position, agent.Position + desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue, Color.yellow);

        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
        yield return ObstacleTrackingRadius;
        yield return AvoidWindow;
    }

    public Avoid() : base("Avoid", Behavior.Avoid)
    {
    }
}
