using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleButton : MonoBehaviour
{
    public void SendMessage(bool toggleValue) 
    {
        if (toggleValue) 
        {
            GameManager.line = true;
        }
        else 
        {
            GameManager.line = false;
        }
    }

    public void SendMessage2(bool toggleValue)
    {
        if (toggleValue)
        {
            GameManager.linePlayer = true;
        }
        else
        {
            GameManager.linePlayer = false;
        }
    }
}
