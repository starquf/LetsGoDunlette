using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_PosionCloud : SkillPiece
{
    public GameObject posionCloudEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        BattleHandler bh = GameManager.Instance.battleHandler;
        Vector3 target = bh.enemy.transform.position;

        Anim_N_PosionCloud posionCloudEffect = PoolManager.GetItem<Anim_N_PosionCloud>();
        posionCloudEffect.transform.position = target;

        posionCloudEffect.Play(() => {
            if(!CheckSilence())
            {
                bh.enemy.GetComponent<SpriteRenderer>().color = Color.green;
                bh.enemy.cc.SetCC(CCType.Wound, 4);
            }
            onCastEnd?.Invoke();
        });


        Action<RulletPiece> onNextTest = result => { };
        int cnt = 4;

        onNextTest = result =>
        {
            if (!CheckSilence())
            {
                Anim_N_PosionCloud posionCloudEffect = PoolManager.GetItem<Anim_N_PosionCloud>();
                posionCloudEffect.transform.position = target;

                posionCloudEffect.Play(() => {
                    bh.enemy.GetDamage(Value);
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
                });
            }
            cnt--;

            // 바로 없엘거면 이렇게
            if(cnt == 0)
            {
                bh.enemy.GetComponent<SpriteRenderer>().color = Color.white;
                bh.onNextAttack -= onNextTest;
            }
        };

        // 이벤트에 추가해주면 됨
        bh.onNextAttack += onNextTest;
    }
}
