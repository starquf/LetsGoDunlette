using System;

public class SC_Hiding : Scroll
{
    public override void Start()
    {
        base.Start();
        scrollType = ScrollType.Hiding;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        if (!bh.isBattle)
        {
            return;
        }

        if (bh.isBoss)
        {
            return;
        }

        if (bh.isElite)
        {
            return;
        }

        bh.BattleForceEnd();
        bh.CheckBattleEnd();
        bh.mainRullet.StopForceRullet();

        //���Ŀ� ����ȳ����� �ؾ���
    }
}
