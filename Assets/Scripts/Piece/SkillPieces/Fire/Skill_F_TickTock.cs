using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_F_TickTock : SkillPiece
{
    public int hitedValue = 40;
    private Action<SkillPiece,Action> onNextTurn = null;
    public Text counterText;
    private int turnCount = 3;

    private BattleHandler bh = null;

    SkillEvent eventInfo = null;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);
    private readonly WaitForSeconds pTwoSecWait = new WaitForSeconds(0.2f);

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
        hasTarget = true;
    }   

    public override void OnRullet()
    {
        bh = GameManager.Instance.battleHandler;

        bh.battleEvent.RemoveEventInfo(eventInfo);

        onNextTurn = (piece,action) =>
        {
            if (piece != this)
            {
                Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                textEffect.SetType(TextUpAnimType.Fixed);
                textEffect.transform.position = skillImg.transform.position;
                textEffect.SetScale(0.8f);
                textEffect.Play("째깍째깍!");

                Anim_F_ChainExplosion skillEffect = PoolManager.GetItem<Anim_F_ChainExplosion>();
                skillEffect.transform.position = skillImg.transform.position;
                skillEffect.SetScale(0.5f);
                skillEffect.Play();

                HighlightColor(0.2f);

                turnCount--;
                counterText.text = turnCount.ToString();
                if (turnCount <= 0)
                {
                    bh.battleEvent.RemoveEventInfo(eventInfo);

                    Anim_F_ManaSphereHit effect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
                    effect.transform.position = bh.playerImgTrans.position;
                    effect.SetScale(Random.Range(0.8f, 1f));

                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.25f, 0.1f);
                    owner.GetComponent<LivingEntity>().GetDamage(hitedValue);
                    effect.Play();
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
        target.GetDamage(value, currentType);

        GameManager.Instance.cameraHandler.ShakeCamera(3.5f, 0.2f);

        Anim_F_ManaSphereHit effect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
        effect.transform.position = target.transform.position;
        effect.SetScale(Random.Range(0.8f, 1f));

        effect.Play();

        Anim_F_ManaSphereHit skillEffect = PoolManager.GetItem<Anim_F_ManaSphereHit>();
        skillEffect.transform.position = target.transform.position;
        skillEffect.SetScale(Random.Range(0.6f, 0.7f));

        skillEffect.Play(() =>
        {
            onCastEnd?.Invoke();
        });
    }

}
