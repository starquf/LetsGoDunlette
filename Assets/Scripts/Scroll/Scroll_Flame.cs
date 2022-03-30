using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Flame : Scroll
{
    private BattleHandler bh;
    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        //scrollType = ScrollType.Flame;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        var enemyList = bh.enemys;
        int damage = 10; // юс╫ц
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetDamage(damage);
        }

         Anim_F_ChainExplosion recoverEffect = PoolManager.GetItem<Anim_F_ChainExplosion>();
        recoverEffect.transform.position = bh.playerImgTrans.position;

        recoverEffect.Play(() =>
        {
            onEndUse?.Invoke();
        });
    }
}
