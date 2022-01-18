using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_PosionCloud : SkillPiece
{
    public GameObject posionCloudEffectPrefab;

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"��ų �ߵ�!! �̸� : {PieceName}");

        BattleHandler bh = GameManager.Instance.battleHandler;
        Vector3 targetPos = target.transform.position;

        Anim_N_PosionCloud posionCloudEffect = PoolManager.GetItem<Anim_N_PosionCloud>();
        posionCloudEffect.transform.position = targetPos;

        posionCloudEffect.Play(() => {
            if(!CheckSilence())
            {
                target.GetComponent<SpriteRenderer>().color = Color.green;
                target.cc.SetCC(CCType.Wound, 4);
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
                posionCloudEffect.transform.position = targetPos;

                posionCloudEffect.Play(() => {
                    target.GetDamage(Value);
                    GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
                });
            }
            cnt--;

            // �ٷ� �����Ÿ� �̷���
            if(cnt == 0)
            {
                target.GetComponent<SpriteRenderer>().color = Color.white;
                bh.onNextAttack -= onNextTest;
            }
        };

        // �̺�Ʈ�� �߰����ָ� ��
        bh.onNextAttack += onNextTest;
    }
}
