using System;
using UnityEngine;

namespace StateMachine
{
    [Serializable]
    public class Transition
    {
        [SerializeReference] State source;
        [SerializeReference] State target;
        [SerializeField] int mask;
        [SerializeField] int pattern;

        public State Source => source;
        public State Target => target;
        public InputPattern InputPattern
        {
            set { (mask, pattern) = value.GetMaskedPattern(); }
        }

        public bool ShouldTransition(Input input) => (input.RawCombined & mask) == pattern;

        public Transition(State source, State target, InputPattern inputPattern = InputPattern.Horizontal)
        {
            (mask, pattern) = inputPattern.GetMaskedPattern();
            this.source = source;
            this.target = target;
        }
    }
}