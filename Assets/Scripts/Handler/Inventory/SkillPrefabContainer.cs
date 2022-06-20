using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

        GameManager.Instance.skillContainer = this;
    }

    public List<SkillPiece> GetSkillsByGrade(GradeInfo gradeInfo)
    {
        List<SkillPiece> skills = new List<SkillPiece>();

        for (int i = 0; i < playerSkillPrefabs.Count; i++)
        {
            SkillPiece piece = playerSkillPrefabs[i].GetComponent<SkillPiece>();
            if(piece.skillGrade == gradeInfo)
            {
                skills.Add(piece);
            }
        }

        return skills;
    }

    public List<SkillPiece> GetSkillsByElement(ElementalType elementalType)
    {
        List<SkillPiece> skills = new List<SkillPiece>();

        for (int i = 0; i < playerSkillPrefabs.Count; i++)
        {
            SkillPiece piece = playerSkillPrefabs[i].GetComponent<SkillPiece>();
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
            skills = GetSkillsByGrade(GradeInfo.True6StarMythAwakeningLegendTranscendentReincarnation);
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

        for (int i = 0; i < count; i++)
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
                skills = GetSkillsByGrade(GradeInfo.True6StarMythAwakeningLegendTranscendentReincarnation);
            }

            int index = Random.Range(0, skills.Count - i - 1);

            result.Add(skills[index]);
            var temp = skills[skills.Count - i - 1];
            skills[skills.Count - i - 1] = skills[index];
            skills[index] = temp;
        }
        

        return result;
    }

    public GameObject GetSkillPrefab<T>() where T : SkillPiece
    {
        Type type = typeof(T);

        return skillPrefabDic[type.Name];
    }
}
