using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutAnim : MonoBehaviour
{
    public float fadeTime = 1f;
    public Text text;

    public float start;
    public float end;

    private float timer;

    private void OnEnable()
    {
        StartCoroutine(FadeInOutLoop());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator FadeInOutLoop()
    {
        while (true)
        {
            yield return FadeIn();
            yield return null;
            yield return FadeOut();
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        Color color = text.color;
        timer = 0;
        while (timer <= 1)
        {
            timer += Time.deltaTime / fadeTime;

            color.a = Mathf.Lerp(start, end, timer);
            text.color = color;

            yield return null;
        }

        yield return null;
    }

    public IEnumerator FadeOut()
    {
        Color color = text.color;
        timer = 0;
        while (timer <= 1)
        {
            timer += Time.deltaTime / fadeTime;

            color.a = Mathf.Lerp(end, start, timer);
            text.color = color;

            yield return null;
        }

        yield return null;
    }
}
