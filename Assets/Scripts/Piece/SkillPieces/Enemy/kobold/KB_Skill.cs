using System;
using Random = UnityEngine.Random;

public class KB_Skill : SkillPiece
{
    private int stolenGolds = 0;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (Random.Range(0, 100) <= value)
        {
            onCastSkill = KB_Pickpocket;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = KB_Transform;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void KB_Pickpocket(LivingEntity target, Action onCastEnd = null) //�÷��̾�Լ� 10��带 ��ģ��.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(10, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            //��� ��ġ�� ��ƾ
            int stolendGold = GameManager.Instance.TryStillGold(10);
            GameManager.Instance.Gold -= stolendGold;
            stolenGolds += stolendGold;
        });
    }

    private void KB_Transform(LivingEntity target, Action onCastEnd = null) //�̹� �������� �÷��̾�Լ� ��ģ ��常ŭ �������� �ش�.
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.GetDamage(stolenGolds, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
