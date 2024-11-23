using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BehaviorConfigurator : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] SteeringAgent agent;
    [SerializeField] BehaviorDisplay behaviorDisplayPrefab;
    [SerializeField] TMP_Text maximumSpeedDisplay;
    [SerializeField] TMP_Text maximumAccelerationDisplay;
    [SerializeField] Slider maximumSpeedSlider;
    [SerializeField] Slider maximumAccelerationSlider;
    [SerializeField] RectTransform scollViewContentSize;
    [SerializeField] float verticalOffset;
    [SerializeField] float yAnchor;
    [SerializeField] BehaviorDisplay[] behaviorDisplays;

    public void ToggleVisibility()
    {
        canvas.SetActive(!canvas.activeInHierarchy);
        Time.timeScale = canvas.activeInHierarchy ? 0f : 1f;
    }

    void Start()
    {
        var index = 0;
        foreach (var behavior in agent.Strategy.Behaviors)
        {
            behaviorDisplays[index].Setup
            (
                behavior,
                (change) =>
                {
                    var (index, targetIndex) = agent.Strategy.AlterPriority(behavior, change);
                    if (index != targetIndex)
                    {
                        behaviorDisplays[index].UpdateOrder(targetIndex);
                        behaviorDisplays[targetIndex].UpdateOrder(index);
                        (behaviorDisplays[index], behaviorDisplays[targetIndex]) = (behaviorDisplays[targetIndex], behaviorDisplays[index]);
                    }
                },
                index,
                yAnchor,
                verticalOffset
            );
            index++;
        }
        ScrollViewResize();
        maximumSpeedDisplay.text = agent.MaxSpeed.CurrentValue.ToString("F2");
        maximumAccelerationDisplay.text = agent.MaxAcceleration.CurrentValue.ToString("F2");

        maximumSpeedSlider.maxValue = agent.MaxSpeed.MaximumValue;
        maximumSpeedSlider.minValue = agent.MaxSpeed.MinimumValue;
        maximumAccelerationSlider.maxValue = agent.MaxAcceleration.MaximumValue;
        maximumAccelerationSlider.minValue = agent.MaxAcceleration.MinimumValue;

        maximumSpeedSlider.value = agent.MaxSpeed.CurrentValue;
        maximumAccelerationSlider.value = agent.MaxAcceleration.CurrentValue;

        maximumSpeedSlider.onValueChanged.AddListener((value) =>
        {
            agent.MaxSpeed.CurrentValue = value;
            maximumSpeedDisplay.text = agent.MaxSpeed.CurrentValue.ToString("F2");
        });
        maximumAccelerationSlider.onValueChanged.AddListener((value) =>
        {
            agent.MaxAcceleration.CurrentValue = value;
            maximumAccelerationDisplay.text = agent.MaxAcceleration.CurrentValue.ToString("F2");
        });
    }

    void OnValidate()
    {
        behaviorDisplays = GetComponentsInChildren<BehaviorDisplay>(true);
    }

    public void ScrollViewResize()
    {
        int svIndex = 0;

        foreach (var behaviorOn in agent.Strategy.Behaviors)
        {
            svIndex++;
        }
        int scrollViewHeight = svIndex * 131;
        scollViewContentSize.sizeDelta = new Vector2(scollViewContentSize.sizeDelta.x, scrollViewHeight);
    }
}