using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacedBlock : BlockBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static event Action<PlacedBlock> OnMoved = delegate { };

    public Vector3 TargetPosition { get; private set; }

    [SerializeField] Color regularColor = Color.white;
    [SerializeField] Color blockedColor = Color.red;
    [SerializeField, HideInInspector] MeshRenderer mesh;
    [SerializeField, HideInInspector] Collider[] colliders;

    HashSet<(object emitter, Collider collider)> overlapping = new HashSet<(object, Collider)>();
    Status state = Status.OnGrid;
    MaterialPropertyBlock block;
    int colorID;

    bool IsBlocked => overlapping.Count > 0;
    bool IsInsideGridArea => TargetPosition.x >= 0f && TargetPosition.x <= 9f && TargetPosition.z >= 0f && TargetPosition.z <= 9f;

    void Awake()
    {
        colorID = Shader.PropertyToID("_Color");
        block = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (state == Status.OnGrid)
            return;

        var lerp = 13 * Time.deltaTime;
        var position = Vector3.Lerp(transform.position, TargetPosition, lerp);
        transform.position = position;
    }

    public override void RegisterTriggerEnter(object emitter, Collider other)
    {
        var overlap = other.TryGetComponent<BlockTrigger>(out _);
        overlap |= other.TryGetComponent<GridWall>(out _);

        if (overlap)
            _ = overlapping.Add((emitter, other));

        UpdateColor();
    }

    public override void RegisterTriggerExit(object emitter, Collider other)
    {
        var overlap = other.TryGetComponent<BlockTrigger>(out _);
        overlap |= other.TryGetComponent<GridWall>(out _);

        if (overlap)
            _ = overlapping.Remove((emitter, other));

        UpdateColor();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Assert(state != Status.BeingDragged);

        OnMoved(this);

        state = Status.BeingDragged;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Assert(state == Status.BeingDragged);

        TargetPosition = new Vector3
        {
            x = MathF.Round(eventData.pointerCurrentRaycast.worldPosition.x),
            y = transform.position.y,
            z = MathF.Round(eventData.pointerCurrentRaycast.worldPosition.z)
        };

        UpdateColor();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Assert(state == Status.BeingDragged);

        if (IsBlocked || !IsInsideGridArea)
        {
            foreach (var overlap in overlapping)
            {
                if (!overlap.collider.TryGetComponent<BlockTrigger>(out var otherEmitter))
                    continue;

                foreach (var collider in colliders)
                    otherEmitter.OnTriggerExit(collider);
            }

            Destroy(gameObject);
            return;
        }

        state = Status.OnGrid;
        transform.position = TargetPosition;
    }

    void OnValidate()
    {
        mesh = GetComponent<MeshRenderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public void UpdateColor()
    {
        block.SetColor(colorID, IsBlocked || !IsInsideGridArea ? blockedColor : regularColor);
        mesh.SetPropertyBlock(block);
    }

    public enum Status { BeingDragged, OnGrid }
}
