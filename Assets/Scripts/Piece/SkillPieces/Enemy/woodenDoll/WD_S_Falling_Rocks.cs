using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WD_S_Falling_Rocks : SkillPiece
{
    private int percent = 30;

    private readonly Vector3 reverseSize = new Vector3(-2f, 2f, 2f);

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        desInfos[1].SetInfo(DesIconType.Stun, $"{percent}%");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(Value), this, Owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            if (Random.Range(0, 100) < percent)
            {
                animHandler.GetTextAnim()
                .SetType(TextUpAnimType.Fixed)
                .SetPosition(target.transform.position)
                .SetScale(0.7f)
                .Play("기절!");

                target.cc.SetCC(CCType.Stun, 1);
            }

            AnimName animName = Random.Range(0, 100) < 50 ? AnimName.EarthEffect02 : AnimName.EarthEffect03;

            animHandler.GetAnim(animName).SetPosition(Owner.transform.position + Vector3.left)
            .SetScale(reverseSize * 2f)
            .Play();

            animName = Random.Range(0, 100) < 50 ? AnimName.EarthEffect02 : AnimName.EarthEffect03;

            animHandler.GetAnim(animName).SetPosition(Owner.transform.position + Vector3.right)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
