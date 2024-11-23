using UnityEngine;

public interface IObstacle
{
    float Radius { get; }
    Vector2 Position { get; }
    Vector2 Velocity { get; }
}

[RequireComponent(typeof(CircleCollider2D))]
public class Obstacle : MonoBehaviour, IObstacle
{
    [SerializeField] Transform origin;
    [SerializeField] float rotation = 1f;
    [SerializeField, HideInInspector] CircleCollider2D myCollider;
    [SerializeField, HideInInspector] float distanceToOrigin;
    Vector2 previousPosition;

    public float Radius => myCollider.radius * transform.lossyScale.x;
    public Vector2 Position => transform.position;
    public Vector2 Velocity => ((Vector2)transform.position - previousPosition) / Time.fixedDeltaTime;

    void OnValidate()
    {
        myCollider = GetComponent<CircleCollider2D>();
        distanceToOrigin = Vector3.Distance(origin.position, transform.position);
    }

    void FixedUpdate()
    {
        previousPosition = transform.position;
        transform.RotateAround(origin.position, Vector3.back, rotation);

        var offset = origin.position - transform.position;
        transform.position = origin.position - offset.normalized * distanceToOrigin;
    }
}
