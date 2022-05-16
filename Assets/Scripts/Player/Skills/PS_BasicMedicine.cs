using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_BasicMedicine : PlayerSkill
{
    private BattleHandler bh;

    public int healValue = 10;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        bh.battleEvent.BookEvent(new NormalEvent(action =>
        {
            if (bh.battleUtil.CheckEnemyDie(bh.enemys))
            {
                bh.player.Heal(healValue);
                GameManager.Instance.animHandler.GetAnim(AnimName.M_Recover).SetPosition(bh.player.transform.position)
                .SetScale(1)
                .Play();
            }
            action?.Invoke();

        }, EventTime.EndOfTurn));
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
        //cooldown = 0;

        //ui.UpdateUI();
    }
}
