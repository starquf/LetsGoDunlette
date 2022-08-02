using System;
using System.Collections.Generic;
using TMPro;

public class Skill_F_Heating : SkillPiece
{
    public TextMeshProUGUI counterText;
    private int turnCount = 3;

    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;

        bh.battleEvent.BookEvent(new NormalEvent((action) =>
        {
            ResetCount();
            action?.Invoke();
        }, EventTime.EndBattle));
    }

    //3번 사용 시 '플래시 오버' 조각으로 변경된다.
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        target.GetDamage(GetDamageCalc(Value), currentType);

        HighlightColor(0.2f);

        turnCount--;
        counterText.text = turnCount.ToString();
        if (turnCount <= 0)
        {
            //bh.mainRullet.PutRulletPieceToGraveYard(pieceIdx);
            GameManager.Instance.inventoryHandler.RemovePiece(this);
        }
        else
        {
            animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Fixed)
            .SetPosition(skillIconImg.transform.position)
            .SetScale(0.8f)
            .Play($"{turnCount}번 후 플래시 오버");
        }

        onCastEnd?.Invoke();
    }

    public override void OnRullet()
    {
        if (bh == null)
        {
            bh = GameManager.Instance.battleHandler;
        }

        counterText.text = turnCount.ToString();
    }

    private void ResetCount() //초기화
    {
        turnCount = 3;
        counterText.text = turnCount.ToString();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(Value)}");
        return desInfos;
    }
}
