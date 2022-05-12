using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Static : SkillPiece
{
    public GameObject staticEffectPrefab;
    public GameObject staticStunEffectPrefab;

    [Header("기절 확률")]
    public int stunPercent;

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        desInfos[1].SetInfo(DesIconType.Stun, $"{stunPercent.ToString()}%");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.2f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        //print($"스킬 발동!! 이름 : {PieceName}");

        Vector3 targetPos = target.transform.position;
        targetPos.y -= 0.7f;

        animHandler.GetAnim(AnimName.E_Static)
        .SetPosition(targetPos)
        .Play(() =>
        {
            int damage = GetDamageCalc();

            target.GetDamage(GetDamageCalc(), currentType);

            LogCon log = new LogCon();
            log.text = $"{GetDamageCalc()} 데미지 부여";
            log.selfSpr = skillImg.sprite;
            log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            if (Random.Range(0, 100) < stunPercent)
            {
                animHandler.GetAnim(AnimName.E_Static_Stun)
                    .SetPosition(target.transform.position)
                    .Play(() => {
                        onCastEnd?.Invoke();
                    });

                target.cc.SetCC(CCType.Stun, 1);

                log = new LogCon();
                log.text = $"기절시킴";
                log.selfSpr = skillImg.sprite;
                log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

                DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
    }
}
