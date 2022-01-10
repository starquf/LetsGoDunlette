using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapHandler : MonoBehaviour
{
    public bool isSetting = false;
    [HideInInspector]
    public List<List<Node>> map;

    public List<Sprite> mapIcons;

    public GameObject Content;

    public Transform curPlayerPosIcon;
    private GameObject curNode;

    private void Awake()
    {
        GameManager.Instance.mapHandler = this;
    }

    void Start()
    {
        StartCoroutine(SleepToSetting());
    }

    void Update()
    {
        
    }

    public IEnumerator SleepToSetting()
    {
        yield return new WaitUntil(() => isSetting);
        ShowMap();
        OnSelectNode(map[0][3]);
    }

    public void MovePlayer()
    {
        curPlayerPosIcon.position = curNode.transform.position;
    }

    public void OnSelectNode(Node node)
    {
        if(node.depth>0)
        {
            int depth = node.depth;
            for (int i = 0; i < map[depth].Count; i++)
            {
                if(map[depth][i].mapNode != mapNode.NONE && i != node.idx)
                {
                    Image img = Content.transform.GetChild((depth * map[0].Count) + i).GetComponent<Image>();
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
                }
                Content.transform.GetChild((depth * map[0].Count) + i).GetComponent<Button>().interactable = false;
            }
        }
        curNode = Content.transform.GetChild((node.depth * map[0].Count) + node.idx).gameObject;
        for (int i = 0; i < node.pointNodeIdx.Count; i++)
        {
            int depth = node.depth + 1;
            int row = node.pointNodeIdx[i];
            Image nextNodeImg = Content.transform.GetChild((depth * map[0].Count) + row).GetComponent<Image>();
            nextNodeImg.color = new Color(nextNodeImg.color.r, nextNodeImg.color.g, nextNodeImg.color.b, 1);
            Content.transform.GetChild((depth * map[0].Count) + row).GetComponent<Button>().interactable = true;
        }
        MovePlayer();
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
                Transform nodeTrm = trm.GetChild((rows * c) + r);
                Color color = Color.clear;
                switch (map[c][r].mapNode)
                {
                    case mapNode.NONE:
                        color = Color.clear;
                        break;
                    case mapNode.START:
                        color = new Color(1, 0, 1, 1f);
                        break;
                    case mapNode.BOSS:
                        color = new Color(1, 0, 0, 0.5f);
                        break;
                    case mapNode.MONSTER:
                        color = new Color(1, 0, 0, 0.5f);
                        break;
                    case mapNode.SHOP:
                        color = new Color(1, 0.92f, 0.016f, 0.5f);
                        break;
                    case mapNode.REST:
                        color = new Color(0, 0, 1, 0.5f);
                        break;
                    case mapNode.TREASURE:
                        color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    default:
                        break;
                }
                nodeTrm.GetComponent<Image>().color = color;
                if(map[c][r].mapNode != mapNode.NONE && c < cols-1)
                {
                    for (int i = 0; i < map[c][r].pointNodeIdx.Count; i++)
                    {
                        LineRenderer lr = nodeTrm.gameObject.GetComponent<LineRenderer>();
                        if (i<1)
                        {
                            lr.startWidth = 0.08f;
                            lr.endWidth = 0.08f;
                            lr.SetPosition(0, nodeTrm.position);
                            lr.SetPosition(1, trm.GetChild((rows * (c + 1)) + map[c][r].pointNodeIdx[i]).position);
                        }
                        else
                        {
                            int curPosCount = lr.positionCount;
                            lr.positionCount = curPosCount+2;
                            lr.SetPosition(curPosCount, nodeTrm.position);
                            lr.SetPosition(curPosCount+1, trm.GetChild((rows * (c + 1)) + map[c][r].pointNodeIdx[i]).position);
                        }
                    }
                }
                int col = c;
                int row = r;
                nodeTrm.GetComponent<Button>().interactable = false;
                nodeTrm.GetComponent<Button>().onClick.AddListener(() => { print(col + ", "+row); OnSelectNode(map[col][row]);  });
            }
        }
    }
}
