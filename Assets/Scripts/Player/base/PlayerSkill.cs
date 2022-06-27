using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerSkill : MonoBehaviour
{
    protected Image icon = null;

    [Header("Á¤º¸µé")]
    public string skillName;

    [TextArea]
    public string skillDes;

    public PlayerSkillType skillType;
    public PlayerSkillName skillNameType;
    public Sprite iconSpr;

    [HideInInspector]
    public bool canUse;
    public bool isUniqueSkill = false;

    protected PlayerSkillButton ui;

    protected virtual void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();

        SetIcon(iconSpr);
    }

    protected virtual void Start()
    {

    }

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

        ui.SkillUseEffect();
    }

    public abstract void UpdateUI(PlayerSkillButton skillBtn);

    public abstract void OnBattleStart();
}
