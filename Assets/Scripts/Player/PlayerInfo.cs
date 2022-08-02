using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [Header("캐릭터 정보")]
    public PlayerCharacterName characterName;
    public Sprite playerImg;
    public Sprite playerIllust;

    public List<PlayerSkillName> playerUniqueSkills = new List<PlayerSkillName>();
}
