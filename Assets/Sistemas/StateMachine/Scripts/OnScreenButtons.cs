using UnityEngine;

public class OnScreenButtons : MonoBehaviour
{
    public bool X { get; private set; }
    public bool Y { get; private set; }
    public bool A { get; private set; }
    public bool B { get; private set; }

    void LateUpdate()
    {
        X = Y = A = B = false;
    }

    public void PressButton(int button)
    {
        switch (button)
        {
            case (int)Button.X: X = true; break;
            case (int)Button.Y: Y = true; break;
            case (int)Button.A: A = true; break;
            case (int)Button.B: B = true; break;
        }
    }

    public enum Button { X = 0, Y = 1, A = 2, B = 3 }
}