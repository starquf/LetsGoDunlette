using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Stigmatized : SkillPiece
{
    private BattleHandler bh = null;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        int turnCnt = 4;
        int targetHp = target.curHp;

        Action<SkillPiece> action = p => { };

        action = piece =>
        {
            print($"카운트중! 적 예전 체력 : {targetHp}  현재 체력 : {target.curHp}");
            turnCnt--;

            if (piece.currentType.Equals(PatternType.Heart) && target.curHp < targetHp)
            {
                print("추가뎀 들어감!");

                Anim_F_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
                hitEffect.transform.position = target.transform.position + Vector3.down * 0.2f;
                hitEffect.SetScale(0.7f);
                hitEffect.Play();

                GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

                target.GetDamage(Value);
            }

            if (turnCnt <= 0 || target.IsDie)
            {
                bh.battleEvent.onNextSkill -= action;
            }

            targetHp = target.curHp;
        };

        bh.battleEvent.onNextSkill += action;

        Anim_F_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
        hitEffect.transform.position = target.transform.position;
        hitEffect.SetScale(0.7f);
        hitEffect.Play(() =>
        {
            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = target.transform.position;
            textEffect.SetScale(0.7f);
            textEffect.Play("낙인됨!");

            onCastEnd?.Invoke();
        });
    }
}
