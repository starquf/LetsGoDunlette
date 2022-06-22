using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_N_Bamboo_Forest : SkillPiece
{
    public Sprite bambooSpr;
    private GameObject bambooSpear;

    protected override void Start()
    {
        base.Start();
        bambooSpear = GameManager.Instance.skillContainer.GetSkillPrefab<Skill_N_Bamboo_Spear>();
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //자연 개수 + 개
    {
        animHandler.GetAnim(AnimName.SkillEffect01)
        .SetPosition(Owner.transform.position)
        .SetScale(2f)
        .SetRotation(Vector3.forward * -90f)
        .Play(() =>
        {
            List<RulletPiece> rulletPieces = bh.mainRullet.GetPieces();
            int count = 0;

            for (int i = 0; i < rulletPieces.Count; i++)
            {
                if (rulletPieces[i] != null)
                {
                    if (rulletPieces[i].patternType == ElementalType.Nature)
                    {
                        count++;

                        int a = i;

                        bh.battleUtil.SetTimer(0.10f * i, () => 
                        {
                            animHandler.GetTextAnim()
                            .SetPosition(rulletPieces[a].skillIconImg.transform.position)
                            .SetType(TextUpAnimType.Up)
                            .Play("대나무 숲 효과 발동!");

                            CreateBambooEffect(rulletPieces[a].skillIconImg.transform.position);

                            GameManager.Instance.inventoryHandler.CreateSkill(bambooSpear, Owner, Owner.transform.position); 
                        });
                    }
                }
            }

            bh.battleUtil.SetTimer(0.10f, () => { GameManager.Instance.inventoryHandler.CreateSkill(bambooSpear, Owner, Owner.transform.position); });
            bh.battleUtil.SetTimer(0.10f * count, onCastEnd);
        });
    }

    private void CreateBambooEffect(Vector3 pos)
    {
        for (int i = 0; i < 3; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(bambooSpr);
            effect.SetScale(Vector3.one * 1.3f);
            effect.SetColorGradient(GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature]);

            pos.z = 0f;

            effect.transform.position = pos;
            effect.transform.rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-90f, 90f), Vector3.forward);

            effect.transform.DOMove(UnityEngine.Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative()
                .OnComplete(() => effect.EndEffect());
        }
    }
}
