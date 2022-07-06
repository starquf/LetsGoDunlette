using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillPanelHandler : MonoBehaviour
{
    private BattleHandler bh;
    private List<PlayerSkillButton> skillButtons = new List<PlayerSkillButton>();
    public Transform skillBtnTrans;
    public CanvasGroup skillChangePopupCvsGroup;

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

        //SetSkill(skillButtons[1], PoolManager.GetPlayerSkill(PlayerSkillName.Reconstruction));
        //SetSkill(skillButtons[2], PoolManager.GetPlayerSkill(PlayerSkillName.FirstAid));
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

    public PlayerSkillName GetPlayerSkillNameInButton(int buttonIdx)
    {
        return skillButtons[buttonIdx].currentSkill.skillNameType;
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
            itemDesHandler.ShowDes(skill.skillName, skill.skillDes, skill.iconSpr, "사용",
                () =>
                {
                    Time.timeScale = 1f;

                    isCasting = true;

                    skill.Cast(() =>
                    {
                        bh.StartTurn();

                        isCasting = false;
                    },
                    () =>
                    {
                        Time.timeScale = 1f;
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

    public void GetSkill(PlayerSkill skill, Action onCompleteAnim = null, Action onCancleChange = null)
    {
        int btnIdx = GetCanSetButtonIdx(skill.isUniqueSkill);
        if (btnIdx == -1 || btnIdx == 0)
        {
            GameManager.Instance.YONHandler.ShowPanel("얻은 스킬과 가지고 있는 스킬을 변경 하기겠습니까?", "변경", "취소", () =>
            {
                // 변경
                if (btnIdx == -1)
                {
                    DOTween.To(() => skillChangePopupCvsGroup.alpha, x => skillChangePopupCvsGroup.alpha = x, 1, 0.5f);
                    for (int i = 1; i < skillButtons.Count; i++)
                    {
                        int iIdx = i;
                        PlayerSkillButton playerSkillButton = skillButtons[iIdx];

                        playerSkillButton.SetStrokeColor(Color.white);
                        // 모든 공용 스킬에 변경할 스크롤 선택으로 버튼 변경
                        playerSkillButton.SetAddListener(() =>
                        {
                            DOTween.To(() => skillChangePopupCvsGroup.alpha, x => skillChangePopupCvsGroup.alpha = x, 0, 0.5f)
                            .OnComplete(() =>
                            {
                                SetSkillAnim(skill, playerSkillButton, () =>
                                {
                                    // 선택시 다른 버튼들 원래 스킬로 다시 변경
                                    for (int j = 0; j < skillButtons.Count; j++)
                                    {
                                        int jIdx = j;
                                        PlayerSkillButton psb = skillButtons[jIdx];
                                        if (iIdx != jIdx)
                                        {
                                            psb.SetCurSkillAgain(ps =>
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

                                        switch (psb.currentSkill.skillType)
                                        {
                                            case PlayerSkillType.Active_Cooldown:
                                                ((PlayerSkill_Cooldown)psb.currentSkill).UpdateUI(psb);
                                                break;
                                            case PlayerSkillType.Active_Count:
                                                Debug.LogError("이거 버그 눌러서 확인");
                                                break;
                                            case PlayerSkillType.Passive:
                                                Debug.LogError("이거 버그 눌러서 확인");
                                                break;
                                            default:
                                                Debug.LogError("이거 버그 눌러서 확인");
                                                break;
                                        }
                                    }
                                    onCompleteAnim?.Invoke();
                                });
                            });
                        });
                    }
                }
                else
                {
                    SetSkillAnim(skill, skillButtons[btnIdx], onCompleteAnim);
                }
            }, () =>
            {
                // 취소
                onCancleChange?.Invoke();
                //onCompleteAnim?.Invoke();
            });
        }
        else
        {
            SetSkillAnim(skill, skillButtons[btnIdx], onCompleteAnim);
        }
        //skill.transform.DOMove()
    }

    private void SetSkillAnim(PlayerSkill skill, PlayerSkillButton playerSkillButton, Action onEnd = null)
    {
        DOTween.Sequence().Append(skill.transform.DOMove(playerSkillButton.transform.position, 0.5f))
            .OnComplete(() =>
            {
                SetSkill(playerSkillButton, skill);
                onEnd?.Invoke();
            });
    }

    private int GetCanSetButtonIdx(bool isUniqueSkill)
    {
        if (isUniqueSkill)
        {
            return 0;
        }
        else
        {
            for (int i = 1; i < skillButtons.Count; i++)
            {
                PlayerSkillButton playerSkillButton = skillButtons[i];
                if (!playerSkillButton.HasSkill())
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public void SetInteract(bool enable)
    {
        canCast = enable;
    }
}
