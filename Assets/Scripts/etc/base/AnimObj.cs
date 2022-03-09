using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObj : MonoBehaviour
{
    private Animator anim;

    private Vector3 originScale;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        originScale = transform.localScale;
    }

    public void SetScale(float scale)
    {
        transform.localScale = originScale * scale;
    }

    public void Play(Action onEndAnim = null)
    {
        StartCoroutine(WaitAnim(onEndAnim));
    }

    private IEnumerator WaitAnim(Action onEndAnim)
    {
        float time = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(time);

        onEndAnim?.Invoke();

        transform.localScale = originScale;
        gameObject.SetActive(false);
    }
}
