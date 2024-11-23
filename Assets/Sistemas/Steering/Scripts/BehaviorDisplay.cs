using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BehaviorDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text nameDisplay;
    [SerializeField] Toggle toggle;
    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField, HideInInspector] SteeringStateParameterDisplay[] parameterDisplays;
    [SerializeField, HideInInspector] RectTransform rect;
    float anchorYPosition;
    float perDisplayOffset;

    public void Setup(SteeringBehavior behavior, Action<PriorityChange> alterPriority, int index, float anchorYPosition, float perDisplayOffset)
    {
        this.anchorYPosition = anchorYPosition;
        this.perDisplayOffset = perDisplayOffset;
        nameDisplay.text = behavior.Name;

        UpdateOrder(index);

        toggle.isOn = behavior.IsActive;
        toggle.onValueChanged.AddListener(value =>
        {
            behavior.IsActive = value;
        });

        upButton.onClick.AddListener(() =>
        {
            alterPriority.Invoke(PriorityChange.Up);
        });
        downButton.onClick.AddListener(() =>
        {
            alterPriority.Invoke(PriorityChange.Down);
        });

        var parameterIndex = 0;
        foreach (var parameter in behavior.GetParameters())
        {
            parameterDisplays[parameterIndex++].Setup(parameter);
        }

        gameObject.SetActive(true);
    }

    public void UpdateOrder(int index)
    {
        rect.anchoredPosition3D = new Vector3
        (
            x: 0f,
            y: anchorYPosition - perDisplayOffset * index,
            z: index
        );
    }

    void OnValidate()
    {
        parameterDisplays = GetComponentsInChildren<SteeringStateParameterDisplay>(true);
        rect = transform as RectTransform;
    }
}
