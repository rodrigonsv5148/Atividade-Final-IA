using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Evade : SteeringBehavior
{
    [SerializeField] StateParameter radius = new StateParameter("Radius", 10f, 200f, 25f); // Raio de ativação
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f);  // Antecipação do movimento do alvo

    public StateParameter Radius => radius;
    public StateParameter Future => future;

    public override Vector2 Steer(SteeringAgent agent)
    {
        // Calcula a diferença entre a posição do agente e a posição futura do alvo
        var delta = agent.Position - agent.TargetPosition;

        // Se o alvo for um obstáculo (com velocidade), calcula a posição futura antecipada
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta -= target.Velocity * future.CurrentValue;

        // Verifica se o alvo está dentro do raio de influência
        if (delta.magnitude < radius.CurrentValue * future.CurrentValue)
        {
            // Foge do alvo
            var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
            var desiredAcceleration = desiredVelocity - agent.Velocity;

            // Visualiza o vetor de fuga com linha (opcional)
            if (GameManager.line && agent.Strategy.Behaviors[3].IsActive)
            {
                agent.lre.enabled = true;
                agent.lre.SetPosition(0, agent.Position);
                agent.lre.SetPosition(1, agent.Position + delta);
            }
            else
            {
                agent.lre.enabled = false;
            }

            return desiredAcceleration * agent.MaxAcceleration.CurrentValue / agent.MaxSpeed.CurrentValue;
        }

        // Fora do raio, não aplica aceleração
        agent.lre.enabled = false;
        return Vector2.zero;
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
}


/*using System;
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
            delta += target.Velocity * future.CurrentValue;

        // Verifica se está dentro do raio de influência para fugir
        var desiredVelocity = delta.magnitude < radius.CurrentValue * future.CurrentValue
            ? delta.normalized * agent.MaxSpeed.CurrentValue  // Foge se dentro do raio
            : agent.Velocity;                                // Mantém a velocidade atual caso contrário
        //var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        // Calcula a aceleração desejada
        var desiredAcceleration = desiredVelocity - agent.Velocity;
        if (GameManager.line && agent.Strategy.Behaviors[3].IsActive == true)
        {
            agent.lre.enabled = true;
            agent.lre.SetPosition(0, agent.Position);
            agent.lre.SetPosition(1, agent.Position + delta);
        }
        else
        {
            agent.lre.enabled = false;
        }
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
}
*/