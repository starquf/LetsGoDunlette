using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Lightning : SkillPiece
{
    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        desInfos[1].SetInfo(DesIconType.Stun, "70%");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.3f + 2);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        int damage = GetDamageCalc();

        target.GetDamage(damage, currentType);

        LogCon log = new LogCon();
        log.text = $"{damage} 데미지 부여";
        log.selfSpr = skillImg.sprite;
        log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

        animHandler.GetAnim(AnimName.E_Static)
        .SetScale(1.2f)
        .SetPosition(target.transform.position)
        .Play(() => 
        {
            PlayerHealth playerHealth = Owner.GetComponent<PlayerHealth>();
            if (playerHealth.HasShield())
            {
                if (Random.Range(0, 100) < 60)
                {
                    animHandler.GetAnim(AnimName.E_Static_Stun)
                    .SetPosition(bh.playerImgTrans.position)
                    .Play();

                    playerHealth.cc.SetCC(CCType.Stun, 1);

                    LogCon log = new LogCon();
                    log.text = $"기절시킴";
                    log.selfSpr = skillImg.sprite;
                    log.targetSpr = playerHealth.GetComponent<SpriteRenderer>().sprite;

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
                }
            }
            else
            {
                if (Random.Range(0, 100) < 70)
                {
                    animHandler.GetAnim(AnimName.E_Static_Stun)
                    .SetPosition(target.transform.position)
                    .Play();

                    target.cc.SetCC(CCType.Stun, 1);

                    LogCon log = new LogCon();
                    log.text = $"기절시킴";
                    log.selfSpr = skillImg.sprite;
                    log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
                }
            }

            onCastEnd?.Invoke();
        });

        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.2f);
    }
}
