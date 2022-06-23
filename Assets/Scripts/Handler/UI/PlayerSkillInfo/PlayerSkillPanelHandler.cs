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

    private void Awake()
    {
        skillBtnTrans.GetComponentsInChildren(skillButtons);
    }

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
    }

    public void Init(PlayerInfo playerInfo)
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            //skillButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < playerInfo.playerUniqueSkills.Count; i++)
        {
            PlayerSkill skill = Instantiate(playerInfo.playerUniqueSkills[i], skillButtons[i].btnPos);

            skillButtons[i].gameObject.SetActive(true);

            print("�־���");
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
        // ��ų�� ��� �������� üũ
        if (skill.CanUseSkill())
        {
            print("����!!");

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

    private void SetSkill(PlayerSkillButton btn, PlayerSkill skill)
    {
        if (btn.currentSkill != null)
        {
            // ��ų Ǯ��
        }

        btn.Init(skill, ps =>
        {
            print($"���� ��ų : {ps.skillName}");

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
