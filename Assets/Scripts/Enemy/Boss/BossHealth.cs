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
