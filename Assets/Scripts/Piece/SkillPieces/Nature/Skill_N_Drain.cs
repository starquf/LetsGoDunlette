using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Drain : SkillPiece
{
    public Sprite drainingEffectSpr;
    private Gradient effectGradient;

    public int healValue = 15;

    protected override void Start()
    {
        base.Start();

        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature];

        isTargeting = true;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");
        desInfos[1].SetInfo(DesIconType.Heal, $"5");
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 50의 피해를 입힌다.		-	회복	체력을 20 회복한다.
    {
        bh.battleUtil.StartCoroutine(Drain(target, onCastEnd));
    }

    private IEnumerator Drain(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc(), currentType);

        animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Up)
        .SetPosition(target.transform.position)
        .Play("흡수!");

        animHandler.GetAnim(AnimName.N_Drain)
                .SetPosition(target.transform.position)
                .SetScale(1f)
                .Play();

        GameManager.Instance.cameraHandler.ShakeCamera(2f, 0.3f);

        yield return new WaitForSeconds(0.1f);

        LivingEntity healTarget = Owner.GetComponent<LivingEntity>();

        const float time = 0.8f;
        int rand = Random.Range(7, 13);
        int healAmount = 0;
        for (int i = 0; i < rand; i++)
        {
            int a = i;
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = target.transform.position;
            effect.SetSprite(drainingEffectSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one * 0.5f);

            effect.Play(healTarget.transform.position, () =>
            {
                if (a == rand - 1)
                {
                    healTarget.Heal(healValue);
                    onCastEnd?.Invoke();
                }
                effect.EndEffect();
            }, BezierType.Quadratic, isRotate: true, playSpeed: 1.5f);
            yield return new WaitForSeconds(time / rand);
        }
    }
}
