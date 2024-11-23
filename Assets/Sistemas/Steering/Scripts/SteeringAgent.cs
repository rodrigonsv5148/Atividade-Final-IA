using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteeringAgent : MonoBehaviour, IObstacle
{
    [SerializeReference] SteeringStrategy strategy;
    [SerializeField] Transform target;

    Rigidbody2D physics;
    CircleCollider2D myCollider;

    public float Radius => myCollider.radius * transform.lossyScale.x;
    public Vector2 Position => transform.position;
    public Vector2 TargetPosition => target.position;
    public Vector2 Velocity => physics.velocity;
    public StateParameter MaxAcceleration => strategy.MaxAcceleration;
    public StateParameter MaxSpeed => strategy.MaxSpeed;
    public Transform Target { get => target; set => target = value; }
    public SteeringOutput SteeringOutput { get; private set; }
    public SteeringStrategy Strategy => strategy;

    [ContextMenu("AddDefaultStrategy")]
    public void AddDefaultStrategy()
    {
        strategy = new SteeringStrategy()
        {
            Behaviors = new List<SteeringBehavior>
            {
                new Avoid() { IsActive = false },
                new Arrive() { IsActive = false },
                new Pursue() { IsActive = false },
                new Seek() { IsActive = false },
                new Flee() { IsActive = false }
            }
        };
    }

    public void FixedUpdate()
    {
        SteeringOutput = strategy.CalculateSteering(this);

        physics.velocity += SteeringOutput.AccelerationResult * Time.fixedDeltaTime;
        transform.eulerAngles = Velocity.magnitude > 0.1f ?
            new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(-Velocity.x, Velocity.y)) :
            transform.eulerAngles;
    }

    void Awake()
    {
        physics = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        strategy = strategy ?? new SteeringStrategy()
        {
            Behaviors = new List<SteeringBehavior>
            {
                new Avoid(),
                new Arrive(),
                new Seek(),
                new Pursue() { IsActive = false },
                new Flee() { IsActive = false }
            }
        };
    }
}
