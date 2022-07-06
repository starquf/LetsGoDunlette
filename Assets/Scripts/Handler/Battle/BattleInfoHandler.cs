using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class BattleInfo
{
    public List<EnemyType> enemyInfos = new List<EnemyType>();

    public bool isWeakEnemy;
    public float appearWeight;

    public Sprite bg;
    public BattleInfo() { }
    public BattleInfo(List<EnemyType> enemyInfos, bool isWeakEnemy, Sprite bg)
    {
        this.enemyInfos = enemyInfos;
        this.isWeakEnemy = isWeakEnemy;
        this.bg = bg;
    }
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
    [SerializeField]
    private List<StageInfo> stages = new List<StageInfo>();

    private int weakCounter = 0;

    private int maxWeakCounter = 2;

    private void Awake()
    {
        LoadStageData();
    }

    private void LoadStageData()
    {
        int k = 1;
        while (Resources.Load<TextAsset>($"StageCSV/Stage{k}") != null)
        {
            List<Dictionary<string, object>> data = CSVReader.Read($"StageCSV/Stage{k}");
            StageInfo stage = new StageInfo();

            for (int i = 0; i < data.Count; i++)
            {
                print("name " + data[i]["EnemyName"] + " " +
                       "age " + data[i]["Type"] + " " +
                       "speed " + data[i]["BG"]);

                List<EnemyType> enemyInfos = new List<EnemyType>();
                bool isWeek = false;

                string name = (string)data[i]["EnemyName"];

                if (name.Contains(','))
                {
                    string[] enemys = Regex.Split(name, ",");

                    for (int j = 0; j < enemys.Length; j++)
                    {
                        bool check = System.Enum.TryParse(enemys[j], out EnemyType type);
                        if (check)
                        {
                            enemyInfos.Add(type);
                        }
                        else
                        {
                            print($"{enemys[j]} 찾을수 없습니다. 엑셀 시트를 확인해주세요. (보통은 기획자 문제입니다.) ");
                        }
                    }
                }
                else
                {
                    bool check = System.Enum.TryParse(name, out EnemyType type);
                    if (check)
                    {
                        enemyInfos.Add(type);
                    }
                    else
                    {
                        print($"{name} 찾을수 없습니다. 엑셀 시트를 확인해주세요. (보통은 기획자 문제입니다.) ");
                    }
                }

                if ((string)data[i]["Type"] == "Week")
                {
                    isWeek = true;
                }

                string path = "Sprite/stageBackground/" + (string)data[i]["BG"];
                Sprite bg = Resources.Load<Sprite>(path);

                if (bg.Equals(null))
                {
                    print($"{path} 를 찾을수 없습니다. 엑셀 시트를 확인해주세요. (보통은 기획자 문제입니다.)");
                    return;
                }

                BattleInfo info = new BattleInfo(enemyInfos, isWeek, bg);

                switch ((string)data[i]["Type"])
                {
                    case "Week":
                    case "Normal":
                        stage.normalInfos.Add(info);
                        break;
                    case "Elite":
                        stage.eliteInfos.Add(info);
                        break;
                    case "Boss":
                        stage.bossInfos.Add(info);
                        break;
                    default:
                        print($"{(string)data[i]["Type"]} 잘못된 타입을 입력하셨습니다. 엑셀 시트를 확인해주세요. (보통은 기획자 문제입니다.) ");
                        return;
                }
            }

            stages.Add(stage);
            k++;
        }
    }

    public BattleInfo GetRandomBattleInfo()
    {
        weakCounter++;

        GetMaxWeakCounter();

        if (weakCounter > maxWeakCounter)
        {
            float total = 0;

            List<BattleInfo> battleInfos = stages[GameManager.Instance.StageIdx].normalInfos.OrderBy(x => x.appearWeight).ToList();
            BattleInfo selectedInfo = battleInfos[battleInfos.Count - 1];

            for (int i = 0; i < battleInfos.Count; i++)
            {
                total += battleInfos[i].appearWeight;
            }

            float randAppear = Random.Range(0f, total);

            for (int i = 0; i < battleInfos.Count; i++)
            {
                if (randAppear < battleInfos[i].appearWeight)
                {
                    selectedInfo = battleInfos[i];
                    break;
                }
                else
                {
                    randAppear -= battleInfos[i].appearWeight;
                }
            }

            return selectedInfo;
        }
        else
        {
            List<BattleInfo> weakInfos = stages[GameManager.Instance.StageIdx].normalInfos.Where(x => x.isWeakEnemy).ToList();

            int randIdx = Random.Range(0, weakInfos.Count);

            //print("약한적");

            return weakInfos[randIdx];
        }
    }

    private void GetMaxWeakCounter()
    {
        switch (GameManager.Instance.StageIdx)
        {
            case 0:
                maxWeakCounter = 2;
                break;

            case 1:
                maxWeakCounter = 1;
                break;

            default:
                maxWeakCounter = 1;
                break;
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

    public void ResetWeakCounter()
    {
        weakCounter = 0;
    }

    public void SetStage(int stage, int maxWeakCounter)
    {
        GameManager.Instance.StageIdx = stage - 1;
        this.maxWeakCounter = maxWeakCounter;
    }
}
