using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class Skill_C_Boomerang : SkillPiece
{
    public int originValue = -2;
    private EventInfo eventInfo;
    public Text counterText;

    protected override void Start()
    {
        base.Start();

        counterText.text = GetDamageCalc().ToString();
        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(eventInfo);
        eventInfo = new NormalEvent(new Action<Action>(ResetValue), EventTime.EndBattle);
        GameManager.Instance.battleHandler.battleEvent.BookEvent(eventInfo);
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc());

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

        onCastEnd?.Invoke();
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
