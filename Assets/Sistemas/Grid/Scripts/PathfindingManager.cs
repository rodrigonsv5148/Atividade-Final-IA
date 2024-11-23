using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject fromDisplay;
    [SerializeField] GameObject toDisplay;
    [SerializeField] Slider slider;
    [SerializeField] GameObject sliderFill;
    [SerializeField] Grid grid;
    [SerializeField] Transform fromMarker;
    [SerializeField] Transform toMarker;
    [SerializeField, Range(1f, 20f)] float cameraMoveSpeed = 1f;
    [SerializeField] Vector3 gridTargetOffset;
    [SerializeField] Vector3 UITargetOffset;
    [SerializeField] Status state = Status.Left;
    [SerializeField] GameObject arrowLeft, arrowRight;
    public float timeScale;
    Node to, from;
    List<Node[]> pathHistory;
    List<PathPoint<Node>[]> pointHistory;

    public void ToggleView()
    {
        grid.ClearGrid();
        from = to = null;
        UpdateDisplays();

        state = state switch
        {
            Status.Left => Status.Right,
            Status.Right => Status.Left,
            _ => Status.Left
        };
        SwitchButton();
    }

    void HandleNodeClicked(Node node)
    {
        if (state == Status.Left)
            return;

        switch (from, to)
        {
            case (null, null):
                from = node;
                break;
            case ({ }, null):
                if (from == node) break;
                to = node;
                (pathHistory, pointHistory) = AStar<Node>.FindPathHistory(from, to);
                slider.maxValue = pathHistory.Count - 1;
                slider.value = 0;
                break;
            case ({ }, { }):
                from = node;
                to = null;
                grid.ClearGrid();
                break;
        }

        UpdateDisplays();
    }

    void HandleBlockMoved(PlacedBlock block)
    {
        grid.ClearGrid();
        from = to = null;
        UpdateDisplays();
    }

    void UpdateDisplays()
    {
        fromDisplay.GetComponentInChildren<TMP_Text>().text = from != null ? $"From: {from.GridPosition}" : "From:";
        toDisplay.GetComponentInChildren<TMP_Text>().text = to != null ? $"To: {to.GridPosition}" : "To:";
        slider.interactable = from != null && to != null;
        bool check = slider.interactable;
        sliderFill.SetActive(check);

        fromMarker.gameObject.SetActive(from != null);
        fromMarker.transform.position = from != null ? from.transform.position + gridTargetOffset : Vector3.zero;

        toMarker.gameObject.SetActive(to != null);
        toMarker.transform.position = to != null ? to.transform.position + gridTargetOffset : Vector3.zero;
    }

    void SwitchButton()
    {
        switch (state)
        {
            case Status.Left:
                arrowLeft.SetActive(false);
                arrowRight.SetActive(true);
                break;
            case Status.Right:
                arrowLeft.SetActive(true);
                arrowRight.SetActive(false);
                break;
        }
        StartCoroutine("MoveInterfaceRoutine");

    }
    IEnumerator MoveInterfaceRoutine()
    {
        switch (state)
        {
            case Status.Left:
                yield return new WaitForSeconds(0);
                break;
            case Status.Right:
                yield return new WaitForSeconds(0.17f);
                break;
        }
        MoveInterface(fromDisplay);
        MoveInterface(toDisplay);
        MoveInterface(slider.gameObject);
    }
    void MoveInterface(GameObject uIObject)
    {
        float uIYPosition = uIObject.transform.position.y;
        float uIZPosition = uIObject.transform.position.z;
        Vector3 leftUIPosition = new Vector3(Screen.width - (Screen.width / 5), uIYPosition, uIZPosition);
        Vector3 rightUIPosition = new Vector3(Screen.width + (Screen.width / 5), uIYPosition, uIZPosition);

        switch (state)
        {
            case Status.Left:
                uIObject.GetComponent<RectTransform>().position = Vector3.Lerp(leftUIPosition, rightUIPosition, 1);
                break;
            case Status.Right:
                uIObject.GetComponent<RectTransform>().position = Vector3.Lerp(rightUIPosition, leftUIPosition, 1);
                break;
        }

    }
    void Start()
    {
        SwitchButton();
        Node.OnClicked += HandleNodeClicked;
        PlacedBlock.OnMoved += HandleBlockMoved;

        fromMarker.gameObject.SetActive(false);
        toMarker.gameObject.SetActive(false);
        sliderFill.SetActive(false);
        slider.interactable = false;
        slider.onValueChanged.AddListener((value) =>
        {
            grid.UpdateGrid(pathHistory[(int)value], pointHistory[(int)value]);
        });
    }

    void Update()
    {
        var cameraTargetPosition = state switch
        {
            Status.Left => new Vector3(0f, 50f, 4.5f),
            Status.Right => new Vector3(9f, 50f, 4.5f),
            _ => Vector3.zero
        };
        cam.transform.position = Vector3.Lerp(cam.transform.position, cameraTargetPosition, Time.deltaTime * cameraMoveSpeed);
    }

    void OnDestroy()
    {
        Node.OnClicked -= HandleNodeClicked;
        PlacedBlock.OnMoved -= HandleBlockMoved;
    }

    public enum Status { Left, Right }
}
