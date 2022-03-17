using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrefabContainer : MonoBehaviour
{
    [Header("전체 스킬 프리팹")]
    [SerializeField]
    private List<GameObject> skillPrefabs = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> enemySkillPrefabs;
    [HideInInspector]
    public List<GameObject> playerSkillPrefabs;

    private Dictionary<string, GameObject> skillPrefabDic;

    private void Awake()
    {
        skillPrefabDic = new Dictionary<string, GameObject>();
        enemySkillPrefabs = new List<GameObject>();
        playerSkillPrefabs = new List<GameObject>();

        for (int i = 0; i < skillPrefabs.Count; i++)
        {
            SkillPiece piece = skillPrefabs[i].GetComponent<SkillPiece>();

            string name = piece.GetType().Name;
            //print($"스킬 이름 : {name}");

            if (piece.isPlayerSkill)
                playerSkillPrefabs.Add(skillPrefabs[i]);
            else
                enemySkillPrefabs.Add(skillPrefabs[i]);

            skillPrefabDic.Add(name, skillPrefabs[i]);
        }

        GameManager.Instance.skillContainer = this;
    }

    public GameObject GetSkillPrefab<T>() where T : SkillPiece
    {
        Type type = typeof(T);

        return skillPrefabDic[type.Name];
    }
}
