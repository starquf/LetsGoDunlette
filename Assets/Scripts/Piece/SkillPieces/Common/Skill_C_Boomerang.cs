using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Skill_C_Boomerang : SkillPiece
{
    public Sprite coinSpr;
    private Gradient effectGradient;

    public int originValue = -2;
    private EventInfo eventInfo;
    public Text counterText;

    protected override void Start()
    {
        base.Start();
        effectGradient = GameManager.Instance.inventoryHandler.effectGradDic[ElementalType.None];
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        return desInfos;
    }

    public override void OnRullet()
    {
        base.OnRullet();
        counterText.text = GetDamageCalc().ToString();
        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(eventInfo);
        eventInfo = new NormalEvent(new Action<Action>(ResetValue), EventTime.EndBattle);
        GameManager.Instance.battleHandler.battleEvent.BookEvent(eventInfo);
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Vector3 targetPos = target.transform.position;
        Vector3 startPos = Owner.transform.position;


        EffectObj skillEffect = PoolManager.GetItem<EffectObj>();
        skillEffect.transform.position = startPos;
        skillEffect.SetSprite(coinSpr);
        skillEffect.SetColorGradient(effectGradient);
        skillEffect.SetScale(Vector3.one * 1.5f);


        skillEffect.Play(targetPos, () =>
        {
            animHandler.GetAnim(AnimName.C_ManaSphereHit)
            .SetPosition(targetPos)
            .SetScale(0.7f)
            .Play();

            target.GetDamage(GetDamageCalc());

            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);


            startPos.x = 0;

            EffectObj skillEffect1 = PoolManager.GetItem<EffectObj>();
            skillEffect1.transform.position = targetPos;
            skillEffect1.SetSprite(coinSpr);
            skillEffect1.SetColorGradient(effectGradient);
            skillEffect1.SetScale(Vector3.one * 1.5f );

            skillEffect1.Play(startPos, () =>
            {
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);




                var skills = GameManager.Instance.inventoryHandler.skills;
                int updateValue = Value + 1;
                foreach (var item in skills)
                {
                    Skill_C_Boomerang skill_C_Boomerang = item.GetComponent<Skill_C_Boomerang>();
                    if (skill_C_Boomerang != null)
                    {
                        skill_C_Boomerang.UpdateValue(updateValue);
                        if (skill_C_Boomerang.IsInRullet)
                        {
                            animHandler.GetTextAnim()
                            .SetType(TextUpAnimType.Up)
                            .SetPosition(skill_C_Boomerang.skillIconImg.transform.position)
                            .SetScale(0.8f)
                            .Play("강화!");
                        }
                    }
                }

                animHandler.GetAnim(AnimName.C_ManaSphereHit)
                .SetPosition(startPos)
                .SetScale(0.5f)
                .Play(() => onCastEnd?.Invoke());

                skillEffect1.EndEffect();
            }, BezierType.Cubic, isRotate: true, playSpeed: 2f);

            skillEffect.EndEffect();
        }, BezierType.Cubic, isRotate: true, playSpeed: 2f);
    }

    private void ResetValue(Action action) //전투가 끝나면 피해가 초기화
    {
        var skills = GameManager.Instance.inventoryHandler.skills;
        foreach (var item in skills)
        {
            Skill_C_Boomerang skill_C_Boomerang = item.GetComponent<Skill_C_Boomerang>();
            if (skill_C_Boomerang != null)
            {
                skill_C_Boomerang.UpdateValue(originValue);
            }
        }
        action?.Invoke();
    }

    public void UpdateValue(int value)
    {
        this.value = value;
        counterText.text = GetDamageCalc().ToString();
    }
}
