using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GB_Skill : SkillPiece
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
            desInfos[0].SetInfo(DesIconType.Stun, $"{pieceInfo[0].GetValue()}%");

            onCastSkill = GB_Flash;
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = GB_Shooting;

            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");
            return pieceInfo[1];
        }
    }
    public override List<DesIconInfo> GetDesIconInfo()
    {
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void GB_Flash(LivingEntity target, Action onCastEnd = null) //����	100% �÷��̾�� 3�ϰ� ���ӵǴ� ǥ���� �����.
    {
        SetIndicator(Owner.gameObject, "����ź").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            target.cc.SetCC(CCType.signOfGoblinGunman, pieceInfo[0].GetValue() + 1, true);
            
            target.cc.SetCC(CCType.Stun, 1, true);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void GB_Shooting(LivingEntity target, Action onCastEnd = null) //calcDamage	-1 �÷��̾�� ǥ���� �ִٸ� �߰� ���ظ� (<sprite=4>3)�ش�.
    {
        SetIndicator(Owner.gameObject, "���").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            int damage = GetDamageCalc(pieceInfo[1].GetValue());
            if (target.cc.IsCC(CCType.signOfGoblinGunman))
            {
                damage += 3;
            }

            target.GetDamage(damage, this, Owner);

            animHandler.GetAnim(AnimName.M_Sword).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
