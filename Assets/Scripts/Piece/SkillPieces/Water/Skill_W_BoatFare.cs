using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_BoatFare : SkillPiece
{
    public GameObject skillEffectPrefab;
    public GameObject getMoneyEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"Ω∫≈≥ πﬂµø!! ¿Ã∏ß : {PieceName}");

        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        Vector3 target = battleHandler.enemy.transform.position;
        target.x -= 0.5f;
        target.y += 0.5f;

        Anim_W_BoatFare boatFaredEffect = PoolManager.GetItem<Anim_W_BoatFare>();
        boatFaredEffect.transform.position = target;

        boatFaredEffect.Play(() => {
            battleHandler.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            if(battleHandler.enemy.IsDie)
            {
                Anim_W_BoatFare_GetMoney boatFaredGetMoneyEffect = PoolManager.GetItem<Anim_W_BoatFare_GetMoney>();
                boatFaredGetMoneyEffect.transform.position = target;
                boatFaredGetMoneyEffect.Play(() =>
                {
                    print("√ﬂ∞° ∞ÒµÂ »πµÊ");
                    onCastEnd?.Invoke();
                });
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });

    }
}
