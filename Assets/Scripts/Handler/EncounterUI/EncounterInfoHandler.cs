using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EncounterInfo
{
    public List<RulletPiece> rewards = new List<RulletPiece>();
}

public class EncounterInfoHandler : MonoBehaviour
{
    public EncounterInfo encounterInfo;

    public List<RulletPiece> GetRandomRewards(int count)
    {
        List<RulletPiece> result = new List<RulletPiece>();
        List<RulletPiece> rewardRulletPieces = encounterInfo.rewards;
        for (int i = 0; i < count; i++)
        {
            RulletPiece randomPiece = rewardRulletPieces[Random.Range(0, rewardRulletPieces.Count)];
            result.Add(randomPiece);
            rewardRulletPieces.Remove(randomPiece);
        }
        return result;
    }
}