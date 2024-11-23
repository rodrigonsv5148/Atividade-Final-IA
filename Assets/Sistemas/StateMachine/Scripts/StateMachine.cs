using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine
    {
        [SerializeReference] List<State> states;
        [SerializeReference] List<Transition> transitions;
        [SerializeReference] State currentState;

        public IEnumerable<State> States => states;
        public IEnumerable<Transition> Transitions => transitions;
        public State CurrentState => currentState;

        public bool TryRemoveState(State state)
        {
            Debug.Assert(state is not null);
            Debug.Assert(states.Contains(state));

            if (states[0] == state)
                return false;

            states.Remove(state);
            currentState = state == currentState ? states[0] : currentState;

            for (int i = transitions.Count - 1; i >= 0; i--)
            {
                if (transitions[i].Source == state || transitions[i].Target == state)
                    transitions.RemoveAt(i);
            }

            return true;
        }

        public void AddState(State state)
        {
            Debug.Assert(state is not null);
            Debug.Assert(!states.Contains(state));

            states.Add(state);
        }

        public void AddTransition(Transition transition)
        {
            Debug.Assert(transition.Source is not null);
            Debug.Assert(transition.Target is not null);
            Debug.Assert(!transitions.Contains(transition));

            transitions.Add(transition);
        }

        public void RemoveTransition(Transition transition)
        {
            Debug.Assert(transition is not null);

            transitions.Remove(transition);
        }

        public void ProcessInput(Input input)
        {
            foreach (var t in transitions.Where(t => t.Source == currentState))
            {
                if (!t.ShouldTransition(input))
                    continue;

                var nextState = t.Target;
                Debug.Log($"transitioning from {currentState.Name} to {nextState.Name}");
                currentState = nextState;
                break;
            }
        }

        public StateMachine()
        {
            State defaultState = StateVariant.Idle.New();
            states = new List<State> { defaultState };
            transitions = new List<Transition>();
            currentState = defaultState;
        }
    }
}