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
                    animHandler.GetAnim(AnimName.E_ManaSphereHit)
                    .SetPosition(targetPos)
                    .Play();

                    if (Random.Range(0, 100) < percentage)
                    {
                        animHandler.GetAnim(AnimName.E_Static_Stun)
                        .SetPosition(targetPos)
                        .Play();

                        target.cc.SetCC(CCType.Stun, 1);

                        LogCon log = new LogCon();
                        log.text = $"������Ŵ";
                        log.selfSpr = skillImg.sprite;
                        log.targetSpr = target.GetComponent<SpriteRenderer>().sprite;

                        DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

                        animHandler.GetTextAnim()
                        .SetType(TextUpAnimType.Up)
                        .SetScale(0.7f)
                        .SetPosition(target.transform.position)
                        .Play("���� Ȯ�� ����!");

                        log = new LogCon();
                        log.text = $"Ȯ�� ����";
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
                    log2.text = $"{GetDamageCalc()} ������ �ο�";
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
