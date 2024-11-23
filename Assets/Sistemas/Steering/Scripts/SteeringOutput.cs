using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public struct SteeringOutput
{
    public Vector2 Seek;
    public Vector2 Arrive;
    public Vector2 Avoid;
    public Vector2 Evade;
    public Vector2 Flee;
    public Vector2 Pursue;
    public float MaxAcceleration;

    public Vector2 Acceleration => Seek + Arrive + Avoid + Evade + Flee + Pursue;
    public Vector2 AccelerationResult => Vector2.ClampMagnitude(Acceleration, MaxAcceleration);

    public static SteeringOutput operator *(SteeringOutput lhs, float rhs)
    {
        var result = new SteeringOutput();

        for (Behavior b = Behavior.Seek; b <= Behavior.Flee; b++)
            result[b] = lhs[b] * rhs;

        return result;
    }

    public static SteeringOutput operator +(SteeringOutput lhs, SteeringOutput rhs)
    {
        var result = new SteeringOutput();

        for (Behavior b = Behavior.Seek; b <= Behavior.Flee; b++)
            result[b] = lhs[b] + rhs[b];

        return result;
    }

    public static SteeringOutput operator /(SteeringOutput lhs, float rhs)
    {
        var result = new SteeringOutput();

        for (Behavior b = Behavior.Seek; b <= Behavior.Flee; b++)
            result[b] = lhs[b] / rhs;

        return result;
    }

    public Vector2 this[Behavior index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return index switch
            {
                Behavior.Seek => Seek,
                Behavior.Arrive => Arrive,
                Behavior.Avoid => Avoid,
                Behavior.Evade => Evade,
                Behavior.Pursue => Pursue,
                Behavior.Flee => Flee,
                _ => throw new IndexOutOfRangeException("invalid Behavior index"),
            };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            switch (index)
            {
                case Behavior.Seek:
                    Seek = value;
                    break;
                case Behavior.Arrive:
                    Arrive = value;
                    break;
                case Behavior.Avoid:
                    Avoid = value;
                    break;
                case Behavior.Evade:
                    Evade = value;
                    break;
                case Behavior.Pursue:
                    Pursue = value;
                    break;
                case Behavior.Flee:
                    Flee = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("invalid Behavior index");
            }
        }
    }
}