using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_BoatFare : SkillPiece
{
    public int getMoney = 10;
    public int getBonusMoney = 5;

    BattleHandler battleHandler;
    Vector3 targetPos;

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        //print($"스킬 발동!! 이름 : {PieceName}");
        battleHandler = GameManager.Instance.battleHandler;
        targetPos = target.transform.position;
        targetPos.x -= 0.5f;
        targetPos.y += 0.5f;

        Anim_W_BoatFare boatFaredEffect = PoolManager.GetItem<Anim_W_BoatFare>();
        boatFaredEffect.transform.position = targetPos;

        boatFaredEffect.Play(() => {
            target.GetDamage(Value, patternType);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

            GetMoney(onCastEnd);
        });

    }

    private void GetMoney(Action onCastEnd)
    {
        GameManager.Instance.Gold += getMoney;
        if (!(battleHandler.enemys.Count > 0))
        {
            Anim_W_BoatFareBonusMoney boatFaredBonusEffect = PoolManager.GetItem<Anim_W_BoatFareBonusMoney>();
            boatFaredBonusEffect.transform.position = targetPos;

            boatFaredBonusEffect.Play(() => {
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
