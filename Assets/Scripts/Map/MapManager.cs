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

    [Header("맵 생성 관현")]
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] CanvasGroup mapCvsGroup;
    [SerializeField] CanvasGroup mapBGCvsGroup;
    [SerializeField] Transform playerTrm;
    [SerializeField] CinemachineVirtualCamera playerFollowCam;
    [SerializeField] Text bossCountTxt;

    [SerializeField] GameObject brokenTile;
    [SerializeField] List<Sprite> mapIconSpriteList = new List<Sprite>();

    [Header("맵 밸런스 관련")]
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

    // 맵 시작
    public void StartMap(mapNode mapType)
    {
        encounterHandler.StartEncounter(mapType);
    }

    // 맵 만들어지고 맨처음에 이동
    public void SetPlayerStartPos(Map map)
    {
        curMap = map;
        SetInteractebleCanSelectMap();
        playerTrm.position = map.transform.position;
    }

    // 맵 만들어지고 해야될거
    public void OnGenerateMap()
    {
        LinkMap();
        SetMapType();
        InitMap();
    }

    // 맵 변수 초기화
    public void InitMap()
    {
        curMap = null;
        bossCount = defaultBossCount;
        bossCountTxt.text = bossCount.ToString();
    }

    // 맵 열어주고 줌하는 연출
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

    // 맵 열리는 연출
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

    // 플레이어 이동 연출
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

    // 맵열리고 줌 되기 전에 해야될 연출
    public IEnumerator PlayDirection(Action onEndDirection, bool first = false)
    {
        //yield return new WaitForSeconds(0.23f);
        yield return null;

        if(first) // 맨처음 부셔지는 맵 연출
        {
            MovePlayer(tiles[new Vector2(0, mapGenerator.gridWidth-1)], ()=>
            {
                onEndDirection?.Invoke();
            });
        }
        else // 보스 카운팅 연출, 연결된 맵 없을시 떨어지면서 죽어야됨
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

    // 이동할 맵이 없으면 떨어져 죽는 연출
    public void CanNotMoveGameOverDirection()
    {
        BreakMap(curMap);
    }

    // 현제 맵에서 연결된 맵이 있는지 확인
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


    // 붙어있는 맵서로 연결
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

    // 연결 불가능한 맵 처리해서 랜덤한 맵 타입 반환
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

    // 랜덤 값에 해당되는 맵 타입 반환
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

    // 모든 맵에 타입 세팅
    public void SetMapType()
    {
        //TODO 고정 갯수 맵 타입 생성
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

    // 맵 타입에 맞는 스프라이트 반환
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

    // 맵의 관련한 모든거 터치 불가능
    public void SetAllInteracteble(bool enable)
    {
        mapCvsGroup.interactable = enable;
    }

    // 연결된 맵만 활성화 해주는거
    public void SetInteractebleCanSelectMap()
    {
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            mapList[i].SetInteracteble(curMap.linkedMoveAbleMap.Contains(mapList[i]));
        }
    }

    // 현재 맵에 연결된 맵들 선택한거 제외하고, 선택 해제
    public void UnSelectedLinkedMap(Map selectedMap)
    {
        for (int i = 0; i < curMap.linkedMoveAbleMap.Count; i++)
        {
            Map map = curMap.linkedMoveAbleMap[i];
            if(map != selectedMap)
                map.OnSelected(false);
        }
    }

    // 디렉토리에서 맵에 해당되는 키값을 반환
    private Vector2 GetTilesKeyToValue(Map value)
    {
        return tiles.FirstOrDefault(x => x.Value == value).Key;
    }

    // 맵 부셔지는 연출
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

    // 카메라 줌되는 연출
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
