using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerInfoPanelHandler : BottomSwapUI
{
    private BattleHandler bh;

    private PlayerInfo playerInfo;

    private List<PlayerSkillButton> skillButtons = new List<PlayerSkillButton>();
    public Transform skillBtnTrans;

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
                    return;

                if (!bh.isBattle || bh.mainRullet.IsStop)
                    return;

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
