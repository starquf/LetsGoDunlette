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
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            target.GetDamage(10, this, owner);

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });

            //��� ��ġ�� ��ƾ
            stolenGolds += GameManager.Instance.TryStillGold(10);
        });
    }

    private void KB_Transform(LivingEntity target, Action onCastEnd = null) //�̹� �������� �÷��̾�Լ� ��ģ ��常ŭ �������� �ش�.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
            effect.transform.position = GameManager.Instance.enemyEffectTrm.position; effect.SetScale(2);

            target.GetDamage(stolenGolds, this, owner);

            effect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
