using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class BattleInfo
{
    public List<EnemyType> enemyInfos = new List<EnemyType>();

    public bool isWeakEnemy;
    public Sprite bg;
}

[System.Serializable]
public class StageInfo
{
    [Header("일반 적 정보")]
    public List<BattleInfo> normalInfos = new List<BattleInfo>();

    [Header("엘리트 적 정보")]
    public List<BattleInfo> eliteInfos = new List<BattleInfo>();

    [Header("보스 적 정보")]
    public List<BattleInfo> bossInfos = new List<BattleInfo>();
}

public class BattleInfoHandler : MonoBehaviour
{
    [Header("스테이지 정보들")]
    public List<StageInfo> stages = new List<StageInfo>();

    private int currentStage = 0;
    private int counter = 0;

    public BattleInfo GetRandomBattleInfo()
    {
        counter++;

        if (counter > 2)
        {
            int randIdx = Random.Range(0, stages[currentStage].normalInfos.Count);

            return stages[currentStage].normalInfos[randIdx];
        }
        else
        {
            List<BattleInfo> weakInfos = stages[currentStage].normalInfos.Where(x => x.isWeakEnemy).ToList();

            int randIdx = Random.Range(0, weakInfos.Count);

            //print("약한적");

            return weakInfos[randIdx];
        }
    }

    public BattleInfo GetRandomBossInfo()
    {
        int randIdx = Random.Range(0, stages[currentStage].bossInfos.Count);

        return stages[currentStage].bossInfos[randIdx];
    }

    public BattleInfo GetRandomEliteInfo()
    {
        int randIdx = Random.Range(0, stages[currentStage].eliteInfos.Count);

        return stages[currentStage].eliteInfos[randIdx];
    }

    public void SetStage(int stage)
    {
        currentStage = stage - 1;
    }
}
