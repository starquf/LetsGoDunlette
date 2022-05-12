using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_F_ChainExplosion : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc().ToString()}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.7f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SkillEvent eventInfo = null;
        BattleHandler bh = GameManager.Instance.battleHandler;
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;

        int damage = GetDamageCalc();

        animHandler.GetAnim(AnimName.F_ChainExplosion)
            .SetPosition(targetPos)
            .SetScale(1.3f)
            .Play(() => 
            {
                target.GetDamage(damage, currentType);
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                Action<SkillPiece, Action> onNextAttack = (result,action) => { };
                int targetHp = target.curHp;

                onNextAttack = (result,action) =>
                {
                    //print($"현재체력 : {target.curHp}       예전 체력 : {targetHp}");

                    if (result.isPlayerSkill && target.curHp < targetHp)
                    {
                        target.GetDamage(5, patternType);
                        GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

                        animHandler.GetAnim(AnimName.F_ChainExplosionBonus)
                            .SetPosition(targetPos)
                            .Play();

                        animHandler.GetTextAnim()
                        .SetType(TextUpAnimType.Fixed)
                        .SetPosition(target.transform.position)
                        .SetScale(0.8f)
                        .Play("연쇄폭발 효과 발동!");
                    }

                    action?.Invoke();
                    // 바로 없엘거면 이렇게
                    //bh.battleEvent.RemoveEventInfo(eventInfo);
                };

            // 이벤트에 추가해주면 됨
            eventInfo = new SkillEvent(true, 2, EventTimeSkill.AfterSkill, onNextAttack);
            bh.battleEvent.BookEvent(eventInfo);

            onCastEnd?.Invoke();
        });
    }
}
