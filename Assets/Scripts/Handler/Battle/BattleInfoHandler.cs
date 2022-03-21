using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class BattleInfo
{
    public List<EnemyType> enemyInfos = new List<EnemyType>();

    public bool isWeakEnemy;
}

public class BattleInfoHandler : MonoBehaviour
{
    public List<BattleInfo> battleInfos = new List<BattleInfo>();
    public List<BattleInfo> bossInfos = new List<BattleInfo>();
    public List<BattleInfo> eliteInfos = new List<BattleInfo>();

    private int counter = 0;

    public BattleInfo GetRandomBattleInfo()
    {
        counter++;

        if (counter > 2)
        {
            int randIdx = Random.Range(0, battleInfos.Count);

            return battleInfos[randIdx];
        }
        else
        {
            List<BattleInfo> weakInfos = battleInfos.Where(x => x.isWeakEnemy).ToList();

            int randIdx = Random.Range(0, weakInfos.Count);

            return weakInfos[randIdx];
        }
    }

    public BattleInfo GetRandomBossInfo()
    {
        counter++;

        int randIdx = Random.Range(0, bossInfos.Count);

        return bossInfos[randIdx];
    }

    public BattleInfo GetRandomEliteInfo()
    {
        counter++;

        int randIdx = Random.Range(0, eliteInfos.Count);

        return eliteInfos[randIdx];
    }
}
