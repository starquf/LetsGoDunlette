using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RF_Present : SkillPiece
{
    private Sprite originIcon;
    private Sprite originSkillTypeIcon;

    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
        originIcon = skillIconImg.sprite;
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }

    public override void OnRullet()
    {
        base.OnRullet();
        List<RulletPiece> pieces = bh.mainRullet.GetPieces();
        List<SkillPiece> targetList = new List<SkillPiece>();

        SkillPiece target = null;

        for (int i = 0; i < pieces.Count; i++)
        {
            SkillPiece piece = pieces[i] as SkillPiece;

            if (piece != null && piece.isPlayerSkill)
            {
                targetList.Add(piece);
            }
        }

        if (targetList.Count > 0)
        {
            target = targetList[Random.Range(0, targetList.Count)];

            bgImg.sprite = GameManager.Instance.inventoryHandler.pieceBGSprDic[target.currentType];
            skillIconImg.sprite = target.skillIconImg.sprite;
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        skillIconImg.sprite = originIcon;

        SetIndicator(Owner.gameObject, "АјАн").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(Value), this, Owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Butt).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
