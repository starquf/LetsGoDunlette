using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerSkill : MonoBehaviour
{
    [Header("Á¤º¸µé")]
    [SerializeField]
    protected Image icon = null;

    public string skillName;

    [TextArea]
    public string skillDes;

    public PlayerSkillType skillType;
    public Sprite iconSpr;

    public bool canUse;

    public bool isUniqueSkill = false;

    protected PlayerSkillButton ui;

    public virtual void Init(PlayerSkillButton ui)
    {
        this.ui = ui;

        UpdateUI(ui);
    }

    public void SetIcon(Sprite img)
    {
        icon.sprite = img;
    }

    public abstract bool CanUseSkill();

    public virtual void Cast(Action onEndSkill)
    {
        canUse = false;
    }

    public abstract void UpdateUI(PlayerSkillButton skillBtn);

    public abstract void OnBattleStart();
}
