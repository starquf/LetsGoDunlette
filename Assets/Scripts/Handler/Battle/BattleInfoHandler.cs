using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class BattleInfo
{
    public List<EnemyType> enemyInfos = new List<EnemyType>();

    public bool isWeakEnemy;
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
    [SerializeField]
    private List<StageInfo> stages = new List<StageInfo>();
    private int counter = 0;

    void Awake()
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

            for (var i = 0; i < data.Count; i++)
            {
                print("name " + data[i]["EnemyName"] + " " +
                       "age " + data[i]["Type"] + " " +
                       "speed " + data[i]["BG"]);

                List<EnemyType> enemyInfos = new List<EnemyType>();
                bool isWeek = false;

                string name = (string)data[i]["EnemyName"];

                if (name.Contains(','))
                {
                    var enemys = Regex.Split(name, ",");

                    for (int j = 0; j < enemys.Length; j++)
                    {
                        EnemyType type;
                        bool check = System.Enum.TryParse(enemys[j], out type);
                        if (check)
                        {
                            enemyInfos.Add(type);
                        }
                        else
                        {
                            print($"{enemys[j]} ã���� �����ϴ�. ���� ��Ʈ�� Ȯ�����ּ���. (������ ��ȹ�� �����Դϴ�.) ");
                        }
                    }
                }
                else
                {
                    EnemyType type;
                    bool check = System.Enum.TryParse(name, out type);
                    if (check)
                    {
                        enemyInfos.Add(type);
                    }
                    else
                    {
                        print($"{name} ã���� �����ϴ�. ���� ��Ʈ�� Ȯ�����ּ���. (������ ��ȹ�� �����Դϴ�.) ");
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
                    print($"{path} �� ã���� �����ϴ�. ���� ��Ʈ�� Ȯ�����ּ���. (������ ��ȹ�� �����Դϴ�.)");
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
                        print($"{(string)data[i]["Type"]} �߸��� Ÿ���� �Է��ϼ̽��ϴ�. ���� ��Ʈ�� Ȯ�����ּ���. (������ ��ȹ�� �����Դϴ�.) ");
                        return;
                }
            }

            stages.Add(stage);
            k++;
        }

        
    }

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
