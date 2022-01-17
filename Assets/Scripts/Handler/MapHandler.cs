using System;
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
    private Node curNode;

    private MapCreater mapCreater;
    private Transform mapHider;

    private void Awake()
    {
        GameManager.Instance.mapHandler = this;
        mapCreater = GetComponent<MapCreater>();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    public void MovePlayer()
    {
        curPlayerPosIcon.SetParent(GetCurNodeTrm(curNode.idx, curNode.depth));
        curPlayerPosIcon.localPosition = Vector2.zero;


        //여기에 각 맵별 대충 구현
        //아래 디버그용
        if(curNode.depth == mapCreater.mapCols-1)
        {
            Invoke("ResetMap", 1);
        }
    }

    public void ResetMap()
    {
        mapCreater.CreateMap();
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
                GetCurNodeTrm(i, depth).GetComponent<Button>().interactable = false;
            }
        }
        curNode = node;
        for (int i = 0; i < node.pointNodeList.Count; i++)
        {
            int depth = node.depth + 1;
            int row = node.pointNodeList[i].idx;
            Image nextNodeImg = Content.transform.GetChild((depth * map[0].Count) + row).GetComponent<Image>();
            nextNodeImg.color = new Color(nextNodeImg.color.r, nextNodeImg.color.g, nextNodeImg.color.b, 1);
            GetCurNodeTrm(row, depth).GetComponent<Button>().interactable = true;
        }
        MovePlayer();
    }

    public void ShowMap()
    {
        Transform trm = Content.transform;
        int cols = map.Count;
        int rows = mapCreater.mapRows;
        Content.GetComponent<GridLayoutGroup>().constraintCount = rows;
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                Transform nodeTrm = GetCurNodeTrm(r, c);
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
                if(map[c][r].mapNode != mapNode.NONE && c < cols-1 && false)
                {
                    Camera mainCam = Camera.main;
                    for (int i = 0; i < map[c][r].pointNodeList.Count; i++)
                    {
                        LineRenderer lr = nodeTrm.gameObject.GetComponent<LineRenderer>();
                        Node pointNode = map[c][r].pointNodeList[i];

                        if (i<1)
                        {
                            lr.startWidth = 0.08f;
                            lr.endWidth = 0.08f;
                            Vector3 pos1 = mainCam.WorldToScreenPoint(Vector3.zero);
                            Vector3 pos2 = nodeTrm.InverseTransformPoint(GetCurNodeTrm(pointNode.idx, pointNode.depth).position);
                            lr.SetPosition(1, pos2);
                            lr.SetPosition(0, Vector2.zero);
                            print(GetCurNodeTrm(pointNode.idx, pointNode.depth));
                            print(GetCurNodeTrm(pointNode.idx, pointNode.depth).position);
                        }
                        else
                        {
                            int curPosCount = lr.positionCount;
                            lr.positionCount = curPosCount+2;
                            Vector3 pos1 = mainCam.WorldToScreenPoint(Vector3.zero);
                            Vector3 pos2 = nodeTrm.InverseTransformPoint(GetCurNodeTrm(pointNode.idx, pointNode.depth).position);
                            lr.SetPosition(curPosCount+1, pos2);
                            lr.SetPosition(curPosCount, Vector2.zero);
                            print(GetCurNodeTrm(pointNode.idx, pointNode.depth));
                            print(GetCurNodeTrm(pointNode.idx, pointNode.depth).position);
                        }
                    }
                }
                int col = c;
                int row = r;
                nodeTrm.GetComponent<Button>().interactable = false;
                nodeTrm.GetComponent<Button>().onClick.AddListener(() => { OnSelectNode(map[col][row]);  });
            }
        }
    }

    private Transform GetCurNodeTrm(int row, int col)
    {
        Transform nodeTrm = Content.transform.GetChild((mapCreater.mapRows * col) + row);
        return nodeTrm;
    }
}
