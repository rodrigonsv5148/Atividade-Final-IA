using System;
using System.Collections.Generic;
using UnityEngine;

public enum PriorityChange { Up = -1, Down = +1 }

[Serializable]
public class SteeringStrategy
{
    [SerializeReference] List<SteeringBehavior> behaviors;
    [SerializeField] StateParameter maxAcceleration = new StateParameter("Maximum Acceleration", 1f, 100f, 20f);
    [SerializeField] StateParameter maxSpeed = new StateParameter("Maximum Speed", 1f, 100f, 12f);

    public List<SteeringBehavior> Behaviors { get => behaviors; set => behaviors = value; }
    public StateParameter MaxAcceleration => maxAcceleration;
    public StateParameter MaxSpeed => maxSpeed;

    public (int index, int targetIndex) AlterPriority(SteeringBehavior behavior, PriorityChange change)
    {
        var index = behaviors.FindIndex(b => b == behavior);
        var targetIndex = index switch
        {
            -1 => index,
            _ => Mathf.Clamp(index + (int)change, 0, behaviors.Count - 1)
        };

        if (index != targetIndex)
            (behaviors[index], behaviors[targetIndex]) = (behaviors[targetIndex], behaviors[index]);

        return (index, targetIndex);
    }

    public SteeringOutput CalculateSteering(SteeringAgent agent)
    {
        var result = new SteeringOutput { MaxAcceleration = maxAcceleration.CurrentValue };
        foreach (var behavior in behaviors)
        {
            if (!behavior.IsActive)
                continue;

            result[behavior.Type] = behavior.Weight.CurrentValue * behavior.Steer(agent);

            if (result.Acceleration.magnitude > maxAcceleration.CurrentValue)
                break;
        }
        return result;
    }
}