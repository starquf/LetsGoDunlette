using System;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    public string skillName;

    public PlayerSkillType skillType;
    public Sprite icon;
    public bool canUse;

    protected PlayerSkillButton ui;

    public virtual void Init(PlayerSkillButton ui)
    {
        this.ui = ui;
    }

    public abstract bool CanUseSkill();

    public virtual void Cast(Action onEndSkill)
    {
        canUse = false;
        GameManager.Instance.battleHandler.playerInfoHandler.UpdateCanPlayerSkillUse();
    }

    public abstract void UpdateUI(PlayerSkillButton skillBtn);

    public abstract void OnBattleStart();
}
