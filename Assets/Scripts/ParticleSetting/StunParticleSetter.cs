using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StunParticleSetter : BuffParticleSetter
{
    public override void Play(float time)
    {
        StartCoroutine(PlayAnim(time));
    }

    public IEnumerator PlayAnim(float time)
    {
        yield return new WaitForSeconds(time);

        damageBGEffect.color = buffColor;
        damageBGEffect.DOFade(0f, 0.55f);

        buffParticle.Play();

        GameManager.Instance.animHandler.GetAnim(AnimName.PlayerStunned).SetPosition(GameManager.Instance.battleHandler.mainRullet.transform.position)
            .SetScale(4f)
            .Play();
    }
}
