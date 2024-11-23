using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEventsOnAnimation : MonoBehaviour
{
    public AnimationEvent[] animationEvent;

    public void CallEvent(int animationIndex)
    {
        animationEvent[animationIndex]?.Invoke();
    }
}

[System.Serializable]
public class AnimationEvent : UnityEvent
{

}