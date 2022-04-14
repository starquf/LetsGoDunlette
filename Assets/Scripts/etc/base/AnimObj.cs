using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObj : MonoBehaviour
{
    protected Animator anim;

    protected Vector3 originScale;
    protected Quaternion originRot;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        originScale = transform.localScale;
        originRot = transform.rotation;
    }

    public void SetScale(float scale)
    {
        transform.localScale = originScale * scale;
    }

    public void SetRotation(Vector3 rot)
    {
        transform.eulerAngles = rot;
    }

    public virtual void Play(Action onEndAnim = null)
    {
        StartCoroutine(WaitAnim(onEndAnim));
    }

    protected virtual IEnumerator WaitAnim(Action onEndAnim)
    {
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
    }
}
