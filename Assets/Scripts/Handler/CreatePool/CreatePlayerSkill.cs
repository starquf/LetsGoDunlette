using System.Collections.Generic;
using UnityEngine;

public class CreatePlayerSkill : MonoBehaviour
{
    [Header("��ũ�� ������Ʈ��")]
    public List<PlayerSkill> skillList = new List<PlayerSkill>();

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < skillList.Count; i++)
        {
            PoolManager.CreatePlayerSkillPool(skillList[i].skillNameType, skillList[i].gameObject, transform, 1);
        }
    }
}
