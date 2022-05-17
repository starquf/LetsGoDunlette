using System;

public class PS_BasicMedicine : PlayerSkill
{
    private BattleHandler bh;

    public int healValue = 10;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
    }

    public override void Cast(Action onEndSkill)
    {
        //cooldown = maxCooldown;

        //ui.UpdateUI();
    }

    public override void UpdateUI(PlayerSkillButton skillBtn)
    {

    }

    public override bool CanUseSkill()
    {
        return true;
    }

    public override void OnBattleStart()
    {

        bh.battleEvent.BookEvent(new NormalEvent(action =>
        {
            if (bh.battleUtil.CheckEnemyDie(bh.enemys))
            {
                bh.player.Heal(healValue);
                GameManager.Instance.animHandler.GetAnim(AnimName.M_Recover).SetPosition(bh.player.transform.position)
                .SetScale(1)
                .Play();
                GameManager.Instance.animHandler.GetTextAnim().SetPosition(bh.player.transform.position)
                .SetScale(1.3f)
                .SetType(TextUpAnimType.Fixed)
                .Play("기초의학");
            }
            action?.Invoke();

        }, EventTime.EndOfTurn));

        //ui.UpdateUI();
    }
}
