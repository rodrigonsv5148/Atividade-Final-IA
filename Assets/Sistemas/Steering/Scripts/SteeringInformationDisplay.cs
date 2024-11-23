using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteeringInformationDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text speedDisplay;
    [SerializeField] TMP_Text accelerationDisplay;
    [SerializeField] SteeringAgent agent;
    [SerializeField] Image[] accelerationBars;
    [SerializeField, Range(0.001f, 1f)] float filterWeight = 0.2f;

    float maximumHeight;

    void Start()
    {
        maximumHeight = accelerationBars[0].rectTransform.rect.height;
    }

    void Update()
    {
        var (velocity, steering) = ComputeFilteredOutputs();

        speedDisplay.text = velocity.magnitude.ToString("F2");
        accelerationDisplay.text = steering.AccelerationResult.magnitude.ToString("F2");

        AdjustBar(accelerationBars[0], velocity, agent.MaxSpeed.CurrentValue);

        for (var index = Behavior.Seek; index <= Behavior.Flee; index++)
        {
            AdjustBar(accelerationBars[(int)index], steering[index], agent.MaxAcceleration.CurrentValue);
        }

    }

    SteeringOutput previousOutput;
    Vector2 previousVelocity;
    (Vector2, SteeringOutput) ComputeFilteredOutputs()
    {
        previousOutput = agent.SteeringOutput * filterWeight + previousOutput * (1.0f - filterWeight);
        previousVelocity = agent.Velocity * filterWeight + previousVelocity * (1.0f - filterWeight);

        return (previousVelocity, previousOutput);
    }

    void AdjustBar(Image bar, Vector2 vector, float maximumMagnitude)
    {
        bar.rectTransform.sizeDelta = new Vector2
        {
            x = bar.rectTransform.sizeDelta.x,
            y = vector.magnitude / maximumMagnitude * maximumHeight
        };
        bar.rectTransform.eulerAngles = new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(-vector.x, vector.y));
    }
}
