using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_ManaSphere : SkillPiece
{
    public GameObject e_ManaSpherePrefab;

    public override void Cast()
    {
        base.Cast();
        print($"��ų �ߵ�!! �̸� : {PieceName}");


        Vector3 target = GameManager.Instance.battleHandler.enemy.transform.position;
        Vector3 startPos = GameManager.Instance.battleHandler.player.transform.position;

        Effect_E_ManaSphere staticEffect = Instantiate(e_ManaSpherePrefab, target, Quaternion.identity).GetComponent<Effect_E_ManaSphere>();
        staticEffect.transform.position = startPos;
        staticEffect.targetPos = target;

        staticEffect.Play(target, ()=> {
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            staticEffect.gameObject.SetActive(false); 
        });

        GameManager.Instance.battleHandler.enemy.GetDamage(Value);
        //StartCoroutine(EffectCast());
    }
}
