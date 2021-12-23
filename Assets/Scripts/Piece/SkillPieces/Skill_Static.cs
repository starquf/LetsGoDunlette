using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Static : SkillPiece
{
    public GameObject staticEffectPrefab;

    public override void Cast()
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        StartCoroutine(EffectCast());

    }

    public IEnumerator EffectCast()
    {
        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        ParticleSystem effect = Instantiate(staticEffectPrefab, target, Quaternion.identity).GetComponent<ParticleSystem>();
        effect.Play();
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        yield return new WaitUntil(()=> !effect.IsAlive());
        Destroy(effect.gameObject);
        //effect.gameObject.SetActive(false);
        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
    }
}
