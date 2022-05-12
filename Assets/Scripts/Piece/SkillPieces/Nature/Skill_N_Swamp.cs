using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Swamp : SkillPiece
{
    public Sprite drainingEffectSpr;
    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();

        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Nature];

        isTargeting = true;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //적에게 50의 피해를 입힌다.		-	회복	체력을 20 회복한다.
    {
        StartCoroutine(Drain(target, onCastEnd));
    }

    private IEnumerator Drain(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        target.GetDamage(value);

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

        target.cc.SetCC(CCType.Exhausted, 2);

        Transform playerTrm = bh.playerImgTrans;

        const float time = 0.8f;
        int rand = Random.Range(7, 13);

        for (int i = 0; i < rand; i++)
        {
            int a = i;
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = target.transform.position;
            effect.SetSprite(drainingEffectSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one * 0.5f);

            effect.Play(bh.playerHpbarTrans.position, () =>
            {
                effect.EndEffect();

                animHandler.GetAnim(AnimName.M_Recover).SetPosition(effect.transform.position)
            .SetScale(0.4f)
            .Play();

            }, BezierType.Quadratic, isRotate: true, playSpeed: 1.5f);
            yield return new WaitForSeconds(time / (float)rand);
        }
    }
}
