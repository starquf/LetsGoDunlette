using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("캐릭터 정보")]
    public string characterName;

    public List<PlayerSkill> playerSkills = new List<PlayerSkill>();

    private void Awake()
    {
        GetComponents(playerSkills);
    }
}
