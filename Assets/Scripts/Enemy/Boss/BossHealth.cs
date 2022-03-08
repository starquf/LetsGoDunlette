using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    protected override void Start()
    {
        base.Start();

        isBoss = true;
    }

    protected override void Die()
    {
        base.Die();

        bh.BattleForceEnd();
    }
}
