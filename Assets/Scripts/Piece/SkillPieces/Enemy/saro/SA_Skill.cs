using System;
using Random = UnityEngine.Random;

public class SA_Skill : SkillPiece
{
    public bool sacrificed = false;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        onCastSkill = SR_CutOff;
        return pieceInfo[0];
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void SR_CutOff(LivingEntity target, Action onCastEnd = null) // �÷��̾�� 30�� ���ظ� ������. //������ ������ ������ �������� 30��ŭ �����Ѵ�.
    {
        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);

            Anim_M_Bite hitEffect = PoolManager.GetItem<Anim_M_Bite>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);
            if(sacrificed)
            {
                target.GetDamage(30 + 30, this, owner);
            }
            else
            {
                target.GetDamage(30, this, owner);
            }
            hitEffect.Play(() =>
            {
                SetIndicator(owner.gameObject, "��ó�ο�").OnEnd(() =>
                {
                    target.cc.SetCC(CCType.Wound, 5);
                    onCastEnd?.Invoke();
                });
            });
        });
    }

    private void SR_Sacrifice1(LivingEntity target, Action onCastEnd = null) //������ ������ ������ �������� 30��ŭ �����Ѵ�. 1ȸ��
    {
        SetIndicator(owner.gameObject, "��ų ��ȭ").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            Anim_M_Recover hitEffect = PoolManager.GetItem<Anim_M_Recover>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void SR_Sacrifice2(LivingEntity target, Action onCastEnd = null) //����� ������ ��ȣ���� 300 ȹ���Ѵ�.
    {
        SetIndicator(owner.gameObject, "��ȣ��").OnEnd(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.7f, 0.15f);

            Anim_M_Recover hitEffect = PoolManager.GetItem<Anim_M_Recover>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
