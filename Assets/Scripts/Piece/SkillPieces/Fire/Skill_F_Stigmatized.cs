using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_Stigmatized : SkillPiece
{
    private BattleHandler bh = null;
    SkillEvent eventInfo = null;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = true;
    }

    public override void OnRullet()
    {
        base.OnRullet();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        int targetHp = target.curHp;

        Action<SkillPiece,Action> action = (p,action) => { };

        action = (piece,action) =>
        {
            if (target.IsDie)
            {
                bh.battleEvent.RemoveEventInfo(eventInfo);
                action?.Invoke();
                return;
            }
            //print($"ī��Ʈ��! �� ���� ü�� : {targetHp}  ���� ü�� : {target.curHp}");

            if (piece.currentType.Equals(PatternType.Heart) && target.curHp < targetHp)
            {
                //print("�߰��� ��!");

                Anim_F_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
                hitEffect.transform.position = target.transform.position + Vector3.down * 0.2f;
                hitEffect.SetScale(0.7f);
                hitEffect.Play(() =>
                {
                    action?.Invoke();
                });
                GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

                target.GetDamage(30, patternType);
            }


            targetHp = target.curHp;

            action?.Invoke();
        };

        eventInfo = new SkillEvent(EventTimeSkill.AfterSkill, action);
        bh.battleEvent.BookEvent(eventInfo);

        target.GetDamage(Value, currentType);

        Anim_F_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
        hitEffect.transform.position = target.transform.position;
        hitEffect.SetScale(0.7f);
        hitEffect.Play(() =>
        {
            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = target.transform.position;
            textEffect.SetScale(0.7f);
            textEffect.Play("���ε�!");

            onCastEnd?.Invoke();
        });
    }
}
