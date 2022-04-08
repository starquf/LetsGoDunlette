using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_ChainExplosion : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SkillEvent eventInfo = null;
        BattleHandler bh = GameManager.Instance.battleHandler;
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;

        Anim_F_ChainExplosion staticEffect = PoolManager.GetItem<Anim_F_ChainExplosion>();
        staticEffect.transform.position = targetPos;

        staticEffect.Play(() => {
            target.GetDamage(Value, currentType);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            Action<SkillPiece, Action> onNextAttack = (result,action) => { };
            int targetHp = target.curHp;

            onNextAttack = (result,action) =>
            {
                print("연폭");
                //print($"현재체력 : {target.curHp}       예전 체력 : {targetHp}");

                if (result.isPlayerSkill && target.curHp < targetHp)
                {
                    print("발동!");

                    target.GetDamage(Value, patternType);
                    GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

                    Anim_F_ChainExplosionBonus bonusEffect = PoolManager.GetItem<Anim_F_ChainExplosionBonus>();
                    bonusEffect.transform.position = targetPos;

                    bonusEffect.Play();
                }

                action?.Invoke();
                // 바로 없엘거면 이렇게
                //bh.battleEvent.RemoveEventInfo(eventInfo);
            };

            // 이벤트에 추가해주면 됨
            eventInfo = new SkillEvent(true,2,EventTimeSkill.AfterSkill, onNextAttack);
            bh.battleEvent.BookEvent(eventInfo);

            onCastEnd?.Invoke();
        });
    }
}
