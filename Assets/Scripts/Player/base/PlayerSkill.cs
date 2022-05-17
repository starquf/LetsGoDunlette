using System;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    public string skillName;

    public PlayerSkillType skillType;
    public Sprite icon;

    protected PlayerSkillButton ui;

    public virtual void Init(PlayerSkillButton ui)
    {
        this.ui = ui;
    }

    public abstract bool CanUseSkill();

    public abstract void Cast(Action onEndSkill);

    public abstract void UpdateUI(PlayerSkillButton skillBtn);

    public abstract void OnBattleStart();
}
