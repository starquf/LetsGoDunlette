using System;
using System.Collections;
using UnityEngine;

public class AnimObj : MonoBehaviour
{
    protected Animator anim;
    protected AnimatorOverrideController aoc;

    public Vector3 originScale;
    protected Quaternion originRot;
    protected Color originColor;
    protected LayerMask originnSortingLayer;
    protected int originnSortingOrder;

    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        InitComponent();
        InitAnim();

        originRot = Quaternion.identity;
        originScale = transform.localScale;
        originColor = Color.white;
        originnSortingLayer = sr.sortingLayerID;
        originnSortingOrder = sr.sortingOrder;
    }

    protected virtual void InitComponent()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void InitAnim()
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController
        {
            runtimeAnimatorController = anim.runtimeAnimatorController
        };

        anim.runtimeAnimatorController = aoc;

        this.aoc = aoc;
    }

    public virtual AnimObj SetScale(float scale)
    {
        transform.localScale = originScale * scale;

        return this;
    }

    public virtual AnimObj SetScale(Vector3 scale)
    {
        transform.localScale = scale;

        return this;
    }

    public virtual AnimObj SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;

        return this;
    }

    public virtual AnimObj SetPosition(Vector3 pos)
    {
        transform.position = pos;

        return this;
    }

    public virtual AnimObj SetColor(Color color)
    {
        sr.color = color;

        return this;
    }

    public virtual AnimObj SetSortLayer(LayerMask layerMask, int order = 0)
    {
        sr.sortingLayerID = layerMask;
        sr.sortingOrder = order;

        return this;
    }

    public virtual void Play(Action onEndAnim = null)
    {
        StartCoroutine(PlayAnim(onEndAnim));
    }

    public virtual void SetAnim(AnimationClip clip)
    {
        aoc["Play"] = clip;
    }

    protected virtual IEnumerator PlayAnim(Action onEndAnim)
    {
        yield return null;

        float time = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(time);

        onEndAnim?.Invoke();

        gameObject.SetActive(false);
    }

    public virtual void ResetAnim()
    {
        transform.localScale = originScale;
        transform.rotation = originRot;
        sr.color = originColor;
        sr.sortingLayerID = originnSortingLayer;
        sr.sortingOrder = originnSortingOrder;
    }
}
