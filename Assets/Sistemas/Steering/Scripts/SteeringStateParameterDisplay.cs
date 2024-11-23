using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteeringStateParameterDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text parameterNameDisplay;
    [SerializeField] Slider slider;

    public void Setup(StateParameter stateParameter)
    {
        gameObject.SetActive(true);
        parameterNameDisplay.gameObject.SetActive(true);

        parameterNameDisplay.text = $"{stateParameter.Name}: {stateParameter.CurrentValue:F2}";
        slider.minValue = stateParameter.MinimumValue;
        slider.maxValue = stateParameter.MaximumValue;
        slider.value = stateParameter.CurrentValue;

        slider.onValueChanged.AddListener(value =>
        {
            stateParameter.CurrentValue = value;
            parameterNameDisplay.text = $"{stateParameter.Name}: {value:F2}";
        });
    }
}