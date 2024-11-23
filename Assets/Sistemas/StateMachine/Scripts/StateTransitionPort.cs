using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine
{
    public class StateTransitionPort : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] PortType type;
        [SerializeField] TransitionDisplay transitionPrefab;
        State state;
        StateMachine machine;
        StateMachineController controller;

        public TransitionDisplay TransitionDisplay { get; set; }
        public PortType Type => type;
        public PortStatus Status { get; set; } = PortStatus.Empty;
        public State State => state;

        public void Setup(StateMachineController controller, StateMachine machine, State state)
        {
            this.controller = controller;
            this.machine = machine;
            this.state = state;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Status == PortStatus.Occupied)
                return;

            Debug.Assert(TransitionDisplay == null);

            TransitionDisplay = Instantiate(transitionPrefab);
            TransitionDisplay.FromPort = this;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Status == PortStatus.Occupied)
                return;

            Debug.Assert(TransitionDisplay != null);

            TransitionDisplay.EndPosition = new Vector3
            {
                x = eventData.pointerCurrentRaycast.worldPosition.x,
                y = eventData.pointerCurrentRaycast.worldPosition.y,
                z = transform.position.z
            };
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (Status == PortStatus.Occupied)
                return;

            Debug.Assert(TransitionDisplay != null);

            if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<StateTransitionPort>(out var targetPort))
            {
                var incorrectTargetType = type == targetPort.Type;
                var isSelf = transform.parent.gameObject == targetPort.transform.parent.gameObject;
                var isOccupied = targetPort.Status == PortStatus.Occupied;
                var isDuplicated = machine.Transitions.Any(t => t.Source == state && t.Target == targetPort.State);

                if (incorrectTargetType || isSelf || isOccupied || isDuplicated)
                    goto InvalidPortTarget;

                var transition = new Transition(state, targetPort.State);
                controller.AddTransition(transition, TransitionDisplay, this, targetPort);
                return;
            }

        InvalidPortTarget:
            Destroy(TransitionDisplay.gameObject);
            TransitionDisplay = null;
        }

        public enum PortType { Entry, Exit };
        public enum PortStatus { Empty, Occupied }
    }
}