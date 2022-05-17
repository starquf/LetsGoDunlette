using System;
using UnityEngine;

public class Skill_W_BoatFare : SkillPiece
{
    public int getMoney = 10;
    public int getBonusMoney = 5;
    private Vector3 targetPos;

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");
        targetPos = target.transform.position;
        targetPos.x -= 0.5f;
        targetPos.y += 0.5f;

        animHandler.GetAnim(AnimName.W_BoatFare)
            .SetPosition(targetPos)
            .Play(() =>
            {
                target.GetDamage(Value, currentType);
                GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

                GetMoney(onCastEnd, target);
            });
    }

    private void GetMoney(Action onCastEnd, LivingEntity target)
    {
        GameManager.Instance.Gold += getMoney;
        if (target.IsDie)
        {
            animHandler.GetAnim(AnimName.W_BoatFareBonusMoney)
                .SetPosition(targetPos)
                .Play(() =>
                {
                    GameManager.Instance.Gold += getBonusMoney;
                    onCastEnd?.Invoke();
                });
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}
