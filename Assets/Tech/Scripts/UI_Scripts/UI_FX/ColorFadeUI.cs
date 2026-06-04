using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorFadeUI : MonoBehaviour
{
    public static IEnumerator FadeIn(VisualElement transitionScreen) {
        transitionScreen.style.display = DisplayStyle.Flex;
        
        float
            r = transitionScreen.resolvedStyle.backgroundColor.r,
            g = transitionScreen.resolvedStyle.backgroundColor.g,
            b = transitionScreen.resolvedStyle.backgroundColor.b,
            a = 0;

        while (a < 1) {
            yield return new WaitForSeconds(0.01f);
            a += 0.02f;
            transitionScreen.style.backgroundColor = new Color(r, g, b, a);
            if (a > 1) { a = 1; }
        }
    }

    public static IEnumerator FadeOut(VisualElement transitionScreen) {
        transitionScreen.style.display = DisplayStyle.Flex;

        float
            r = transitionScreen.resolvedStyle.backgroundColor.r,
            g = transitionScreen.resolvedStyle.backgroundColor.g,
            b = transitionScreen.resolvedStyle.backgroundColor.b,
            a = 1;

        while (a > 0) {
            yield return new WaitForSeconds(0.01f);
            a -= 0.02f;
            transitionScreen.style.backgroundColor = new Color(r, g, b, a);
        }

        transitionScreen.style.display = DisplayStyle.None;
    }
}
