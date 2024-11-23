using System.Linq;
using TMPro;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    [SerializeField] Transform[] targets;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] SteeringAgent playerAgent;

    void Start()
    {
        dropdown.AddOptions(targets.Select(t => t.name).ToList());
        dropdown.onValueChanged.AddListener(value =>
        {
            playerAgent.Target = targets[value];
        });
    }
}