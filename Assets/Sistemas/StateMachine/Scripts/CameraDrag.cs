using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour, IDragHandler
{
    [SerializeField] Camera cam;
    [SerializeField, Range(1f, 100f)] float factor = 1f;

    public void OnDrag(PointerEventData eventData)
    {
        cam.transform.Translate(-factor * Time.deltaTime * eventData.delta);
    }
}