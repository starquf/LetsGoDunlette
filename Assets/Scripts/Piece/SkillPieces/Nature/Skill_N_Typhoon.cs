using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Typhoon : SkillPiece
{
    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        isTargeting = false;
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
        
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        List<LivingEntity> targets = new List<LivingEntity>();

        EnemyHitEffectUtil.Typhoon(target.gameObject);

        if (target == bh.player)
        {
            targets.Add(target);
        }
        else
        {
            targets = bh.battleUtil.DeepCopyEnemyList(bh.enemys);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            Vector2 attackPos = targets[i].transform.position;

            animHandler.GetAnim(AnimName.N_Wind)
                .SetPosition(attackPos)
                .SetScale(0.7f)
                .Play();

            targets[i].GetDamage(GetDamageCalc(Value), currentType);

            if (targets.Count >= 2)
            {
                targets[i].GetDamage(5, ElementalType.Nature);
            }
        }

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();

        for (int i = 0; i < 6; i++)
        {
            bh.battleUtil.SetPieceToGraveyard(i);
        }

        Vector2 effectPos = bh.mainRullet.transform.position;

        animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(effectPos)
        .SetScale(0.8f)
        .Play("태풍 효과발동!");

        for (int i = 0; i < 7; i++)
        {
            animHandler.GetAnim(AnimName.N_Wind)
                .SetPosition(effectPos + (Random.insideUnitCircle * 2f))
                .SetScale(Random.Range(0.15f, 0.5f))
                .Play();
        }

        GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.3f);
        GameManager.Instance.shakeHandler.ShakeBackCvsUI(1.5f, 0.3f);

        onCastEnd?.Invoke();
    }
}
