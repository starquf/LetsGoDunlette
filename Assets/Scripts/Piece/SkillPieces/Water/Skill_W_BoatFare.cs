using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_BoatFare : SkillPiece
{
    BattleHandler battleHandler;
    Vector3 targetPos;

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        print($"스킬 발동!! 이름 : {PieceName}");
        battleHandler = GameManager.Instance.battleHandler;
        targetPos = target.transform.position;
        targetPos.x -= 0.5f;
        targetPos.y += 0.5f;

        Anim_W_BoatFare boatFaredEffect = PoolManager.GetItem<Anim_W_BoatFare>();
        boatFaredEffect.transform.position = targetPos;

        boatFaredEffect.Play(() => {
            target.GetDamage(Value);
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
        if (battleHandler.enemys[0].IsDie)
        {
            Anim_W_BoatFareBonusMoney boatFaredBonusEffect = PoolManager.GetItem<Anim_W_BoatFareBonusMoney>();
            boatFaredBonusEffect.transform.position = targetPos;

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
