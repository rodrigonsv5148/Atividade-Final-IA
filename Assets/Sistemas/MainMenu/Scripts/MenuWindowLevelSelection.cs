// Change the mesh color in response to mouse actions.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuWindowLevelSelection : MonoBehaviour
{

   

IEnumerator MenuFadeLevelCall()
{

 yield return new WaitForSeconds(1);
 
}

    // public Image gameImage;
    // public Color32 initialColor;
    // public Vector3 initialScale;

    // void Start()
    // {
        // initialColor = gameImage.color;
        // initialScale = new Vector3(1,1,1);
        // gameImage = GetComponent<Image>();
    // }

    // The mesh goes red when the mouse is over it...
    // void OnMouseEnter()
    // {
    //     gameImage.color = new Color32(1, 0, 0,1);

    //     transform.localScale = new Vector3(1.05f,1.05f,1);
    // }

    // ...the red fades out to cyan as the mouse is held over...
    // void OnMouseOver()
    // {
    //     gameImage.color = new Color32(1, 0, 0,1);
  
    // }

    // ...and the mesh finally turns white when the mouse moves away.
    // void OnMouseExit()
    // {
    //     transform.localScale = initialScale;
    //     gameImage.color = initialColor;
    // }
}