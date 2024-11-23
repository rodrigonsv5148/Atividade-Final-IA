using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public enum StateVariant { Idle, Run, Jump, Crouch, Kick, Throw };

    [Serializable]
    public class RunState : State
    {
        [SerializeField]
        StateParameter speed = new StateParameter(RUN_SPEED_PARAMETER_NAME, 0.1f, 2f);

        public override void Enact(Character character, Input input)
        {
            character.Animator.Play("run");
            character.Animator.speed = speed.CurrentValue;

            if (input.Horizontal != 0)
                character.Image.transform.eulerAngles = input.Horizontal == 1 ?
                    new Vector3(0, 180, 0) :
                    new Vector3(0, 0, 0);
        }

        public override IEnumerable<StateParameter> GetParameters()
        {
            yield return speed;
        }

        public RunState() : base(RUN_STATE_NAME) { }

        public const string RUN_STATE_NAME = "Run";
        public const string RUN_SPEED_PARAMETER_NAME = "Speed";
    }

    [Serializable]
    public class JumpState : State
    {
        [SerializeField]
        StateParameter impulse = new StateParameter(JUMP_IMPULSE_PARAMETER_NAME, 0f, 5f);

        public override void Enact(Character character, Input input)
        {
            character.Animator.Play("jump");
        }

        public override IEnumerable<StateParameter> GetParameters()
        {
            yield return impulse;
        }

        public JumpState() : base(JUMP_STATE_NAME) { }

        public const string JUMP_STATE_NAME = "Jump";
        public const string JUMP_IMPULSE_PARAMETER_NAME = "Strength";
    }

    [Serializable]
    public class CrouchState : State
    {
        public override void Enact(Character character, Input input)
        {
            character.Animator.Play("crouch");
        }

        public override IEnumerable<StateParameter> GetParameters()
        {
            yield break;
        }

        public CrouchState() : base(CROUCH_STATE_NAME) { }

        public const string CROUCH_STATE_NAME = "Crouch";
    }

    [Serializable]
    public class KickState : State
    {
        public override void Enact(Character character, Input input)
        {
            character.Animator.Play("kick");
        }

        public override IEnumerable<StateParameter> GetParameters()
        {
            yield break;
        }

        public KickState() : base(KICK_STATE_NAME) { }

        public const string KICK_STATE_NAME = "Kick";
    }

    [Serializable]
    public class ThrowState : State
    {
        public override void Enact(Character character, Input input)
        {
            character.Animator.Play("throw");
        }

        public override IEnumerable<StateParameter> GetParameters()
        {
            yield break;
        }

        public ThrowState() : base(THROW_STATE_NAME) { }

        public const string THROW_STATE_NAME = "Throw";
    }

    [Serializable]
    public class IdleState : State
    {
        public override void Enact(Character character, Input _)
        {
            character.Animator.Play("idle");
        }

        public override IEnumerable<StateParameter> GetParameters()
        {
            yield break;
        }

        public IdleState() : base(IDLE_STATE_NAME) { }

        public const string IDLE_STATE_NAME = "Idle";
    }

    [Serializable]
    public abstract class State
    {
        [SerializeField] string name;
        [SerializeField] Vector2 position;

        public string Name => name;

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public abstract void Enact(Character character, Input input);

        public abstract IEnumerable<StateParameter> GetParameters();

        protected State(string name)
        {
            this.name = name;
        }
    }

    public static class StateExtensions
    {
        public static State New(this StateVariant variant)
        {
            return variant switch
            {
                StateVariant.Idle => new IdleState(),
                StateVariant.Run => new RunState(),
                StateVariant.Jump => new JumpState(),
                StateVariant.Crouch => new CrouchState(),
                StateVariant.Kick => new KickState(),
                StateVariant.Throw => new ThrowState(),
                _ => null,
            };
        }

        public static string GetName(this StateVariant variant)
        {
            return variant switch
            {
                StateVariant.Idle => IdleState.IDLE_STATE_NAME,
                StateVariant.Run => RunState.RUN_STATE_NAME,
                StateVariant.Jump => JumpState.JUMP_STATE_NAME,
                StateVariant.Crouch => CrouchState.CROUCH_STATE_NAME,
                StateVariant.Kick => KickState.KICK_STATE_NAME,
                StateVariant.Throw => ThrowState.THROW_STATE_NAME,
                _ => string.Empty,
            };
        }
    }
}