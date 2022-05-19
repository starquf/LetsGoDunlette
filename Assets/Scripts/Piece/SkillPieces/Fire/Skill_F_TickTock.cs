using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_F_TickTock : SkillPiece
{
    public int hitedValue = 5;
    private Action<SkillPiece, Action> onNextTurn = null;
    public Text counterText;
    private int turnCount = 3;
    private SkillEvent eventInfo = null;
    protected override void Start()
    {
        base.Start();
        isTargeting = true;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc()}");

        return desInfos;
    }

    public override void OnRullet()
    {
        bh.battleEvent.RemoveEventInfo(eventInfo);

        onNextTurn = (piece, action) =>
        {
            if (piece != this)
            {
                animHandler.GetTextAnim()
                .SetType(TextUpAnimType.Fixed)
                .SetPosition(skillIconImg.transform.position)
                .SetScale(0.8f)
                .Play("°��°��!");

                animHandler.GetAnim(AnimName.F_ChainExplosion)
                .SetPosition(skillIconImg.transform.position)
                .SetScale(0.5f)
                .Play();

                HighlightColor(0.2f);

                turnCount--;
                counterText.text = turnCount.ToString();
                if (turnCount <= 0)
                {
                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.25f, 0.1f);
                    Owner.GetComponent<LivingEntity>().GetDamage(hitedValue);

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
        if (bh == null)
        {
            bh = GameManager.Instance.battleHandler;
        }
        bh.battleEvent.RemoveEventInfo(eventInfo);

        turnCount = 3;
        counterText.text = turnCount.ToString();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //�귿�� ���� �� ������ ����ä�� 3���� ������ �ڽſ��� 60�� �������� �� �� �������� �̵��Ѵ�.
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
