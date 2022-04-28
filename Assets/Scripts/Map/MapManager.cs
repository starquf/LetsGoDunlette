using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using UnityEngine.UI;
using Cinemachine;
using CustomDic;
using TMPro;

public enum mapNode
{
    NONE = 0,
    START = 1,
    BOSS = 2,
    EMONSTER = 3,
    MONSTER = 4,
    SHOP = 5,
    REST = 6,
    RandomEncounter = 7,
}

public class MapManager : MonoBehaviour
{
    Sequence mapOpenSequence;
    Tween zoomTween;

    [HideInInspector] public Dictionary<Vector2, Map> tiles;
    private Map curMap;
    private int bossCount;

    [Header("�� ���� ����")]
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] CanvasGroup mapCvsGroup;
    [SerializeField] CanvasGroup mapBGCvsGroup;
    [SerializeField] Transform playerTrm;
    [SerializeField] CinemachineVirtualCamera playerFollowCam;
    [SerializeField] Text bossCountTxt;

    [SerializeField] GameObject brokenTile;
    [SerializeField] List<Sprite> mapIconSpriteList = new List<Sprite>();

    [Header("�� �뷱�� ����")]
    [SerializeField] int defaultBossCount;
    [SerializeField] List<mapNode> canNotLinkMapType = new List<mapNode>();
    [SerializeField] SerializableDictionary<mapNode, int> fixedMapTypeCount = new SerializableDictionary<mapNode, int>();



    private EncounterHandler encounterHandler;

    void Awake()
    {
        GameManager.Instance.mapManager = this;
        tiles = new Dictionary<Vector2, Map>();
    }

    void Start()
    {
        encounterHandler = GameManager.Instance.encounterHandler;
        mapGenerator.GenerateGrid(OnGenerateMap);
    }

    // �� ����
    public void StartMap(mapNode mapType)
    {
        encounterHandler.StartEncounter(mapType);
    }

    // �� ��������� ��ó���� �̵�
    public void SetPlayerStartPos(Map map)
    {
        curMap = map;
        SetInteractebleCanSelectMap();
        playerTrm.position = map.transform.position;
    }

    // �� ��������� �ؾߵɰ�
    public void OnGenerateMap()
    {
        LinkMap();
        SetMapType();
        InitMap();
    }

    // �� ���� �ʱ�ȭ
    public void InitMap()
    {
        curMap = null;
        bossCount = defaultBossCount;
        bossCountTxt.text = bossCount.ToString();
    }

    // �� �����ְ� ���ϴ� ����
    public void OpenMap(bool enable, float time = 0.5f, bool first = false)
    {
        //playerFollowCam.State cameraState
        if (enable)
        {
            SetAllInteracteble(false);
            ZoomCamera(5, true);
            playerFollowCam.gameObject.SetActive(true);
            if (first)
            {
                SetPlayerStartPos(tiles[new Vector2(-1, mapGenerator.gridWidth - 1)]);
            }
            OpenMapPanel(true, first, time: time, OnComplete: () =>
            {
                StartCoroutine(PlayDirection(() =>
                {
                    ZoomCamera(3, time:0.65f, ease:Ease.OutQuad);
                }, first));
            });
        }
        else
        {
            SetAllInteracteble(false);
            playerFollowCam.gameObject.SetActive(false);
            OpenMapPanel(false, time: time, OnComplete: () =>
            {
                ZoomCamera(5, true);
            });
        }
    }

    // �� ������ ����
    public void OpenMapPanel(bool open, bool skip = false, float time = 0.5f, Action OnComplete = null)
    {
        List<CanvasGroup> cvsGroupList = new List<CanvasGroup>();
        cvsGroupList.Add(mapCvsGroup);
        cvsGroupList.Add(mapBGCvsGroup);

        mapOpenSequence.Kill();
        for (int i = 0; i < cvsGroupList.Count; i++)
        {
            int idx = i;
            CanvasGroup cvsGroup = cvsGroupList[idx];
            if (!skip)
            {
                if (!open)
                {
                    SetAllInteracteble(false);
                }
                mapOpenSequence = DOTween.Sequence()
                    .Append(cvsGroup.DOFade(open ? 1 : 0, time).OnComplete(() => {
                        SetAllInteracteble(open);
                        cvsGroup.blocksRaycasts = open;

                        if (idx == cvsGroupList.Count - 1)
                        {
                            OnComplete?.Invoke();
                        }
                    }));
            }
            else
            {
                cvsGroup.alpha = open ? 1 : 0;
                cvsGroup.interactable = open;
                cvsGroup.blocksRaycasts = open;

                if (idx == cvsGroupList.Count - 1)
                {
                    OnComplete?.Invoke();
                }
            }
        }
        //if (open)
        //{
        //    SoundHandler.Instance.PlayBGMSound("Battle_2");
        //}
    }

    // �÷��̾� �̵� ����
    public void MovePlayer(Map map, Action onComplete = null)
    {
        playerTrm.DOJump(map.transform.position, 0.35f, 1, 0.35f).SetEase(Ease.OutQuad).SetDelay(0.5f).OnComplete(() =>
        {
            BreakMap(curMap);
            curMap = map;
            SetInteractebleCanSelectMap();
            onComplete?.Invoke();
        });
    }

    // �ʿ����� �� �Ǳ� ���� �ؾߵ� ����
    public IEnumerator PlayDirection(Action onEndDirection, bool first = false)
    {
        //yield return new WaitForSeconds(0.23f);
        yield return null;

        if(first) // ��ó�� �μ����� �� ����
        {
            MovePlayer(tiles[new Vector2(0, mapGenerator.gridWidth-1)], ()=>
            {
                onEndDirection?.Invoke();
            });
        }
        else // ���� ī���� ����, ����� �� ������ �������鼭 �׾�ߵ�
        {
            if(CheckHasLinckedMap())
            {
                BossCountDirection(onEndDirection);
            }
            else
            {
                CanNotMoveGameOverDirection();
            }
        }
    }

    // �̵��� ���� ������ ������ �״� ����
    public void CanNotMoveGameOverDirection()
    {
        BreakMap(curMap);
    }

    // ���� �ʿ��� ����� ���� �ִ��� Ȯ��
    public bool CheckHasLinckedMap()
    {
        return curMap.linkedMoveAbleMap.Count > 0;
    }

    public void BossCountDirection(Action onEndDirection)
    {
        if(bossCount-- > 0)
        {
            DOTween.Sequence()
                .Append(bossCountTxt.DOText(bossCount.ToString(), 0.5f))
                .OnComplete(()=>
                {
                    onEndDirection?.Invoke();
                });
        }
        else
        {
            DOTween.Sequence()
                .Append(playerTrm.DORotate(new Vector3(playerTrm.rotation.x, playerTrm.rotation.y-360f*10, playerTrm.rotation.z), 1.3f).SetEase(Ease.OutCubic))
                .Join(playerTrm.DOMoveY(playerTrm.position.y + 8f, 1f).SetEase(Ease.OutCubic).SetDelay(0.3f))
                .OnComplete(() =>
                {
                    StartMap(mapNode.BOSS);
                    //SetPlayerStartPos(curMap);
                });
        }
    }


    // �پ��ִ� �ʼ��� ����
    public void LinkMap()
    {
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            Vector2 mapPos = GetTilesKeyToValue(mapList[i]);
            IEnumerable<Map> qry = from tile in tiles
                      where (tile.Key - mapPos).x >= -1 && (tile.Key - mapPos).x <= 1 && (tile.Key - mapPos).y >= -1 && (tile.Key - mapPos).y <= 1 && (tile.Key - mapPos).x+ (tile.Key - mapPos).y != 0
                      select tile.Value;
            mapList[i].linkedMoveAbleMap.AddRange(qry);
        }
    }

    // ���� �Ұ����� �� ó���ؼ� ������ �� Ÿ�� ��ȯ
    private mapNode GetCanSetType(Map map)
    {
        mapNode mapType;
        int rand = Random.Range(0, 100);
        bool canNotSet = false;

        do
        {
            canNotSet = false;
            rand = Random.Range(0, 100);
            mapType = GetMapType(rand);
            if(canNotLinkMapType.Contains(mapType))
            {
                for (int i = 0; i < map.linkedMoveAbleMap.Count; i++)
                {
                    if(map.linkedMoveAbleMap[i].MapType == mapType)
                    {
                        canNotSet = true;
                        break;
                    }
                }
            }
        } while (canNotSet);

        return mapType;
    }

    // ���� ���� �ش�Ǵ� �� Ÿ�� ��ȯ
    private mapNode GetMapType(int randIdx)
    {
        mapNode mapType = mapNode.NONE;
        if (randIdx < 12)
        {
            mapType = mapNode.REST;
        }
        else if (randIdx < 34)
        {
            mapType = mapNode.RandomEncounter;
        }
        else if (randIdx < 42)
        {
            mapType = mapNode.SHOP;
        }
        else if (randIdx < 50)
        {
            mapType = mapNode.EMONSTER;
        }
        else
        {
            mapType = mapNode.MONSTER;
        }
        return mapType;
    }

    // ��� �ʿ� Ÿ�� ����
    public void SetMapType()
    {
        //TODO ���� ���� �� Ÿ�� ����
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            Vector2 mapPos = GetTilesKeyToValue(mapList[i]);
            if (mapPos.Equals(new Vector2(-1f, 4f)) || mapPos.Equals(new Vector2(0f, 4f)))
            {
                mapList[i].MapType = mapNode.NONE;
            }
            else
            {
                mapList[i].MapType = GetCanSetType(mapList[i]);
            }
        }
    }

    // �� Ÿ�Կ� �´� ��������Ʈ ��ȯ
    public Sprite GetMapIcon(mapNode mapType)
    {
        int spriteIdx = -1;
        switch (mapType)
        {
            case mapNode.NONE:
                break;
            case mapNode.START:
                spriteIdx = 0;
                break;
            case mapNode.BOSS:
                //spriteIdx = 0;
                break;
            case mapNode.MONSTER:
                spriteIdx = 0;
                break;
            case mapNode.EMONSTER:
                spriteIdx = 1;
                break;
            case mapNode.SHOP:
                spriteIdx = 4;
                break;
            case mapNode.REST:
                spriteIdx = 3;
                break;
            case mapNode.RandomEncounter:
                spriteIdx = 2;
                break;
            default:
                break;
        }
        if (spriteIdx < 0)
            return null;
        return mapIconSpriteList[spriteIdx];
    }

    // ���� ������ ���� ��ġ �Ұ���
    public void SetAllInteracteble(bool enable)
    {
        mapCvsGroup.interactable = enable;
    }

    // ����� �ʸ� Ȱ��ȭ ���ִ°�
    public void SetInteractebleCanSelectMap()
    {
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            mapList[i].SetInteracteble(curMap.linkedMoveAbleMap.Contains(mapList[i]));
        }
    }

    // ���� �ʿ� ����� �ʵ� �����Ѱ� �����ϰ�, ���� ����
    public void UnSelectedLinkedMap(Map selectedMap)
    {
        for (int i = 0; i < curMap.linkedMoveAbleMap.Count; i++)
        {
            Map map = curMap.linkedMoveAbleMap[i];
            if(map != selectedMap)
                map.OnSelected(false);
        }
    }

    // ���丮���� �ʿ� �ش�Ǵ� Ű���� ��ȯ
    private Vector2 GetTilesKeyToValue(Map value)
    {
        return tiles.FirstOrDefault(x => x.Value == value).Key;
    }

    // �� �μ����� ����
    public void BreakMap(Map map)
    {
        if (map == null)
            return;
        map.SetInteracteble(false);
        for (int i = 0; i < map.linkedMoveAbleMap.Count; i++)
        {
            map.linkedMoveAbleMap[i].linkedMoveAbleMap.Remove(map);
        }
        Vector2 mapKey = GetTilesKeyToValue(map);
        tiles.Remove(mapKey);

        Vector2 mapPosition = map.transform.position;

        DOTween.Sequence()
            .Append(map.GetComponent<Image>().DOFade(0, 0.5f))
            .Join(map.transform.DOMoveY(map.transform.position.y - 0.5f, 0.5f))
            .OnComplete(() =>
            {
                RectTransform rect = Instantiate(brokenTile, mapPosition, Quaternion.identity, mapGenerator.transform).GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 0f);
                Destroy(map.gameObject);
            });
    }

    // ī�޶� �ܵǴ� ����
    public void ZoomCamera(float orthographicSize, bool skip = false, float time = 0.5f, Ease ease = Ease.Unset, Action onComplete = null)
    {
        if (!skip)
        {
            zoomTween.Kill();
            zoomTween = DOTween.To(() => playerFollowCam.m_Lens.OrthographicSize, x => playerFollowCam.m_Lens.OrthographicSize = x, orthographicSize, time).SetEase(ease)
                .OnComplete(()=>
                {
                    onComplete?.Invoke();
                });
        }
        else
        {
            playerFollowCam.m_Lens.OrthographicSize = orthographicSize;
            onComplete?.Invoke();
        }
    }
}
