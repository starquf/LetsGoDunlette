using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_BoatFare : SkillPiece
{
    public GameObject skillEffectPrefab;

    public override void Cast(Action onCastEnd = null)
    {
        base.Cast();
        print($"½ºÅ³ ¹ßµ¿!! ÀÌ¸§ : {PieceName}");

        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        Vector3 target = battleHandler.enemy.transform.position;
        target.x -= 0.5f;
        target.y += 0.5f;

        Anim_W_BoatFare boatFaredEffect = PoolManager.GetItem<Anim_W_BoatFare>();
        boatFaredEffect.transform.position = target;

        boatFaredEffect.Play(() => {
            battleHandler.enemy.GetDamage(Value);
            GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
            if(!CheckSilence())
            {
                GetMoney();
            }
            onCastEnd?.Invoke();
        });

    }

    private void GetMoney()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        if (battleHandler.enemy.IsDie)
        {
            print("Å« °ñµå È¹µæ");
        }
        else
        {
            print("°ñµå È¹µæ");
        }
    }
}
