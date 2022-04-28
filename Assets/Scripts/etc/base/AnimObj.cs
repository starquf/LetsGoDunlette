using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObj : MonoBehaviour
{
    protected Animator anim;

    protected AnimatorOverrideController aoc;

    protected Vector3 originScale;
    protected Quaternion originRot;
    protected Color originColor;

    protected SpriteRenderer sr;

    protected int playTrigger;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        InitAnim();

        originRot = Quaternion.identity;
        originScale = Vector3.one;
        originColor = Color.white;
    }

    public virtual void InitAnim()
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController();
        aoc.runtimeAnimatorController = anim.runtimeAnimatorController;

        anim.runtimeAnimatorController = aoc;

        this.aoc = aoc;

        playTrigger = Animator.StringToHash("Play");
    }

    public AnimObj SetScale(float scale)
    {
        transform.localScale = originScale * scale;

        return this;
    }

    public AnimObj SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;

        return this;
    }

    public AnimObj SetPosition(Vector3 pos)
    {
        transform.position = pos;

        return this;
    }

    public virtual void Play(Action onEndAnim = null)
    {
        anim.SetTrigger(playTrigger);
        StartCoroutine(PlayAnim(onEndAnim));
    }

    public void SetAnim(AnimationClip clip)
    {
        aoc["Play"] = clip;
    }

    protected virtual IEnumerator PlayAnim(Action onEndAnim)
    {
        yield return null;

        float time = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(time);

        onEndAnim?.Invoke();

        ResetAnim();

        gameObject.SetActive(false);
    }

    protected virtual void ResetAnim()
    {
        transform.localScale = originScale;
        transform.rotation = originRot;
        sr.color = originColor;
    }
}
