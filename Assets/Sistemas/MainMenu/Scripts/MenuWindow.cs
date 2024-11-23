using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuWindow : MonoBehaviour
{
    public CanvasGroup MyCanvasGroup { get; set; }
    private bool canvasCheck;
    private float fadespeed;
    public bool withSlide;

    private void Awake()
    {
        MyCanvasGroup = GetComponent<CanvasGroup>();
    }
    public void Start()
    {
        fadespeed = 0.7f;
        canvasCheck = false;
        MyCanvasGroup.alpha = 0;
        MyCanvasGroup.blocksRaycasts = canvasCheck;
        if (withSlide)
        {
            MyCanvasGroup.transform.localPosition = new Vector2(-Screen.width / 2, MyCanvasGroup.transform.localPosition.y);
        }
    }
    public void MenuCanvas()
    {
        if (withSlide)
        {
            StartCoroutine(DoSlide(canvasCheck, fadespeed));
        }

        if (canvasCheck == true)
        {
            StartCoroutine(DoFade(0, fadespeed));
        }
        else
        {
            StartCoroutine(DoFade(1, fadespeed));
        }
        canvasCheck = !canvasCheck;
        MyCanvasGroup.blocksRaycasts = canvasCheck;
    }
    private IEnumerator DoFade(float alphaTarget, float time)
    {
        if (MyCanvasGroup == null)
        {
            yield break;
        }
        float initialAlpha = MyCanvasGroup.alpha;

        float finalAlpha = alphaTarget;
        float timeCount = 0;
        while (timeCount < time)
        {
            timeCount = Mathf.Min(timeCount + Time.deltaTime, time);
            float alpha = Mathf.Lerp(initialAlpha, finalAlpha, timeCount / time);
            if (MyCanvasGroup != null) MyCanvasGroup.alpha = alpha;
            yield return null;
        }
    }

    private IEnumerator DoSlide(bool slideIn, float time)
    {
        if (MyCanvasGroup == null)
        {
            yield break;
        }
        RectTransform rect = MyCanvasGroup.GetComponent<RectTransform>();
        float screenWidth = rect.rect.width;
        float initialPosition = slideIn == false ? -screenWidth / 2 : screenWidth / 2;

        float finalPosition = slideIn == false ? screenWidth / 2 : -screenWidth / 2;

        float timeCount = 0;
        while (timeCount < time)
        {
            timeCount = Mathf.Min(timeCount + Time.deltaTime, time);
            float xPosition = Mathf.Lerp(initialPosition, finalPosition, timeCount / time);
            Vector3 position = new Vector3(xPosition, MyCanvasGroup.transform.localPosition.y, MyCanvasGroup.transform.localPosition.z);
            if (MyCanvasGroup != null) MyCanvasGroup.transform.localPosition = position;
            yield return null;
        }
    }
}