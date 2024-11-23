using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace StateMachine
{
    public class StateMachineDisplay : MonoBehaviour
    {
        [SerializeField] float searchRadius = 15f;
        [SerializeField] float separationRadius = 5f;
        [SerializeField] Canvas canvas;
        [SerializeField] TMP_Text stateNameDisplay;
        [SerializeField, HideInInspector] List<StateParameterDisplay> parameterDisplays;
        [SerializeField, HideInInspector] StateTransitionPort[] entryPorts;
        [SerializeField, HideInInspector] StateTransitionPort[] exitPorts;

        State state;
        StateMachine machine;
        StateMachineController controller;
        bool isBeingDragged;
        Vector3 targetPosition;
        [SerializeField] Collider2D[] overlap = new Collider2D[8];

        public State State => state;

        public void RemoveState()
        {
            controller.RemoveState(state, this);
        }

        void Update()
        {
            if (isBeingDragged)
            {
                var position = Vector3.Lerp(transform.position, targetPosition, 13 * Time.deltaTime);
                transform.position = position;
                state.Position = position;
            }
            else
            {
                int found = Physics2D.OverlapCircleNonAlloc
                (
                    point: transform.position,
                    radius: searchRadius,
                    results: overlap
                );

                var translation = Vector3.zero;
                foreach (var other in overlap[..found])
                {
                    if (other.gameObject == this.gameObject)
                        continue;

                    var distance = Vector3.Distance(transform.position, other.transform.position);
                    var factor = (distance - separationRadius) / separationRadius;
                    factor = factor > 0f ? factor : 0f;
                    translation += factor * (transform.position - other.transform.position) / (found - 1);
                }
                transform.position = Vector3.Lerp(transform.position, transform.position + translation, Time.deltaTime);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isBeingDragged = true;
            eventData.Use();
        }

        public void OnDrag(PointerEventData eventData)
        {
            targetPosition = new Vector3
            {
                x = eventData.pointerCurrentRaycast.worldPosition.x,
                y = eventData.pointerCurrentRaycast.worldPosition.y,
                z = transform.position.z
            };

            foreach (var port in entryPorts)
                port.transform.hasChanged = true;
            foreach (var port in exitPorts)
                port.transform.hasChanged = true;

            eventData.Use();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isBeingDragged = false;
            eventData.Use();
        }

        public void Setup(StateMachineController controller, StateMachine machine, State state, Camera cam)
        {
            this.controller = controller;
            this.machine = machine;
            this.state = state;

            canvas.worldCamera = cam;
            stateNameDisplay.text = state.Name.ToUpper();

            var index = 0;
            foreach (var parameter in state.GetParameters())
                parameterDisplays[index++].Setup(parameter);

            foreach (var port in entryPorts)
                port.Setup(controller, machine, state);
            foreach (var port in exitPorts)
                port.Setup(controller, machine, state);
        }

        void OnValidate()
        {
            parameterDisplays = new List<StateParameterDisplay>(GetComponentsInChildren<StateParameterDisplay>(true));
            entryPorts = GetComponentsInChildren<StateTransitionPort>(true).Where(p => p.Type == StateTransitionPort.PortType.Entry).ToArray();
            exitPorts = GetComponentsInChildren<StateTransitionPort>(true).Where(p => p.Type == StateTransitionPort.PortType.Exit).ToArray();
        }
    }
}