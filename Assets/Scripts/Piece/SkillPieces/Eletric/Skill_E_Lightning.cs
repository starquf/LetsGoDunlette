using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Lightning : SkillPiece
{
    private BattleHandler bh = null;

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
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.3f + 2);

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

        Anim_E_Static effect = PoolManager.GetItem<Anim_E_Static>();
        effect.transform.position = target.transform.position;
        effect.SetScale(1.2f);

        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.2f);

        effect.Play(() =>
        {
            PlayerHealth playerHealth = owner.GetComponent<PlayerHealth>();
            if (playerHealth.HasShield())
            {
                if (Random.Range(0, 100) < 60)
                {
                    Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                    stunEffect.transform.position = bh.playerImgTrans.position;

                    stunEffect.Play();

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
                    Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                    stunEffect.transform.position = target.transform.position;

                    stunEffect.Play();

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

    }
}
