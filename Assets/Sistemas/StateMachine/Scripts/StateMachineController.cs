using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateMachineController : MonoBehaviour
    {
        [SerializeReference] StateMachine machine;
        [SerializeField] Character character;
        [SerializeField] Camera cam;
        [SerializeField] StateMachineDisplay stateDisplayPrefab;
        List<StateMachineDisplay> stateDisplays = new List<StateMachineDisplay>();
        List<TransitionDisplay> transitionDisplays = new List<TransitionDisplay>();

        public void AddState(State state)
        {
            machine.AddState(state);
            var display = Instantiate(stateDisplayPrefab, state.Position, Quaternion.identity, transform);
            display.Setup(this, machine, state, cam);
            stateDisplays.Add(display);
        }

        public void RemoveState(State state, StateMachineDisplay display)
        {
            var removed = machine.TryRemoveState(state);

            if (!removed)
                return;

            for (int i = transitionDisplays.Count - 1; i >= 0; i--)
            {
                if (transitionDisplays[i].Transition.Source == state || transitionDisplays[i].Transition.Target == state)
                    RemoveTransition(transitionDisplays[i]);
            }

            stateDisplays.Remove(display);
            Destroy(display.gameObject);
        }

        public void AddTransition(Transition transition, TransitionDisplay transitionDisplay, StateTransitionPort fromPort, StateTransitionPort toPort)
        {
            transitionDisplay.Transition = transition;
            transitionDisplay.FromPort = fromPort;
            transitionDisplay.ToPort = toPort;
            transitionDisplay.Dropdown.gameObject.SetActive(true);

            fromPort.Status = toPort.Status = StateTransitionPort.PortStatus.Occupied;

            machine.AddTransition(transition);

            transitionDisplays.Add(transitionDisplay);
        }

        public void RemoveTransition(TransitionDisplay transitionDisplay)
        {
            var transition = transitionDisplay.Transition;
            machine.RemoveTransition(transitionDisplay.Transition);
            transitionDisplay.ToPort.Status = transitionDisplay.FromPort.Status = StateTransitionPort.PortStatus.Empty;
            transitionDisplay.Transition = null;
            transitionDisplays.Remove(transitionDisplay);
            Destroy(transitionDisplay.gameObject);
        }

        void Start()
        {
            machine = new StateMachine();

            var display = Instantiate(stateDisplayPrefab, machine.CurrentState.Position, Quaternion.identity, transform);
            display.Setup(this, machine, machine.CurrentState, cam);
            stateDisplays.Add(display);
        }

        public void ProcessInput(Input input)
        {
            // input = new Input
            // {
            //     Horizontal = (int)UnityEngine.Input.GetAxisRaw("Horizontal"),
            //     Vertical = (int)UnityEngine.Input.GetAxisRaw("Vertical"),
            //     X = UnityEngine.Input.GetKey(KeyCode.Z),
            //     Y = UnityEngine.Input.GetKey(KeyCode.X),
            //     B = UnityEngine.Input.GetKey(KeyCode.C),
            //     A = UnityEngine.Input.GetKey(KeyCode.V),
            // };

            character.Animator.speed = 1f;
            machine.ProcessInput(input);
            machine.CurrentState.Enact(character, input);
        }
    }
}