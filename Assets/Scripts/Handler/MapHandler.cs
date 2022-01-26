using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MapHandler : MonoBehaviour
{
    public EncounterHandler encounterHandler;

    [HideInInspector]
    public List<List<Node>> map;

    public List<Sprite> mapIcons;
    public List<Sprite> mapClearIcons;

    public GameObject mapUIs;
    public GameObject Content;

    public Transform curPlayerPosIcon;
    private Node curNode;

    private MapCreater mapCreater;
    private Transform mapHider;

    private Sequence openSequence;


    public GameObject lineNodePrefab;
    public Material SelectedLineMat;

    private void Awake()
    {
        GameManager.Instance.mapHandler = this;
        mapCreater = GetComponent<MapCreater>();
    }

    void Start()
    {
        ShowMap();
        OnSelectNode(map[0][3]);
    }

    void Update()
    {

    }

    public void OpenMapPanel(bool open, bool quick = false)
    {
        CanvasGroup cvsGroup = mapUIs.GetComponent<CanvasGroup>();

        openSequence.Kill();
        if (!quick)
        {
            if(open) mapUIs.SetActive(true);
            openSequence.Append(cvsGroup.DOFade(open ? 1 : 0, 0.5f).OnComplete(() => {
                cvsGroup.interactable = open;
                cvsGroup.blocksRaycasts = open;
                if(!open) mapUIs.SetActive(false);
            }));
        }
        else
        {
            cvsGroup.alpha = open ? 1 : 0;
            cvsGroup.interactable = open;
            cvsGroup.blocksRaycasts = open;
        }
    }

    public void MovePlayer()
    {
        curPlayerPosIcon.SetParent(GetCurNodeTrm(curNode.idx, curNode.depth));
        curPlayerPosIcon.localPosition = Vector2.zero;


        //여기에 각 맵별 대충 구현
        if(curNode.mapNode != mapNode.START)
        {
            print(curNode.mapNode);
            encounterHandler.StartEncounter(curNode.mapNode);
        }
        //아래 디버그용
        if(curNode.depth == mapCreater.mapCols-1)
        {
            Invoke("ResetMap", 1);
        }
    }

    public void ResetMap()
    {
        mapCreater.MapReset();
        mapCreater.CreateMap();
    }

    public void OnSelectNode(Node node)
    {
        if (node.depth>0)
        {
            int depth = node.depth;
            for (int i = 0; i < mapCreater.mapRows; i++)
            {
                if(map[depth][i].mapNode != mapNode.NONE && i != node.idx)
                {
                    Image img = Content.transform.GetChild((depth * map[0].Count) + i).GetComponent<Image>();
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
                }
                GetCurNodeTrm(i, depth).GetComponent<Button>().interactable = false;
            }

            for (int i = 0; i < curNode.pointNodeList.Count; i++)
            {
                if (curNode.pointNodeList[i] == node)
                {
                    print("tlqkf");
                    LineRenderer lr = GetCurNodeTrm(curNode.idx, curNode.depth).GetChild(i).GetComponent<LineRenderer>();
                    SetLineMat(lr, curNode.idx, node.idx, SelectedLineMat);
                }
            }
        }
        GetCurNodeTrm(node.idx, node.depth).GetComponent<Image>().sprite = mapClearIcons[node.spriteIdx];
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
        LayoutRebuilder.ForceRebuildLayoutImmediate(Content.GetComponent<RectTransform>());
        Transform trm = Content.transform;
        int cols = mapCreater.mapCols;
        int rows = mapCreater.mapRows;
        Content.GetComponent<GridLayoutGroup>().constraintCount = rows;
        for (int c = 0; c < cols; c++)
        {
            for (int r = 0; r < rows; r++)
            {
                //print(c + ","+r);
                Transform nodeTrm = GetCurNodeTrm(r, c);
                Sprite icon;
                switch (map[c][r].mapNode)
                {
                    case mapNode.NONE:
                        nodeTrm.GetComponent<Image>().color = Color.clear;
                        map[c][r].spriteIdx = 0;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    case mapNode.START:
                        map[c][r].spriteIdx = 0;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    case mapNode.BOSS:
                        GetCurNodeTrm(r, c).localScale =  Vector2.one *2f;
                        map[c][r].spriteIdx = 5;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    case mapNode.MONSTER:
                        map[c][r].spriteIdx = 0;
                        icon = mapIcons[map[c][r].spriteIdx];
                        icon = mapIcons[0];
                        break;
                    case mapNode.EMONSTER:
                        map[c][r].spriteIdx = 1;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    case mapNode.SHOP:
                        map[c][r].spriteIdx = 4;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    case mapNode.REST:
                        map[c][r].spriteIdx = 3;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    case mapNode.TREASURE:
                        map[c][r].spriteIdx = 2;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                    default:
                        map[c][r].spriteIdx = 0;
                        icon = mapIcons[map[c][r].spriteIdx];
                        break;
                }
                nodeTrm.GetComponent<Image>().sprite = icon;


                nodeTrm.GetComponent<Button>().onClick.RemoveAllListeners();
                if (map[c][r].mapNode != mapNode.NONE)
                {
                    if (c < cols - 1)
                    {
                        Camera mainCam = Camera.main;
                        //LineRenderer lr = nodeTrm.gameObject.GetComponent<LineRenderer>();
                        for (int i = 0; i < map[c][r].pointNodeList.Count; i++)
                        {
                            LineRenderer lr;
                            if (nodeTrm.childCount-1 < i)
                            {
                                GameObject lineObj = Instantiate(lineNodePrefab, nodeTrm);
                                lr = lineObj.GetComponent<LineRenderer>();
                                lr.positionCount = 2;
                            }
                            else
                            {
                                lr = nodeTrm.GetChild(i).GetComponent<LineRenderer>();
                            }

                            Node pointNode = map[c][r].pointNodeList[i];

                            SetLineMat(lr, r, pointNode.idx);

                            lr.startWidth = 0.08f;
                            lr.endWidth = 0.08f;
                            Vector3 pos1 = mainCam.WorldToScreenPoint(Vector3.zero);
                            Vector3 pos2 = nodeTrm.InverseTransformPoint(GetCurNodeTrm(pointNode.idx, pointNode.depth).position);
                            lr.SetPosition(1, pos2);
                            lr.SetPosition(0, Vector2.zero);


                        }
                    }
                    int col = c;
                    int row = r;
                    nodeTrm.GetComponent<Button>().onClick.AddListener(() => { OnSelectNode(map[col][row]); });
                }
                //else
                //{
                //    LineRenderer lr = nodeTrm.gameObject.GetComponent<LineRenderer>();
                //    lr.positionCount = 0;
                //}
                nodeTrm.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void SetLineMat(LineRenderer lr, int r, int p, Material mat = null)
    {
        if(mat != null)
        {
            lr.material = mat;
        }
        float x = Mathf.Abs(r - p);

        int z = ((int)new Vector2(x, 1).magnitude);
        lr.material.mainTextureScale = new Vector2(z * 3, lr.material.mainTextureScale.y);
    }

    private Transform GetCurNodeTrm(int row, int col)
    {
        Transform nodeTrm = Content.transform.GetChild((mapCreater.mapRows * col) + row);
        return nodeTrm;
    }
}
