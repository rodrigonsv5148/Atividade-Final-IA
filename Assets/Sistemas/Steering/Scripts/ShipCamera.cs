using UnityEngine;

public class ShipCamera : MonoBehaviour
{
    [SerializeField] Transform shipTransform;
    [SerializeField] float minimumHeight = 100;
    [SerializeField] float maximumHeight = 600f;
    [SerializeField] float scrollGain = 1f;
    [SerializeField] float smoothing = 1f;

    void FixedUpdate()
    {
        var targetHeight = transform.position.z - scrollGain * Input.mouseScrollDelta.y;

        var targetPosition = new Vector3
        {
            x = shipTransform.position.x,
            y = shipTransform.position.y,
            z = Mathf.Clamp(targetHeight, -maximumHeight, -minimumHeight)
        };

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothing);
    }
}
