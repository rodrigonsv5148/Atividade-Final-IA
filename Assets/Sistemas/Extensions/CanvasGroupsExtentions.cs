using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CanvasGroupsExtentions
{
    public static IEnumerator AtivarCanvasGroup(this CanvasGroup a, bool ativar, float velocity = 0.01f)
    {
        float factor = 0;
        a.blocksRaycasts = ativar;
        a.interactable = ativar;
        float initAlpha = a.alpha;
        float targetAlpha = ativar ? 1 : 0;
        while (factor <= 1)
        {
            a.alpha = Mathf.Lerp(initAlpha, targetAlpha, Mathf.Clamp01(factor));
            factor += velocity;
            yield return null;
        }
    }
    public static IEnumerator FadeInAlpha(this CanvasGroup a, bool fadeIN, float velocity = 0.01f)
    {
        yield return new WaitForSeconds(1);
        float factor = 0;
        float initAlpha = a.alpha;
        float targetAlpha = fadeIN ? 1 : 0;
        while (factor <= 1)
        {
            a.alpha = Mathf.Lerp(initAlpha, targetAlpha, Mathf.Clamp01(factor));
            factor += velocity;
            yield return null;
        }
        a.alpha = Mathf.Lerp(initAlpha, targetAlpha, 1);
    }
}
