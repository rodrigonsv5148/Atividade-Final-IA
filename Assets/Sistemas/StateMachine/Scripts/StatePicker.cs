using System;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatePicker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static event Action<StatePicker> OnDropped = delegate { };

    [SerializeField] StateVariant variant;
    [SerializeField] TMP_Text nameDisplay;

    bool isBeingDragged;
    Vector3 targetPosition;

    public StateVariant Variant => variant;
    public Vector2 TargetPosition => targetPosition;
    public Vector3 StartPosition { get; set; }

    void Update()
    {
        if (!isBeingDragged)
            return;

        var position = Vector3.Lerp(transform.position, targetPosition, 13 * Time.deltaTime);
        transform.position = position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeingDragged = true;
        eventData.Use();
    }

    public void OnDrag(PointerEventData eventData)
    {
        targetPosition = new Vector3
        {
            x = eventData.pointerCurrentRaycast.worldPosition.x,
            y = eventData.pointerCurrentRaycast.worldPosition.y,
            z = transform.position.z
        };
        eventData.Use();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isBeingDragged = false;
        OnDropped(this);
        transform.GetComponent<RectTransform>().localPosition = StartPosition;
        eventData.Use();
    }

    void OnValidate()
    {
        nameDisplay.text = variant.GetName();
    }
}