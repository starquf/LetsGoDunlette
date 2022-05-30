using System;

public class PS_Test : PlayerSkill
{
    private BattleHandler bh;

    public int maxCooldown = 5;
    private int cooldown = 0;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        cooldown = maxCooldown;
    }

    public override void Cast(Action onEndSkill)
    {
        base.Cast(onEndSkill);
        bh.mainRullet.PauseRullet();

        StartCoroutine(bh.battleUtil.ResetRullet(() =>
        {
            StartCoroutine(bh.CheckPanelty(onEndSkill));
        }));

        cooldown = maxCooldown;

        ui.UpdateUI();
    }

    public override void UpdateUI(PlayerSkillButton skillBtn)
    {
        skillBtn.SetCoolDown(cooldown / (float)maxCooldown);

        if (canUse)
        {
            skillBtn.SetMessege("��� ����");
        }
        else
        {
            skillBtn.SetMessege($"{cooldown}�� ����");
        }
    }

    public override bool CanUseSkill()
    {
        bool canUse = false;

        canUse = cooldown <= 0;

        return canUse;
    }

    public override void OnBattleStart()
    {
        cooldown = 0;

        bh.battleEvent.BookEvent(new NormalEvent(action =>
        {
            if (cooldown > 0)
            {
                cooldown--;
            }
            canUse = cooldown == 0;

            ui.UpdateUI();

            action?.Invoke();
        }, EventTime.StartTurn));

        ui.UpdateUI();
    }
}
