using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RF_Present : SkillPiece
{
    private Sprite originIcon;
    private Sprite originSkillTypeIcon;

    private Image skillTypeIconImage;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
        skillTypeIconImage = skillImg.transform.GetChild(0).GetComponent<Image>();
        originIcon = skillImg.sprite;
        originSkillTypeIcon = skillTypeIconImage.sprite;
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
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
            skillImg.sprite = target.skillImg.sprite;
            skillTypeIconImage.sprite = target.skillImg.transform.GetChild(0).GetComponent<Image>().sprite; //여기서 왜 에러남 ?
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        skillImg.sprite = originIcon;
        skillTypeIconImage.sprite = originSkillTypeIcon;

        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(Value), this, Owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
