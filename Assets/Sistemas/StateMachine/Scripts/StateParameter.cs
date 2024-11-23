using System;
using UnityEngine;

[Serializable]
public class StateParameter
{
    [SerializeField] float currentValue;
    [SerializeField] float minimumValue;
    [SerializeField] float maximumValue;
    [SerializeField, HideInInspector] string name;

    public float CurrentValue
    {
        get => currentValue;
        set => currentValue = Mathf.Clamp(value, minimumValue, maximumValue);
    }

    public float MinimumValue => minimumValue;
    public float MaximumValue => maximumValue;
    public string Name => name;

    public StateParameter(string name, float minimumValue, float maximumValue, float? defaultValue = null)
    {
        this.name = name;
        this.minimumValue = minimumValue;
        this.maximumValue = maximumValue;

        currentValue = defaultValue ?? (minimumValue + maximumValue) / 2;
    }
}