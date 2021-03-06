using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Anim_TextUp : AnimObj
{
    public Text textValue;
    public AnimationClip textClip;

    private TextUpAnimType currentType = TextUpAnimType.Up;

    protected override void Awake()
    {
        base.Awake();

        originColor = textValue.color;
    }

    protected override void InitComponent()
    {
        anim = GetComponent<Animator>();
        textValue = GetComponentInChildren<Text>();
    }

    public override void InitAnim()
    {
        base.InitAnim();
        aoc["Play"] = textClip;
    }

    public Anim_TextUp SetType(TextUpAnimType type)
    {
        currentType = type;

        return this;
    }

    public Anim_TextUp SetTextColor(Color color)
    {
        textValue.color = color;

        return this;
    }

    public new Anim_TextUp SetScale(float scale)
    {
        transform.localScale = originScale * scale;

        return this;
    }

    public new Anim_TextUp SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;

        return this;
    }

    public new Anim_TextUp SetPosition(Vector3 pos)
    {
        transform.position = pos;

        return this;
    }

    public void Play(string value)
    {
        textValue.text = value;
        base.Play();
    }

    public override void SetAnim(AnimationClip clip)
    {

    }

    protected override IEnumerator PlayAnim(Action onEndAnim)
    {
        yield return null;

        float time = anim.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0;

        Vector3 startSize = textValue.transform.localScale;

        textValue.transform.DOScale(startSize * 1.5f, (time / 2f) - 0.05f)
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
                    Vector3 endPos = transform.position + Vector3.down + (Vector3.right * Random.Range(-1.1f, 1.1f));
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

    public override void ResetAnim()
    {
        transform.localScale = originScale;
        transform.rotation = originRot;

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
        float t2 = (-4 * height * t * t) + (4 * height * t);

        Vector3 mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, t2 + Mathf.Lerp(start.y, end.y, t), mid.z);
    }
}
