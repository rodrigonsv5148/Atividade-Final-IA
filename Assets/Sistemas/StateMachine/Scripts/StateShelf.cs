using UnityEngine;

namespace StateMachine
{
    public class StateShelf : MonoBehaviour
    {
        [SerializeField] StatePicker[] pickers;
        [SerializeField] Vector3 middlePosition;
        [SerializeField] float yOffset;
        [SerializeField] StateMachineController controller;

        private void HandleStateDropped(StatePicker picker)
        {
            var state = picker.Variant.New();
            state.Position = picker.TargetPosition;
            controller.AddState(state);
        }

        void Start()
        {
            int count = 0, sign = 1;
            foreach (var picker in pickers)
            {
                picker.transform.localPosition = middlePosition + (++count / 2) * sign * yOffset * Vector3.up;
                picker.StartPosition = picker.GetComponent<RectTransform>().localPosition;
                sign *= -1;
            }
        }

        void Awake()
        {
            StatePicker.OnDropped += HandleStateDropped;
        }

        void OnValidate()
        {
            pickers = GetComponentsInChildren<StatePicker>();
        }

        void OnDestroy()
        {
            StatePicker.OnDropped -= HandleStateDropped;
        }
    }
}