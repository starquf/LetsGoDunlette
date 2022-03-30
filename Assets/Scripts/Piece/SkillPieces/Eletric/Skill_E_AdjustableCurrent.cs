using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_E_AdjustableCurrent : SkillPiece
{
    [SerializeField]
    private int percentage = 20;
    private int stack = 1;

    public Text percentText;

    public List<Sprite> effectSprList = new List<Sprite>();

    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[PatternType.Diamonds];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 startPos = skillImg.transform.position;
        Vector3 targetPos = target.transform.position;

        for (int i = 0; i < effectSprList.Count; i++)
        {
            EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
            skillEffect.transform.position = startPos;
            skillEffect.SetSprite(effectSprList[i]);
            skillEffect.SetColorGradient(effectGradient);
            skillEffect.SetScale(Vector3.one);

            if (i == 0)
            {
                skillEffect.Play(targetPos, () =>
                {
                    Anim_E_ManaSphereHit hitEffect = PoolManager.GetItem<Anim_E_ManaSphereHit>();
                    hitEffect.transform.position = targetPos;
                    hitEffect.Play();

                    if (Random.Range(0, 100) < percentage)
                    {
                        Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                        stunEffect.transform.position = targetPos;

                        stunEffect.Play();

                        target.cc.SetCC(CCType.Stun, 1);

                        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                        textEffect.SetType(TextUpAnimType.Damage);
                        textEffect.transform.position = target.transform.position;
                        textEffect.SetScale(0.7f);
                        textEffect.Play("기절 확률 증가!");

                        percentage += 10;
                        stack += 1;
                        percentText.text = stack.ToString();

                        if (percentage > 100)
                            percentage = 100;
                    }

                    target.GetDamage(Value, currentType);

                    GameManager.Instance.cameraHandler.ShakeCamera(0.7f, 0.15f);

                    onCastEnd?.Invoke();

                    skillEffect.EndEffect();
                }, BezierType.Linear, isRotate: true, playSpeed: 6f, delay: i * 0.07f);
            }
            else
            {
                skillEffect.Play(targetPos, () => 
                {
                    skillEffect.EndEffect();
                }, BezierType.Linear, isRotate: true, playSpeed: 6f, delay: i * 0.07f);
            }
        }
    }
}
