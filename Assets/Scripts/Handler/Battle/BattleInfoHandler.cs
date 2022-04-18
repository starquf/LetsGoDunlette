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
    [Header("�Ϲ� �� ����")]
    public List<BattleInfo> normalInfos = new List<BattleInfo>();

    [Header("����Ʈ �� ����")]
    public List<BattleInfo> eliteInfos = new List<BattleInfo>();

    [Header("���� �� ����")]
    public List<BattleInfo> bossInfos = new List<BattleInfo>();
}

public class BattleInfoHandler : MonoBehaviour
{
    [Header("�������� ������")]
    public List<StageInfo> stages = new List<StageInfo>();
    private int counter = 0;

    public BattleInfo GetRandomBattleInfo()
    {
        counter++;

        if (counter > 2)
        {
            int randIdx = Random.Range(0, stages[GameManager.Instance.StageIdx].normalInfos.Count);

            return stages[GameManager.Instance.StageIdx].normalInfos[randIdx];
        }
        else
        {
            List<BattleInfo> weakInfos = stages[GameManager.Instance.StageIdx].normalInfos.Where(x => x.isWeakEnemy).ToList();

            int randIdx = Random.Range(0, weakInfos.Count);

            //print("������");

            return weakInfos[randIdx];
        }
    }

    public BattleInfo GetRandomBossInfo()
    {
        int randIdx = Random.Range(0, stages[GameManager.Instance.StageIdx].bossInfos.Count);

        return stages[GameManager.Instance.StageIdx].bossInfos[randIdx];
    }

    public BattleInfo GetRandomEliteInfo()
    {
        int randIdx = Random.Range(0, stages[GameManager.Instance.StageIdx].eliteInfos.Count);

        return stages[GameManager.Instance.StageIdx].eliteInfos[randIdx];
    }

    public void SetStage(int stage)
    {
        GameManager.Instance.StageIdx = stage - 1;
    }
}
