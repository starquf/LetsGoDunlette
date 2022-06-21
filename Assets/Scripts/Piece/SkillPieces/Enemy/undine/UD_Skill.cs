using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class UD_Skill : SkillPiece
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
            desInfos[0].SetInfo(DesIconType.Exhausted, $"{pieceInfo[0].GetValue()}");
            usedIcons.Add(DesIconType.Exhausted);

            onCastSkill = UD_Undine_Curse;
            return pieceInfo[0];
        }
        else
        {
            usedIcons.Add(DesIconType.Shield);
            onCastSkill = UD_Water_Drop;
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

    private void UD_Undine_Curse(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "운디네의 저주").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.15f);
            animHandler.GetAnim(AnimName.M_Recover).SetPosition(Owner.transform.position)
            .SetScale(1)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });

            target.cc.SetCC(CCType.Exhausted, pieceInfo[0].GetValue() + 1);
        });
    }

    private void UD_Water_Drop(LivingEntity target, Action onCastEnd = null) //아군에게 보호막 10을 줌
    {
        SetIndicator(Owner.gameObject, "물방울").OnEndAction(() =>
        {
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            List<EnemyHealth> enemys = bh.enemys;

            for (int i = 0; i < enemys.Count; i++)
            {
                EnemyHealth health = enemys[i];
                health.AddShield(10);
                animHandler.GetAnim(AnimName.M_Shield).SetPosition(health.transform.position)
                .SetScale(1)
                .Play();
            }

            animHandler.GetAnim(AnimName.M_Butt)
            .SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2f)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }


}
