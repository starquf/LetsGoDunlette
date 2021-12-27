using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_BoatFare : SkillPiece
{
    BattleHandler battleHandler;
    Vector3 target;
    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");
        battleHandler = GameManager.Instance.battleHandler;
        target = battleHandler.enemy.transform.position;
        target.x -= 0.5f;
        target.y += 0.5f;

        Anim_W_BoatFare boatFaredEffect = PoolManager.GetItem<Anim_W_BoatFare>();
        boatFaredEffect.transform.position = target;

        boatFaredEffect.Play(() => {
            battleHandler.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            if (!CheckSilence())
            {
                GetMoney(onCastEnd);
            }
            else 
            {
                onCastEnd?.Invoke();
            }
        });

    }

    private void GetMoney(Action onCastEnd)
    {
        GameManager.Instance.Gold += 5;
        if (battleHandler.enemy.IsDie)
        {
            Anim_W_BoatFareBonusMoney boatFaredBonusEffect = PoolManager.GetItem<Anim_W_BoatFareBonusMoney>();
            boatFaredBonusEffect.transform.position = target;

            boatFaredBonusEffect.Play(() => {
                GameManager.Instance.Gold += 5;
                onCastEnd?.Invoke();
            });
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}
