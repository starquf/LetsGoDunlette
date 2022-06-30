using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_High_Voltage : SkillPiece
{
    private Action<Action> highVolatge = null;
    private NormalEvent normalEvent = null;

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Shield, $"{Value}");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        LivingEntity owner = Owner.GetComponent<LivingEntity>();

        owner.AddShield(Value);

        highVolatge = (a) =>
        {
            if (UnityEngine.Random.Range(0, 100) < 50)
            {
                if (owner.GetShieldHp() <= 0)
                {
                    print("¹ßµ¿µÊ!");

                    // ¼¼ÀÌ·»
                    if (target == bh.player)
                    {
                        bh.player.cc.SetCC(CCType.Stun, 1);
                    }
                    else
                    {
                        for (int i = 0; i < bh.enemys.Count; i++)
                        {
                            bh.enemys[i].cc.SetCC(CCType.Stun, 1);
                        }
                    }

                    GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.2f);
                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(1.5f, 0.2f);

                    animHandler.GetTextAnim()
                    .SetPosition(owner.transform.position)
                    .SetType(TextUpAnimType.Fixed)
                    .Play("°íÀü¾Ð ¹ßµ¿µÊ!");

                    animHandler.GetAnim(AnimName.Anim_ElecEffect07)
                    .SetPosition(owner.transform.position)
                    .SetScale(1f)
                    .Play(() =>
                    {
                        a?.Invoke();
                    });
                }
                else 
                {
                    a?.Invoke();
                }
            }
            else
            {
                a?.Invoke();
            }
        };

        normalEvent = new NormalEvent(true, 2, highVolatge, EventTime.EndOfTurn);

        bh.battleEvent.BookEvent(normalEvent);

        animHandler.GetAnim(AnimName.Anim_ElecEffect02)
        .SetPosition(Owner.transform.position)
        .SetScale(2f)
        .Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }
}
