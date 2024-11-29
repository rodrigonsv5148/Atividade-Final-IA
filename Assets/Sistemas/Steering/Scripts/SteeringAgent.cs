using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteeringAgent : MonoBehaviour, IObstacle
{
    [SerializeReference] SteeringStrategy strategy;
    [SerializeField] Transform target;
    [SerializeField] List<GameObject> lrs;

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
    public bool isSteering = true;
    public LineRenderer lr => lrs[0].GetComponent<LineRenderer>();
    public LineRenderer lrp => lrs[1].GetComponent<LineRenderer>();
    public LineRenderer lrf => lrs[2].GetComponent<LineRenderer>();
    public LineRenderer lre => lrs[3].GetComponent<LineRenderer>();
    public LineRenderer lrar => lrs[4].GetComponent<LineRenderer>();
    public LineRenderer lrav => lrs[5].GetComponent<LineRenderer>();

    [ContextMenu("AddDefaultStrategy")]
    public void AddDefaultStrategy()
    {
        strategy = new SteeringStrategy()
        {
            Behaviors = new List<SteeringBehavior>
            {
                new Seek() { IsActive = false },
                new Pursue() { IsActive = false },
                new Flee() { IsActive = false },
                new Evade() { IsActive = false },
                new Avoid() { IsActive = false },
                new Arrive() { IsActive = false }
            }
        };
    }

    public void FixedUpdate()
    {
        if (isSteering) 
        {
            SteeringOutput = strategy.CalculateSteering(this);

            physics.velocity += SteeringOutput.AccelerationResult * Time.fixedDeltaTime;
            transform.eulerAngles = Velocity.magnitude > 0.1f ?
                new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(-Velocity.x, Velocity.y)) :
                transform.eulerAngles;
        }

        if (Strategy.Behaviors[0].IsActive == false) 
        {
            lr.enabled = false;
        }
        if (Strategy.Behaviors[1].IsActive == false)
        {
            lrp.enabled = false;
        }
        if (Strategy.Behaviors[2].IsActive == false)
        {
            lrf.enabled = false;
        }
        if (Strategy.Behaviors[3].IsActive == false)
        {
            lre.enabled = false;
        }
    }

    void Awake()
    {
        physics = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<CircleCollider2D>();
        strategy = strategy ?? new SteeringStrategy()
        {
            Behaviors = new List<SteeringBehavior>
            {
                new Seek(),
                new Pursue() { IsActive = false },
                new Flee() { IsActive = false },
                new Evade() { IsActive= false },
                new Avoid(){ IsActive = false },
                new Arrive(){ IsActive = false }
            }
        };
    }
}
