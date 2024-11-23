using System;

namespace StateMachine
{
    public enum InputPattern { NoInput, Horizontal, Vertical, Up, Down, Left, Right, X, Y, A, B };

    public struct Input
    {
        int rawCombined;

        public int RawCombined => rawCombined;

        public int Horizontal
        {
            get => -((rawCombined & HORIZONTAL_BIT_MASK) >> HORIZONTAL_SHIFT) + 2;
            set => rawCombined = (rawCombined & ~HORIZONTAL_BIT_MASK) | ((-value + 2) << HORIZONTAL_SHIFT);
        }

        public int Vertical
        {
            get => -((rawCombined & VERTICAL_BIT_MASK) >> VERTICAL_SHIFT) + 2;
            set => rawCombined = (rawCombined & ~VERTICAL_BIT_MASK) | ((-value + 2) << VERTICAL_SHIFT);
        }

        public bool X
        {
            get => (rawCombined & X_BIT_MASK) != 0;
            set => rawCombined = (rawCombined & ~X_BIT_MASK) | Convert.ToInt32(value) << X_SHIFT;
        }

        public bool Y
        {
            get => (rawCombined & Y_BIT_MASK) != 0;
            set => rawCombined = (rawCombined & ~Y_BIT_MASK) | Convert.ToInt32(value) << Y_SHIFT;
        }

        public bool B
        {
            get => (rawCombined & B_BIT_MASK) != 0;
            set => rawCombined = (rawCombined & ~B_BIT_MASK) | Convert.ToInt32(value) << B_SHIFT;
        }

        public bool A
        {
            get => (rawCombined & A_BIT_MASK) != 0;
            set => rawCombined = (rawCombined & ~A_BIT_MASK) | Convert.ToInt32(value) << A_SHIFT;
        }

        Input(int raw) => rawCombined = raw;

        public override string ToString() => Convert.ToString(rawCombined, 2).PadLeft(8, '0');

        public static Input Zeroed => new Input(0b10100000);
        public static Input Up => new Input(0b10010000);
        public static Input Down => new Input(0b10110000);
        public static Input Left => new Input(0b11100000);
        public static Input Right => new Input(0b01100000);
        public static Input XInput => new Input(X_BIT_MASK);
        public static Input YInput => new Input(Y_BIT_MASK);
        public static Input AInput => new Input(A_BIT_MASK);
        public static Input BInput => new Input(B_BIT_MASK);

        public const int HORIZONTAL_BIT_MASK = 0b11000000;
        public const int VERTICAL_BIT_MASK = 0b00110000;
        public const int X_BIT_MASK = 0b00001000;
        public const int Y_BIT_MASK = 0b00000100;
        public const int B_BIT_MASK = 0b00000010;
        public const int A_BIT_MASK = 0b00000001;

        const int HORIZONTAL_SHIFT = 6;
        const int VERTICAL_SHIFT = 4;
        const int X_SHIFT = 3;
        const int Y_SHIFT = 2;
        const int B_SHIFT = 1;
        const int A_SHIFT = 0;

        public static readonly InputPattern[] Patterns = { InputPattern.NoInput, InputPattern.Horizontal, InputPattern.Vertical, InputPattern.Up, InputPattern.Down, InputPattern.Left, InputPattern.Right, InputPattern.X, InputPattern.Y, InputPattern.A, InputPattern.B };
    }

    public static class InputExtensions
    {
        public static (int mask, int pattern) GetMaskedPattern(this InputPattern inputPattern)
        {
            var (mask, pattern) = inputPattern switch
            {
                InputPattern.NoInput => (0b11111111, Input.Zeroed.RawCombined),
                InputPattern.Horizontal => (0b01000000, 0b01000000),
                InputPattern.Vertical => (0b00010000, 0b00010000),
                InputPattern.Up => (Input.VERTICAL_BIT_MASK, Input.Up.RawCombined),
                InputPattern.Down => (Input.VERTICAL_BIT_MASK, Input.Down.RawCombined),
                InputPattern.Left => (Input.HORIZONTAL_BIT_MASK, Input.Left.RawCombined),
                InputPattern.Right => (Input.HORIZONTAL_BIT_MASK, Input.Right.RawCombined),
                InputPattern.X => (Input.X_BIT_MASK, Input.XInput.RawCombined),
                InputPattern.Y => (Input.Y_BIT_MASK, Input.YInput.RawCombined),
                InputPattern.A => (Input.A_BIT_MASK, Input.AInput.RawCombined),
                InputPattern.B => (Input.B_BIT_MASK, Input.BInput.RawCombined),
                _ => (-1, 0),
            };
            pattern &= mask;
            return (mask, pattern);
        }
    }
}