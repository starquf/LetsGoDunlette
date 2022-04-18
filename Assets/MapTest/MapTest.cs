using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomDic;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class MapTest : MonoBehaviour
{
    public int mapLength = 12;
    public int encounterAndMonsterShowIdx = 4;

    public SerializableDictionary<int, mapNode> fixedMapDic = new SerializableDictionary<int, mapNode>();
    [HideInInspector]
    public List<Map> updateapList = new List<Map>();

    public Transform mapUIs;
    public Transform mapEffectParent;
    public Transform Content;
    public Transform curPlayerPosIcon;

    public List<Sprite> mapIcons;

    public Map nodePrefab;
    public Button nodeBtnPrefab;

    private Sequence openSequence;
    void Start()
    {
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitMap()
    {
        // clear map content
        for (int i = 0; i < mapLength+1; i++)
        {
            Map map = Instantiate(nodePrefab.gameObject, Content).GetComponent<Map>();
            map.mapTest = this;
            map.mapIdx = i;
            if(i<= encounterAndMonsterShowIdx)
            {
                map.cannotBeNodesList.AddRange(new List<mapNode>() { mapNode.REST, mapNode.SHOP, mapNode.EMONSTER });
            }
            updateapList.Add(map);
        }
        CreateMap();
    }

    public void ResetMap()
    {

    }

    public void CreateMap()
    {
        for (int i = 0; i < updateapList.Count; i++)
        {
            Button btn = Instantiate(nodeBtnPrefab, updateapList[i].transform).GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                OnNodeBtnClick();
            });
            btn.interactable = false;
        }
        SetMapNode();
        OnNodeBtnClick();
    }

    public void SetMapNode()
    {
        for (int i = 0; i < updateapList.Count; i++)
        {
            Map map = updateapList[i];

            map.isFixedType = fixedMapDic.ContainsKey(map.mapIdx);
            if (map.isFixedType)
            {
                map.SetMapNode(fixedMapDic[i]);

            }
            else
            {
                map.SetRandomNode();
            }
        }
    }
    public void OnNodeBtnClick()
    {
        Map willMoveMap = updateapList[0];
        updateapList.Remove(willMoveMap);

        willMoveMap.SetBtnInteractable(false);

        if (willMoveMap.randomNodesList.Count > 0)
        {
            willMoveMap.SetRandomNode();
        }

        if (willMoveMap.isFixedType)
        {
            MovePlayer(willMoveMap.transform.GetChild(0), () =>
            {
                willMoveMap.StartMap();
                updateapList[0].SetBtnInteractable(true);
            });
        }
        else
        {
            Destroy(willMoveMap.transform.GetChild(0).gameObject);
            int count = willMoveMap.randomNodesList.Count;
            List<Button> selectBtnList = new List<Button>();
            for (int i = 0; i < count; i++)
            {
                mapNode type = willMoveMap.randomNodesList[i];
                //Image mapImg = Instantiate(nodeBtnPrefab.gameObject, mapEffectParent).GetComponent<Image>();
                //mapImg.transform.position = willMoveMap.transform.position;
                Button btn = Instantiate(nodeBtnPrefab, willMoveMap.transform).GetComponent<Button>();
                selectBtnList.Add(btn);
                btn.GetComponent<Image>().sprite = GetTypeIcon(type, false);
                btn.onClick.AddListener(() =>
                {
                    // 자신이 아닌 버튼 제거
                    for (int j = selectBtnList.Count-1; j >= 0; j--)
                    {
                        if(btn != selectBtnList[j])
                        {
                            Destroy(selectBtnList[j].gameObject);
                        }
                    }

                    willMoveMap.SetMapNode(type);
                    MovePlayer(btn.transform, () =>
                    {
                        willMoveMap.StartMap();
                willMoveMap.SetBtnInteractable(false);
                        updateapList[0].SetBtnInteractable(true);
                    }, true);
                });
            }
        }
    }

    public List<Map> GetUpDownMap(int idx)
    {
        List<Map> upDownMapList = new List<Map>();
        for (int i = 0; i < updateapList.Count; i++)
        {
            Map map = updateapList[i];
            if (map.mapIdx == idx-1 || map.mapIdx == idx +1)
            {
                upDownMapList.Add(map);
            }
        }
        return upDownMapList;
    }

    public Sprite GetTypeIcon(mapNode type, bool isClearIcon = false)
    {
        int iconSpriteIdx = -1;
        switch (type)
        {
            case mapNode.NONE:
                break;
            case mapNode.START:
                break;
            case mapNode.BOSS:
                /*
                int bossIdx = -1;
                bossIdx = GameManager.Instance.battleHandler.SetRandomBoss();
                if (bossIdx < 0)
                {
                    Debug.LogError("이상한 보스가 설정됨2");
                }*/
                iconSpriteIdx = 5;// + bossIdx;
                break;
            case mapNode.MONSTER:
                iconSpriteIdx = 0;
                break;
            case mapNode.EMONSTER:
                iconSpriteIdx = 1;
                break;
            case mapNode.SHOP:
                iconSpriteIdx = 4;
                break;
            case mapNode.REST:
                iconSpriteIdx = 3;
                break;
            case mapNode.RandomEncounter:
                iconSpriteIdx = 2;
                break;
            default:
                break;
        }
        if (iconSpriteIdx < 0)
            return null;

        return mapIcons[isClearIcon? (iconSpriteIdx * 2) : (iconSpriteIdx * 2) + 1];
    }

    private void MovePlayer(Transform mapBtn, Action onComplete, bool animSkip = false)
    {
        curPlayerPosIcon.SetParent(mapBtn);
        if (animSkip)
        {
            curPlayerPosIcon.localPosition = Vector3.zero;
            onComplete?.Invoke();
        }
        else
        {
            DOTween.Sequence().Append(curPlayerPosIcon.DOLocalMove(Vector3.zero, 0.5f).SetDelay(0.2f))
                .OnComplete(() => {
                    onComplete?.Invoke();
                });
        }
    }

    public void OpenMapPanel(bool open, bool quick = false)
    {
        CanvasGroup cvsGroup = mapUIs.GetComponent<CanvasGroup>();

        openSequence.Kill();
        if (!quick)
        {
            if (!open)
            {
                cvsGroup.interactable = false;
                cvsGroup.blocksRaycasts = false;
            }
            openSequence = DOTween.Sequence()
                .Append(cvsGroup.DOFade(open ? 1 : 0, 0.5f).OnComplete(() => {
                    cvsGroup.interactable = open;
                    cvsGroup.blocksRaycasts = open;
                }));
        }
        else
        {
            cvsGroup.alpha = open ? 1 : 0;
            cvsGroup.interactable = open;
            cvsGroup.blocksRaycasts = open;
        }
        if (open)
        {
            SoundHandler.Instance.PlayBGMSound("Battle_2");
        }
    }
}
