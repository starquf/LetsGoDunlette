using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BattleInfo
{
    public List<EnemyHealth> enemyInfos = new List<EnemyHealth>();
    public List<GameObject> rewards = new List<GameObject>();
}

public class BattleInfoHandler : MonoBehaviour
{
    public List<BattleInfo> battleInfos = new List<BattleInfo>();

    public BattleInfo GetRandomBattleInfo()
    {
        int randIdx = Random.Range(0, battleInfos.Count);

        return battleInfos[randIdx];
    }
}
