using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObj : MonoBehaviour
{
    private Animator anim;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Play(string animName, Action onEndAnim = null)
    {
        anim.Play(animName);
        StartCoroutine(WaitAnim(onEndAnim));
    }

    private IEnumerator WaitAnim(Action onEndAnim)
    {
        float time = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(time);

        onEndAnim?.Invoke();
        gameObject.SetActive(false);
    }
}
