using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class HD_Poison : SkillPiece
{
    private Action<SkillPiece, Action> onNextTurn = null;
    public Text counterText;
    private int turnCount = 3;
    private SkillEvent eventInfo = null;

    protected override void Start()
    {
        base.Start();
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Wound, $"{Value}");

        return desInfos;
    }

    public override void OnRullet()
    {
        if (bh == null)
        {
            bh = GameManager.Instance.battleHandler;            
        }

        bh.battleEvent.RemoveEventInfo(eventInfo);

        onNextTurn = (piece, action) =>
        {
            if (piece != this)
            {
                HighlightColor(0.2f);

                turnCount--;
                counterText.text = turnCount.ToString();
                if (turnCount <= 0)
                {
                    action?.Invoke();

                    bh.mainRullet.PutRulletPieceToGraveYard(pieceIdx);
                }
                else
                {
                    animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Fixed)
                    .SetPosition(skillIconImg.transform.position)
                    .SetScale(0.8f)
                    .Play($"{turnCount}턴 후 삭제");
                }
            }

            action?.Invoke();
        };

        turnCount = 3;
        counterText.text = turnCount.ToString();

        eventInfo = new SkillEvent(EventTimeSkill.AfterSkill, onNextTurn);

        bh.battleEvent.BookEvent(eventInfo);
    }

    public override void ResetPiece()
    {
        base.ResetPiece();

        if (bh == null)
        {
            bh = GameManager.Instance.battleHandler;
        }

        bh.battleEvent.RemoveEventInfo(eventInfo);

        turnCount = 3;
        counterText.text = turnCount.ToString();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        bh.battleEvent.RemoveEventInfo(eventInfo);

        SetIndicator(Owner.gameObject, "상처 부여").OnEndAction(() =>
        {
            target.cc.SetCC(CCType.Wound, Value, true);

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            animHandler.GetAnim(AnimName.N_PoisionCloud)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}
