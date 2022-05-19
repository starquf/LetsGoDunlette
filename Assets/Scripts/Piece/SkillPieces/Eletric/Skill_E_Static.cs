using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_Static : SkillPiece
{
    [Header("기절 확률")]
    public int stunPercent;

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        desInfos[1].SetInfo(DesIconType.Stun, $"{stunPercent}%");

        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 targetPos = target.transform.position;
        targetPos.y -= 0.7f;

        animHandler.GetAnim(AnimName.E_Static)
        .SetPosition(targetPos)
        .Play(() =>
        {
            int damage = GetDamageCalc();

            target.GetDamage(GetDamageCalc(), currentType);

            LogCon log = new LogCon
            {
                text = $"{GetDamageCalc()} 데미지 부여",
                selfSpr = skillImg.sprite,
                targetSpr = target.GetComponent<SpriteRenderer>().sprite
            };

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            if (Random.Range(0, 100) < stunPercent)
            {
                animHandler.GetAnim(AnimName.E_Static_Stun)
                    .SetPosition(target.transform.position)
                    .Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });

                target.cc.SetCC(CCType.Stun, 1);

                log = new LogCon
                {
                    text = $"기절시킴",
                    selfSpr = skillImg.sprite,
                    targetSpr = target.GetComponent<SpriteRenderer>().sprite
                };

                DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
    }
}
