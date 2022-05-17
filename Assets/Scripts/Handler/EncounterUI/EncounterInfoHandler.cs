using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class EncounterInfo
{
    public List<Scroll> scrollRewards = new List<Scroll>();
}

public class EncounterInfoHandler : MonoBehaviour
{
    public EncounterInfo encounterInfo;

    private SkillPrefabContainer sc;

    private void Start()
    {
        sc = GameManager.Instance.skillContainer;
    }

    public List<GameObject> GetRandomSkillRewards(int count)
    {
        List<GameObject> result = new List<GameObject>();
        List<GameObject> rewardRulletPieces = sc.playerSkillPrefabs.ToList();
        for (int i = 0; i < count; i++)
        {
            GameObject randomPiece = rewardRulletPieces[Random.Range(0, rewardRulletPieces.Count)];
            result.Add(randomPiece);
            rewardRulletPieces.Remove(randomPiece);
        }
        return result;
    }

    public List<Scroll> GetRandomScrollRewards(int count)
    {
        List<Scroll> result = new List<Scroll>();
        List<Scroll> rewardScrolls = encounterInfo.scrollRewards.ToList();
        for (int i = 0; i < count; i++)
        {
            Scroll randomScroll = rewardScrolls[Random.Range(0, rewardScrolls.Count)];
            result.Add(randomScroll);
            rewardScrolls.Remove(randomScroll);
        }
        return result;
    }
}