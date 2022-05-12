using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("ĳ���� ����")]
    public string characterName;

    public List<PlayerSkill> playerSkills = new List<PlayerSkill>();

    private void Awake()
    {
        GetComponents(playerSkills);
    }
}
