using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanelHandler : BottomSwapUI
{
    public TextMeshProUGUI atkTxt;
    public TextMeshProUGUI hpTxt;
    public TextMeshProUGUI maxPieceCountTxt;
    public TextMeshProUGUI playerLevelTxt;

    public TextMeshProUGUI expTxt;
    public Image expFillImage;

    private BattleHandler bh;
    private PlayerInfo playerInfo;
    private PlayerHealth player;
    private List<PlayerSkillButton> skillButtons = new List<PlayerSkillButton>();
    public Transform skillBtnTrans;
    public Image skillAbleIcon;

    public bool hasCanUseSkill = false;
    public bool isCasting = false;
    private bool canCast = true;

    protected override void Awake()
    {
        base.Awake();
        skillBtnTrans.GetComponentsInChildren(skillButtons);
    }

    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
        player = GameManager.Instance.battleHandler.player;
    }

    protected override void SetCGEnable(bool enable)
    {
        skillAbleIcon.gameObject.SetActive(!enable && hasCanUseSkill);
        base.SetCGEnable(enable);
        Synchronization();
    }

    public void Synchronization()
    {
        if (player == null)
        {
            player = GameManager.Instance.battleHandler.player;
        }

        playerLevelTxt.text = $"{player.PlayerLevel}";
        atkTxt.text = $"{player.AttackPower}";
        hpTxt.text = $"{player.maxHp}";
        maxPieceCountTxt.text = $"{player.MaxPieceCount}";

        expTxt.text = $"{player.CurrentExp}/{player.MaxExp}";
        expFillImage.fillAmount = player.CurrentExp / (float)player.MaxExp;
    }

    public void Init(PlayerInfo playerInfo)
    {
        this.playerInfo = playerInfo;

        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < playerInfo.playerSkills.Count; i++)
        {
            PlayerSkill skill = playerInfo.playerSkills[i];

            skillButtons[i].gameObject.SetActive(true);
            skillButtons[i].Init(skill, ps =>
            {
                print($"눌린 스킬 : {ps.skillName}");

                if (!canCast)
                {
                    return;
                }

                if (!bh.isBattle || bh.mainRullet.IsStop)
                {
                    return;
                }

                if (!isCasting)
                {
                    UseSkill(ps);
                }
            });
        }
    }

    public void OnBattleStart()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (skillButtons[i].currentSkill != null)
            {
                skillButtons[i].currentSkill.OnBattleStart();
            }
        }
    }

    public void UpdateCanPlayerSkillUse()
    {
        hasCanUseSkill = false;
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (skillButtons[i].currentSkill != null)
            {
                PlayerSkill ps = skillButtons[i].currentSkill;
                if ((ps.skillType.Equals(PlayerSkillType.Active_Cooldown) || ps.skillType.Equals(PlayerSkillType.Active_Count)) && ps.canUse)
                {
                    hasCanUseSkill = true;
                    break;
                }
            }
        }
        skillAbleIcon.gameObject.SetActive(hasCanUseSkill && !isShow);
    }

    private void UseSkill(PlayerSkill skill)
    {
        // 스킬이 사용 가능한지 체크
        if (skill.CanUseSkill())
        {
            print("사용됨!!");

            bh.SetInteract(false);
            isCasting = true;

            skill.Cast(() =>
            {
                bh.StartTurn();

                isCasting = false;
            });

            //ClosePanel();
        }
    }

    public void SetInteract(bool enable)
    {
        canCast = enable;
    }
}
