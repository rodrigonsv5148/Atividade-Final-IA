using UnityEngine;
using UnityEngine.EventSystems;

public class BackgroundTargetPosition : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] SteeringAgent agent;
    [SerializeField] Transform target;

    public void OnPointerDown(PointerEventData eventData)
    {
        target.position = eventData.pointerPressRaycast.worldPosition;
        agent.Target = target;
    }
}
