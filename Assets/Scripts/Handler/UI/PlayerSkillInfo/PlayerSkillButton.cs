using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillButton : MonoBehaviour
{
    private Button skillBtn;

    public Transform btnPos;
    public PlayerSkill currentSkill;

    private readonly string resetStr = "";

    private void Awake()
    {
        skillBtn = GetComponent<Button>();
    }

    public void Init(PlayerSkill skill, Action<PlayerSkill> onClickBtn)
    {
        currentSkill = skill;

        skill.Init(this);

        skillBtn.onClick.AddListener(() =>
        {
            onClickBtn?.Invoke(currentSkill);
        });
    }
}
