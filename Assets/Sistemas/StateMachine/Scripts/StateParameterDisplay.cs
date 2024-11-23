using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StateParameterDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text parameterNameDisplay;
    [SerializeField] Slider slider;

    public void Setup(StateParameter stateParameter)
    {
        gameObject.SetActive(true);
        parameterNameDisplay.gameObject.SetActive(true);

        parameterNameDisplay.text = stateParameter.Name;
        slider.minValue = stateParameter.MinimumValue;
        slider.maxValue = stateParameter.MaximumValue;
        slider.value = stateParameter.CurrentValue;

        slider.onValueChanged.AddListener(value =>
        {
            stateParameter.CurrentValue = value;
        });
    }
}