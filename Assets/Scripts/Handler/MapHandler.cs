using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHandler : MonoBehaviour
{
    public bool isSetting = false;
    public List<List<Node>> map;

    public GameObject Content;

    private void Awake()
    {
        GameManager.Instance.mapHandler = this;
    }

    void Start()
    {
        StartCoroutine(SleepToSetting());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SleepToSetting()
    {
        yield return new WaitUntil(() => isSetting);
        ShowMap();
    }

    public void ShowMap()
    {
        Content.GetComponent<GridLayoutGroup>().constraintCount = map[0].Count;
        Transform trm = Content.transform;
        int cols = map.Count;
        int rows = map[0].Count;
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Color color = Color.clear;
                switch (map[c][r].mapNode)
                {
                    case mapNode.NONE:
                        color = Color.clear;
                        break;
                    case mapNode.START:
                        color = Color.green;
                        break;
                    case mapNode.BOSS:
                        color = Color.red;
                        break;
                    case mapNode.MONSTER:
                        color = Color.red;
                        break;
                    case mapNode.SHOP:
                        color = Color.yellow;
                        break;
                    case mapNode.REST:
                        color = Color.blue;
                        break;
                    case mapNode.TREASURE:
                        color = Color.grey;
                        break;
                    default:
                        break;
                }
                trm.GetChild((rows * c) + r).GetComponent<Image>().color = color;
                if(map[c][r].mapNode != mapNode.NONE && c < cols-1)
                {
                    for (int i = 0; i < map[c][r].pointNodeIdx.Count; i++)
                    {
                        LineRenderer lr = trm.GetChild((rows * c) + r).gameObject.GetComponent<LineRenderer>();
                        if (i<1)
                        {
                            lr.startWidth = 0.08f;
                            lr.endWidth = 0.08f;
                            lr.SetPosition(0, trm.GetChild((rows * c) + r).position);
                            lr.SetPosition(1, trm.GetChild((rows * (c + 1)) + map[c][r].pointNodeIdx[i]).position);
                        }
                        else
                        {
                            int curPosCount = lr.positionCount;
                            lr.positionCount = curPosCount+2;
                            lr.SetPosition(curPosCount, trm.GetChild((rows * c) + r).position);
                            lr.SetPosition(curPosCount+1, trm.GetChild((rows * (c + 1)) + map[c][r].pointNodeIdx[i]).position);
                        }
                    }
                }
            }
        }
    }
}
