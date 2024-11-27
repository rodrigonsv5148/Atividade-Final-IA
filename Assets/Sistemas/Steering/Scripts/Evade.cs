using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Evade : SteeringBehavior
{
    [SerializeField] StateParameter radius = new StateParameter("Radius", 10f, 200f, 25f); // Distância de ativação
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f);  // Antecipação do movimento do alvo

    public StateParameter Radius => radius;
    public StateParameter Future => future;

    public override Vector2 Steer(SteeringAgent agent)
    {
        // Calcula a diferença entre a posição do agente e a posição futura do alvo
        var delta = agent.Position - agent.TargetPosition;

        // Se o alvo for um obstáculo (com velocidade), adiciona a posição futura antecipada
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta -= target.Velocity * future.CurrentValue;

        // Verifica se está dentro do raio de influência para fugir
        var desiredVelocity = delta.magnitude < radius.CurrentValue
            ? delta.normalized * agent.MaxSpeed.CurrentValue  // Foge se dentro do raio
            : agent.Velocity;                                // Mantém a velocidade atual caso contrário

        // Calcula a aceleração desejada
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
        yield return Radius;
        yield return Future;
    }

    public Evade() : base("Evade", Behavior.Evade)
    {
    }
    /*
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f);
    [SerializeField] StateParameter radius = new StateParameter("Radius", 10f, 200f, 25f);

    public StateParameter Radius => radius;
    public StateParameter Future => future;

    public override Vector2 Steer(SteeringAgent agent)
    {
        var delta = agent.Position - agent.TargetPosition;
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta += target.Velocity * future.CurrentValue;
        Debug.DrawLine(agent.Position, agent.Position + delta, Color.white);
        var desiredVelocity = delta.magnitude < radius.CurrentValue ? delta.normalized * agent.MaxSpeed.CurrentValue : agent.Velocity;
        //var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
    }

    public override IEnumerable<StateParameter> GetParameters()
    {
        yield return Weight;
        yield return Future;
        yield return radius;
    }

    public Evade() : base("Evade", Behavior.Evade)
    {
    }*/
}
