using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Evade : SteeringBehavior
{
    [SerializeField] StateParameter radius = new StateParameter("Radius", 10f, 200f, 25f); // Raio de ativa��o
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f);  // Antecipa��o do movimento do alvo

    public StateParameter Radius => radius;
    public StateParameter Future => future;

    public override Vector2 Steer(SteeringAgent agent)
    {
        // Calcula a diferen�a entre a posi��o do agente e a posi��o futura do alvo
        var delta = agent.Position - agent.TargetPosition;

        // Se o alvo for um obst�culo (com velocidade), calcula a posi��o futura antecipada
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta -= target.Velocity * future.CurrentValue;

        // Verifica se o alvo est� dentro do raio de influ�ncia
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

        // Fora do raio, n�o aplica acelera��o
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
    [SerializeField] StateParameter radius = new StateParameter("Radius", 10f, 200f, 25f); // Dist�ncia de ativa��o
    [SerializeField] StateParameter future = new StateParameter("Future", 0f, 100f, 10f);  // Antecipa��o do movimento do alvo

    public StateParameter Radius => radius;
    public StateParameter Future => future;

    public override Vector2 Steer(SteeringAgent agent)
    {
        // Calcula a diferen�a entre a posi��o do agente e a posi��o futura do alvo
        var delta = agent.Position - agent.TargetPosition;

        // Se o alvo for um obst�culo (com velocidade), adiciona a posi��o futura antecipada
        if (agent.Target.gameObject.TryGetComponent<IObstacle>(out var target))
            delta += target.Velocity * future.CurrentValue;

        // Verifica se est� dentro do raio de influ�ncia para fugir
        var desiredVelocity = delta.magnitude < radius.CurrentValue * future.CurrentValue
            ? delta.normalized * agent.MaxSpeed.CurrentValue  // Foge se dentro do raio
            : agent.Velocity;                                // Mant�m a velocidade atual caso contr�rio
        //var desiredVelocity = delta.normalized * agent.MaxSpeed.CurrentValue;
        // Calcula a acelera��o desejada
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