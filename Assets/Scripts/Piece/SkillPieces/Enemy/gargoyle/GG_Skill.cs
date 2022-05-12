using System;
using Random = UnityEngine.Random;

public class GG_Skill : SkillPiece
{
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
            onCastSkill = GG_Beat;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = GG_Recover;
            return pieceInfo[1];
        }
    }


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void GG_Beat(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "����").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            target.GetDamage(30, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });



    }

    private void GG_Recover(LivingEntity target, Action onCastEnd = null) //�ڽ��� ü���� 30��ŭ ȸ���Ѵ�.
    {
        SetIndicator(Owner.gameObject, "ȸ��").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Owner.GetComponent<EnemyHealth>().Heal(30);

            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });

    }


}
