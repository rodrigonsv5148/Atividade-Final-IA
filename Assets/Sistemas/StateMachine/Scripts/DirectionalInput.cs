using UnityEngine;
using UnityEngine.EventSystems;

public class DirectionalInput : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2Int Latest { get; private set; }

    [SerializeField] float deadZone = 0.125f;
    [SerializeField] float dragGain = 1f;
    [SerializeField] float maximumOffset = 1f;
    [SerializeField, Range(0.1f, 0.9f)] float snapToOriginFactor = 0.5f;
    [SerializeField] RectTransform handle;

    bool isDragging = false;

    void Update()
    {
        if (!isDragging)
        {
            handle.anchoredPosition *= snapToOriginFactor;
        }

        var raw = handle.anchoredPosition;
        Latest = new Vector2Int
        (
            x: Mathf.Abs(raw.x) > deadZone ? (int)Mathf.Sign(raw.x) : 0,
            y: Mathf.Abs(raw.y) > deadZone ? (int)Mathf.Sign(raw.y) : 0
        );
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        eventData.Use();
    }

    public void OnDrag(PointerEventData eventData)
    {
        handle.Translate(dragGain * Time.deltaTime * eventData.delta);
        handle.anchoredPosition = Vector2.ClampMagnitude(handle.anchoredPosition, maximumOffset);
        eventData.Use();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        eventData.Use();
    }
}
