using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillPanelHandler : MonoBehaviour
{
    private BattleHandler bh;
    private List<PlayerSkillButton> skillButtons = new List<PlayerSkillButton>();
    public Transform skillBtnTrans;

    public bool hasCanUseSkill = false;
    public bool isCasting = false;
    private bool canCast = true;

    public ItemDesUIHandler itemDesHandler;

    private void Awake()
    {
        skillBtnTrans.GetComponentsInChildren(skillButtons);
    }

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        SetSkill(skillButtons[1], PoolManager.GetPlayerSkill(PlayerSkillName.FirstAid));
        SetSkill(skillButtons[2], PoolManager.GetPlayerSkill(PlayerSkillName.FirstAid));
    }

    public void Init(PlayerInfo playerInfo)
    {
        for (int i = 0; i < playerInfo.playerUniqueSkills.Count; i++)
        {
            PlayerSkill skill = Instantiate(playerInfo.playerUniqueSkills[i], skillButtons[i].btnPos);

            skillButtons[i].gameObject.SetActive(true);

            print("넣어짐");
            SetSkill(skillButtons[i], skill);
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
    }

    private void UseSkill(PlayerSkill skill)
    {
        // 스킬이 사용 가능한지 체크
        if (skill.CanUseSkill())
        {
            print("사용됨!!");

            itemDesHandler.ShowDes(skill.skillName, skill.skillDes, skill.iconSpr, "사용",
                () =>
                {
                    Time.timeScale = 1f;

                    isCasting = true;

                    skill.Cast(() =>
                    {
                        bh.StartTurn();

                        isCasting = false;
                    });
                },

                () =>
                {
                    Time.timeScale = 1f;
                    bh.SetInteract(true);
                });

            Time.timeScale = 0f;

            bh.SetInteract(false);

            //ClosePanel();
        }
    }

    private void SetSkill(PlayerSkillButton btn, PlayerSkill skill)
    {
        btn.Init(skill, ps =>
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

    public void SetInteract(bool enable)
    {
        canCast = enable;
    }
}
