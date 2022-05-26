using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WD_S_Water_Pressure : SkillPiece
{
    public List<Sprite> effectSprList = new List<Sprite>();

    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Water];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "АјАн").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(Value), this);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            for (int i = 0; i < effectSprList.Count; i++)
            {
                int cnt = effectSprList.Count;

                EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
                skillEffect.transform.position = Owner.transform.position + Vector3.one * 0.4f;
                skillEffect.SetSprite(effectSprList[i]);
                skillEffect.SetColorGradient(effectGradient);
                skillEffect.SetScale(Vector3.one);

                // target.transform.position + (Vector3.right * (-0.5f * (cnt / 2f)) + (Vector3.right * i)
                skillEffect.Play(target.transform.position + (Vector3.right * (0.7f * Mathf.Cos(i * 0.5f))), () =>
                {
                    skillEffect.EndEffect();
                }, BezierType.Linear, isRotate: false, playSpeed: 4f, delay: i * 0.04f);


                EffectObj skillEffect2 = PoolManager.GetItem<EffectObj>();
                skillEffect2.transform.position = Owner.transform.position + (Vector3.right * -0.4f) + (Vector3.up * 0.35f);
                skillEffect2.SetSprite(effectSprList[i]);
                skillEffect2.SetColorGradient(effectGradient);
                skillEffect2.SetScale(Vector3.one);

                skillEffect2.Play(target.transform.position - (Vector3.right * (0.7f * Mathf.Cos(i * 0.5f))), () =>
                {
                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(1f, 0.03f);
                    skillEffect2.EndEffect();
                }, BezierType.Linear, isRotate: false, playSpeed: 4f, delay: i * 0.04f);
            }

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
