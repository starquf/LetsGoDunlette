using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SkillGetType
{
    NONE = -1,
    BATTLE = 0,
    SHOP = 1,
}

public class SkillPrefabContainer : MonoBehaviour
{
    [Header("전체 스킬 프리팹")]
    [SerializeField]
    private List<GameObject> skillPrefabs = new List<GameObject>();

    [Header("보상 스킬 프리팹")]
    [SerializeField]
    private List<GameObject> rewardSkillPrefabs = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> enemySkillPrefabs;
    [HideInInspector]
    public List<GameObject> playerSkillPrefabs;

    [Header("상점 확률 보상")]
    public List<RewardChance> shopRewardChances;

    private Dictionary<string, GameObject> skillPrefabDic;
    private Dictionary<string, GameObject> rewardSkillPrefabDic;

    private void Awake()
    {
        skillPrefabDic = new Dictionary<string, GameObject>();
        rewardSkillPrefabDic = new Dictionary<string, GameObject>();
        enemySkillPrefabs = new List<GameObject>();
        playerSkillPrefabs = new List<GameObject>();

        for (int i = 0; i < skillPrefabs.Count; i++)
        {
            SkillPiece piece = skillPrefabs[i].GetComponent<SkillPiece>();

            string name = piece.GetType().Name;
            ////print($"스킬 이름 : {name}");

            if (piece.isPlayerSkill)
            {
                playerSkillPrefabs.Add(skillPrefabs[i]);
            }
            else
            {
                enemySkillPrefabs.Add(skillPrefabs[i]);
            }

            skillPrefabDic.Add(name, skillPrefabs[i]);
        }

        for (int i = 0; i < rewardSkillPrefabs.Count; i++)
        {
            SkillPiece piece = rewardSkillPrefabs[i].GetComponent<SkillPiece>();
            string name = piece.GetType().Name;
            rewardSkillPrefabDic.Add(name, skillPrefabs[i]);
        }

        GameManager.Instance.skillContainer = this;
    }

    public List<SkillPiece> GetSkillsByGrade(GradeInfo gradeInfo)
    {
        List<SkillPiece> skills = new List<SkillPiece>();

        for (int i = 0; i < rewardSkillPrefabs.Count; i++)
        {
            SkillPiece piece = rewardSkillPrefabs[i].GetComponent<SkillPiece>();
            if (piece.skillGrade == gradeInfo && piece.isDisposable == false)
            {
                skills.Add(piece);
            }
        }

        return skills;
    }

    public List<SkillPiece> GetSkillsByElement(ElementalType elementalType)
    {
        List<SkillPiece> skills = new List<SkillPiece>();

        for (int i = 0; i < rewardSkillPrefabs.Count; i++)
        {
            SkillPiece piece = rewardSkillPrefabs[i].GetComponent<SkillPiece>();
            if (piece.patternType == elementalType)
            {
                skills.Add(piece);
            }
        }

        return skills;
    }

    public SkillPiece GetSkillByChance(RewardChance chance)
    {
        int gradeOne = chance.gradeOne;
        int gradeTwo = chance.gradeTwo;
        int gradeThree = chance.gradeThree;

        SkillPiece skill = null;
        List<SkillPiece> skills = null;
        int random = Random.Range(0, gradeOne + gradeTwo + gradeThree);

        if (random <= gradeOne)
        {
            skills = GetSkillsByGrade(GradeInfo.Normal);
            skill = skills[Random.Range(0, skills.Count)];
        }
        else if (random <= gradeOne + gradeTwo)
        {
            skills = GetSkillsByGrade(GradeInfo.Epic);
            skill = skills[Random.Range(0, skills.Count)];
        }
        else if (random <= gradeOne + gradeTwo + gradeThree)
        {
            skills = GetSkillsByGrade(GradeInfo.Legend);
            skill = skills[Random.Range(0, skills.Count)];
        }

        return skill;
    }

    public List<SkillPiece> GetSkillsByChance(RewardChance chance, int count)
    {
        int gradeOne = chance.gradeOne;
        int gradeTwo = chance.gradeTwo;
        int gradeThree = chance.gradeThree;

        List<SkillPiece> result = new List<SkillPiece>();
        List<SkillPiece> skills = null;

        for (int i = 1; i <= count; i++)
        {
            SkillPiece skill = null;
            do
            {
                int random = Random.Range(0, gradeOne + gradeTwo + gradeThree);

                if (random <= gradeOne)
                {
                    skills = GetSkillsByGrade(GradeInfo.Normal);
                }
                else if (random <= gradeOne + gradeTwo)
                {
                    skills = GetSkillsByGrade(GradeInfo.Epic);
                }
                else if (random <= gradeOne + gradeTwo + gradeThree)
                {
                    skills = GetSkillsByGrade(GradeInfo.Legend);
                }

                skill = skills[Random.Range(0, skills.Count)];

            } while (result.Contains(skill));

            result.Add(skill);
        }
        

        return result;
    }

    public GameObject GetSkillPrefab<T>() where T : SkillPiece
    {
        Type type = typeof(T);
        return skillPrefabDic[type.Name];
    }
}
