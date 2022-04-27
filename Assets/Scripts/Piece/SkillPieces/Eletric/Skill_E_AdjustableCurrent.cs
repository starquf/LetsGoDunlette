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

    public Text percentText;

    public List<Sprite> effectSprList = new List<Sprite>();

    private Gradient effectGradient;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.Electric];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        desInfos[1].SetInfo(DesIconType.Stun, $"{percentage.ToString()}%");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.3f - 1);

        return attack;
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

                        LogCon log = new LogCon();
                        log.text = $"기절시킴";
                        log.selfSpr = skillImg.sprite;
                        log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

                        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

                        Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                        textEffect.SetType(TextUpAnimType.Up);
                        textEffect.transform.position = target.transform.position;
                        textEffect.SetScale(0.7f);
                        textEffect.Play("기절 확률 증가!");

                        log = new LogCon();
                        log.text = $"확률 증가";
                        log.selfSpr = skillImg.sprite;

                        DebugLogHandler.AddLog(LogType.ImageText, log);

                        percentage += 10;

                        if (percentage > 100)
                        {
                            percentage = 100;
                        }

                        percentText.text = $"{percentage.ToString()}%";
                    }

                    int damage = GetDamageCalc();

                    target.GetDamage(GetDamageCalc(), currentType);

                    LogCon log2 = new LogCon();
                    log2.text = $"{GetDamageCalc()} 데미지 부여";
                    log2.selfSpr = skillImg.sprite;
                    log2.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

                    DebugLogHandler.AddLog(LogType.ImgTextToTarget, log2);

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
