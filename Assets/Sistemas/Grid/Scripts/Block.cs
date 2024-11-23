using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : BlockBase, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static event Action<Block> OnDropped = delegate { };

    public Shape Profile => profile;
    public Rotation LookRotation { get; set; }
    public Vector3 TargetPosition { get; private set; }
    public Vector3 StartPosition { get; private set; }

    [SerializeField] Color regularColor = Color.white;
    [SerializeField] Color blockedColor = Color.red;
    [SerializeField] Shape profile;
    [SerializeField, HideInInspector] MeshRenderer mesh;

    HashSet<(object, Collider)> overlapping = new HashSet<(object, Collider)>();
    Status state = Status.OnMenu;
    MaterialPropertyBlock block;
    int colorID;

    bool IsBlocked => overlapping.Count > 0;
    bool IsInsideGridArea => TargetPosition.x >= 0f && TargetPosition.x <= 9f && TargetPosition.z >= 0f && TargetPosition.z <= 9f;

    void Awake()
    {
        colorID = Shader.PropertyToID("_Color");
        block = new MaterialPropertyBlock();
    }

    void Start()
    {
        StartPosition = TargetPosition = transform.position;
    }

    void Update()
    {
        var lerp = 13 * Time.deltaTime;
        var position = state switch
        {
            Status.BeingDragged => Vector3.Lerp(transform.position, TargetPosition, lerp),
            Status.OnMenu => Vector3.Lerp(transform.position, StartPosition, lerp),
            _ => Vector3.zero
        };

        transform.position = position;
        transform.eulerAngles = LookRotation.EulerAngle();
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

        state = Status.BeingDragged;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Assert(state == Status.BeingDragged);

        TargetPosition = new Vector3
        {
            x = MathF.Round(eventData.pointerCurrentRaycast.worldPosition.x),
            y = StartPosition.y + 0.25f,
            z = MathF.Round(eventData.pointerCurrentRaycast.worldPosition.z)
        };
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Assert(state == Status.BeingDragged);

        state = (IsBlocked, IsInsideGridArea) switch
        {
            (true, _) => Status.OnMenu,
            (false, false) => Status.OnMenu,
            (false, true) => Status.OnGrid
        };

        if (state == Status.OnGrid)
        {
            OnDropped(this);
            Destroy(gameObject);
        }
    }

    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (state != Status.OnMenu)
            return;

        LookRotation = LookRotation.Rotate(profile);
    }

    void OnValidate()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    public void UpdateColor()
    {
        block.SetColor(colorID, IsBlocked ? blockedColor : regularColor);
        mesh.SetPropertyBlock(block);
    }

    public enum Status { OnMenu, BeingDragged, OnGrid }
    public enum Rotation { Default, Left, Down, Right }
    public enum Shape { Single, L, I, Quad, Corner }
}
