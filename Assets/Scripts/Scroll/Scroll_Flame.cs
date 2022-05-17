using System;
using System.Collections.Generic;

public class Scroll_Flame : Scroll
{
    public override void Start()
    {
        base.Start();
        //scrollType = ScrollType.Flame;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        List<EnemyHealth> enemyList = bh.enemys;
        int damage = 10; // юс╫ц
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetDamage(damage);
        }

        GameManager.Instance.animHandler.GetAnim(AnimName.F_ChainExplosion)
                .SetPosition(bh.playerImgTrans.position)
                .SetScale(0.5f)
                .Play(() =>
                {
                    onEndUse?.Invoke();
                });
    }
}
