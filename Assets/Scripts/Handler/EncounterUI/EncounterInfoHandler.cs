using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EncounterInfo
{
    public List<GameObject> rewards = new List<GameObject>();
}

public class EncounterInfoHandler : MonoBehaviour
{
    public EncounterInfo encounterInfo;

    public List<GameObject> GetRandomRewards(int count)
    {
        List<GameObject> result = new List<GameObject>();
        List<GameObject> rewardRulletPieces = encounterInfo.rewards;
        for (int i = 0; i < count; i++)
        {
            GameObject randomPiece = rewardRulletPieces[Random.Range(0, rewardRulletPieces.Count)];
            result.Add(randomPiece);
            rewardRulletPieces.Remove(randomPiece);
        }
        return result;
    }
}