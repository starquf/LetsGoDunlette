using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Anim_TextUp : AnimObj
{
    public Text textValue;
    private TextUpAnimType currentType = TextUpAnimType.Up;

    protected override void Awake()
    {
        base.Awake();

        originColor = textValue.color;
    }

    public void SetType(TextUpAnimType type)
    {
        currentType = type;
    }

    public void SetTextColor(Color color)
    {
        textValue.color = color;
    }

    public void Play(string value)
    {
        textValue.text = value;
        base.Play();
    }

    protected override IEnumerator PlayAnim(Action onEndAnim)
    {
        yield return null;

        float time = anim.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0;

        Vector3 startSize = textValue.transform.localScale;

        textValue.transform.DOScale(startSize * 1.5f, time / 2f - 0.05f)
            .SetLoops(2, LoopType.Yoyo);

        switch (currentType)
        {
            case TextUpAnimType.Fixed:
                {
                    while (timer < time)
                    {
                        timer += Time.deltaTime;

                        yield return null;
                    }
                }
                break;

            case TextUpAnimType.Up:
                {
                    textValue.transform.DOLocalMoveY(transform.localPosition.y + 250f, time / 2f);

                    while (timer < time)
                    {
                        timer += Time.deltaTime;

                        yield return null;
                    }
                }
                break;

            case TextUpAnimType.Volcano:
                {
                    Vector3 startPos = transform.position;
                    Vector3 endPos = transform.position + Vector3.down + (Vector3.right * Random.Range(-1f, 1f));
                    float height = Random.Range(1.1f, 1.3f);

                    while (timer < time)
                    {
                        timer += Time.deltaTime;

                        Vector3 tempPos = Parabola(startPos, endPos, height, timer * 1.32f);
                        transform.position = tempPos;

                        yield return null;
                    }
                }
                break;

            default:
                break;
        }

        onEndAnim?.Invoke();

        ResetAnim();


        gameObject.SetActive(false);
    }

    protected override void ResetAnim()
    {
        base.ResetAnim();

        ResetText();
    }

    private void ResetText()
    {
        textValue.transform.localScale = Vector3.one;
        textValue.transform.localPosition = Vector3.zero;
        currentType = TextUpAnimType.Up;
        textValue.color = originColor;
    }

    protected Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float t2 = -4 * height * t * t + 4 * height * t;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, t2 + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
