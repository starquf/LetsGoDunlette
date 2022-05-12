using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_F_TickTock : SkillPiece
{
    public int hitedValue = 5;
    private Action<SkillPiece,Action> onNextTurn = null;
    public Text counterText;
    private int turnCount = 3;

    private BattleHandler bh = null;

    SkillEvent eventInfo = null;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds pTwoSecWait = new WaitForSeconds(0.2f);

    private bool onReset = false;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
        isTargeting = true;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc().ToString()}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.9f);

        return attack;
    }

    public override void OnRullet()
    {
        onReset = false;

        bh = GameManager.Instance.battleHandler;

        bh.battleEvent.RemoveEventInfo(eventInfo);

        onNextTurn = (piece,action) =>
        {
            if (piece != this)
            {
                animHandler.GetTextAnim()
                .SetType(TextUpAnimType.Fixed)
                .SetPosition(skillImg.transform.position)
                .SetScale(0.8f)
                .Play("째깍째깍!");

                animHandler.GetAnim(AnimName.F_ChainExplosion)
                .SetPosition(skillImg.transform.position)
                .SetScale(0.5f)
                .Play();

                HighlightColor(0.2f);

                turnCount--;
                counterText.text = turnCount.ToString();
                if (turnCount <= 0)
                {
                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.25f, 0.1f);
                    owner.GetComponent<LivingEntity>().GetDamage(hitedValue);

                    animHandler.GetAnim(AnimName.F_ManaSphereHit)
                    .SetPosition(bh.playerImgTrans.position)
                    .SetScale(Random.Range(0.8f, 1f))
                    .Play();

                    action?.Invoke();
                    bh.mainRullet.PutRulletPieceToGraveYard(pieceIdx);
                }
            }

            action?.Invoke();
        };
        turnCount = 3;
        eventInfo = new SkillEvent(EventTimeSkill.AfterSkill, onNextTurn);
        bh.battleEvent.BookEvent(eventInfo);
    }

    public override void ResetPiece()
    {
        base.ResetPiece();

        bh = GameManager.Instance.battleHandler;
        bh.battleEvent.RemoveEventInfo(eventInfo);

        turnCount = 3;
        counterText.text = turnCount.ToString();
    }
    
    public override void Cast(LivingEntity target, Action onCastEnd = null) //룰렛에 들어온 뒤 사용되지 않은채로 3턴이 지나면 자신에게 60의 데미지를 준 뒤 무덤으로 이동한다.
    {
        target.GetDamage(GetDamageCalc(), currentType);

        GameManager.Instance.cameraHandler.ShakeCamera(3.5f, 0.2f);

        animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(target.transform.position)
                .SetScale(Random.Range(0.8f, 1f))
                .Play();

        animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(target.transform.position)
                .SetScale(Random.Range(0.6f, 0.7f))
                .Play(() =>
                {
                    onCastEnd?.Invoke();
                });
    }
}
