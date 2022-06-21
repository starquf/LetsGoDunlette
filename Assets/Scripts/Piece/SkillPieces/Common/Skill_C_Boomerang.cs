using System;
using System.Collections.Generic;

public class Skill_C_Boomerang : SkillPiece
{
    private int originValue = 0;
    private EventInfo eventInfo;

    protected override void Start()
    {
        base.Start();
        originValue = value;

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
        value++;

        animHandler.GetTextAnim()
               .SetType(TextUpAnimType.Up)
               .SetPosition(skillIconImg.transform.position)
               .SetScale(0.8f)
               .Play("��ȭ!");

        animHandler.GetAnim(AnimName.E_ManaSphereHit)
            .SetScale(0.5f)
            .SetPosition(skillIconImg.transform.position)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
    }

    private void ResetValue(Action action) //������ ������ ���ذ� �ʱ�ȭ
    {
        value = originValue;
        action?.Invoke();
    }
}
