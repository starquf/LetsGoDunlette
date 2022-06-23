using CustomDic;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum mapNode
{
    NONE = 0,
    START = 1,
    BOSS = 2,
    EMONSTER = 3,
    MONSTER = 4,
    SHOP = 5,
    REST = 6,
    RANDOMENCOUNTER = 7,

    TELEPORT = 10,
    SWITCH = 11,
}

public enum mapTileEvent
{
    NONE = 0,
    BLINK = 1,
    TIMELIMIT = 2,
}

public enum moveType
{
    JUMP = 0,
    TELEPORT = 1,
}

public enum DicType
{
    FIXEDPOS = 0,
    FIXEDRANDOMPOS = 1,
}

[Serializable]
public class FixedMap
{
    public mapNode mapType;
    public int teleportTargetIdx;
}

[Serializable]
public class FixedMapRangeRandom
{
    public Vector2 minPos;
    public Vector2 maxPos;
    public FixedMap map;
}

[Serializable]
public class FixedSpecialMapRangeRandom
{
    public List<FixedMapRangeRandom> teleportMapList;
    public FixedMapRangeRandom switchMap;
    public int randomVlaue;
}

[Serializable]
public class FixedTile
{
    public mapTileEvent mapType;
    public int limitTime;
}

[Serializable]
public class FixedTileRangeRandom
{
    public Vector2 minPos;
    public Vector2 maxPos;
    public FixedTile tile;
}

public class MapManager : MonoBehaviour
{
    private Sequence mapOpenSequence;

    private Vector2 defaultBossCloudPos;
    private Vector2 defaultBossEffectPos;

    [HideInInspector] public Dictionary<Vector2, Map> tiles;
    [HideInInspector] public Map curMap;
    private int bossCount;

    [Header("맵 생성 관현")]
    public MapGenerator mapGenerator;
    [SerializeField] private CanvasGroup mapCvsGroup;
    [SerializeField] private CanvasGroup mapBGCvsGroup;
    [SerializeField] private Image blockPanel;
    [SerializeField] private Transform bossCloudTrm;
    [SerializeField] private Transform bossEffectTrm;
    [SerializeField] private Transform playerTrm;
    [SerializeField] private MapCanvasFollow mapCvsFollow;
    [SerializeField] private Text bossCountTxt;

    [SerializeField] private List<Sprite> mapIconSpriteList = new List<Sprite>();
    [SerializeField] private List<Sprite> tileSpriteList = new List<Sprite>();

    [SerializeField] private Image mapBg;
    [SerializeField] private List<Sprite> mapBgSpriteList = new List<Sprite>();

    private Image bossCloudImage;
    [SerializeField] private List<Sprite> bossCloudSpriteList = new List<Sprite>();
    [SerializeField] private Animator bossEffectAnimator;

    [Header("맵 밸런스 관련")]
    [SerializeField] private int defaultBossCount;
    [SerializeField] private List<mapNode> canNotLinkMapType = new List<mapNode>();
    [SerializeField] private List<mapTileEvent> canNotLinkTileType = new List<mapTileEvent>();
    //[SerializeField] SerializableDictionary<mapNode, int> fixedMapTypeCount = new SerializableDictionary<mapNode, int>();
    private Dictionary<Vector2, FixedMap> useFixedPosMapType = new Dictionary<Vector2, FixedMap>();
    [SerializeField] private SerializableDictionary<Vector2, FixedMap> fixedPosMapType = new SerializableDictionary<Vector2, FixedMap>();
    [SerializeField] private List<FixedMapRangeRandom> fixedRangeMapType = new List<FixedMapRangeRandom>();
    [SerializeField] private List<FixedSpecialMapRangeRandom> fixedRangeSpecialMapType = new List<FixedSpecialMapRangeRandom>();
    [SerializeField] private SerializableDictionary<mapNode, float> mapTypeProportionDic = new SerializableDictionary<mapNode, float>();
    [SerializeField] private SerializableDictionary<mapTileEvent, float> tileTypeProportionDic = new SerializableDictionary<mapTileEvent, float>();
    [HideInInspector] public List<Map> blinkMapList = new List<Map>();
    [HideInInspector] public List<Map> timeLimitMapList = new List<Map>();
    [HideInInspector] public List<Map> switchEnableMapList = new List<Map>();

    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private int minLinkedMap = 3;
    [SerializeField] private int maxDestroyCount = 5;

    private EncounterHandler encounterHandler;
    private BattleHandler battleHandler;
    private BattleInfoHandler battleInfoHandler;

    private void Awake()
    {
        GameManager.Instance.mapManager = this;
        tiles = new Dictionary<Vector2, Map>();
        bossCloudImage = bossCloudTrm.GetChild(0).GetComponent<Image>();
        defaultBossCloudPos = bossCloudTrm.position;
        defaultBossEffectPos = bossEffectTrm.position;
    }

    private void Start()
    {
        encounterHandler = GameManager.Instance.encounterHandler;
        battleHandler = GameManager.Instance.battleHandler;
        battleInfoHandler = battleHandler.GetComponent<BattleInfoHandler>();
        mapGenerator.GenerateGrid(gridHeight, gridWidth, OnGenerateMap);
        GameManager.Instance.OnNextStage += () =>
        {
            ResetMap();
            mapGenerator.GenerateGrid(gridHeight, gridWidth, OnGenerateMap);
        };
    }
    
    public void ResetMap()
    {
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            mapList[i].gameObject.SetActive(false);
        }
        tiles.Clear();

        useFixedPosMapType.Clear();
        /*
        List<Vector2> fixedPosMapList = fixedPosMapType.Keys.ToList();
        for (int i = 0; i < fixedPosMapList.Count; i++)
        {
            mapNode mapType = fixedPosMapType[fixedPosMapList[i]];
            if (fixedMapTypeCount.Keys.Contains(mapType))
            {
                fixedMapTypeCount[mapType]++;
            }
        }*/
    }

    public void SetAllBlink()
    {
        for (int i = 0; i < blinkMapList.Count; i++)
        {
            blinkMapList[i].BlinkMap();
        }
    }

    // 변경되는 맵 타일 이벤트 순차 실행
    public IEnumerator ChageMapDeration(bool skip = false ,Action onEnd = null)
    {
        if (skip)
        {
            onEnd?.Invoke();
            yield break;
        }
        bool isEnd = false;

        int count = 1;

        for (int i = timeLimitMapList.Count-1; i >= 0; i--)
        {
            Map map = timeLimitMapList[i];
            if (curMap == map)
            {
                timeLimitMapList.Remove(map);
                continue;
            }
            mapCvsFollow.targetTrm = map.transform;
            float speed = 1f + Mathf.Clamp((float)count, 0f, 3f);

            isEnd = false;
            mapCvsFollow.Follow(speed, onEndAnim: () => {
                map.UpdateLimitTime(time: 0.5f / speed, onEnd: () =>
                {
                    isEnd = true;
                });
            });
            yield return new WaitUntil(()=>isEnd);
            count++;
        }
        count = 1;
        for (int i = blinkMapList.Count-1; i >=0 ; i--)
        {
            Map map = blinkMapList[i];
            if (curMap == map)
            {
                blinkMapList.Remove(map);
                continue;
            }
            mapCvsFollow.targetTrm = map.transform;
            float speed = 1f + Mathf.Clamp((float)count, 0f, 3f);

            isEnd = false;
            mapCvsFollow.Follow(speed, onEndAnim: () => {
                map.BlinkMap(0.7f/speed,onEndEvent: () => { isEnd = true; });
            });
            yield return new WaitUntil(() => isEnd);
            count++;
        }

        if (mapCvsFollow.targetTrm != playerTrm)
        {
            mapCvsFollow.targetTrm = playerTrm;
            isEnd = false;
            mapCvsFollow.Follow(onEndAnim:()=> { isEnd = true; });
            yield return new WaitUntil(() => isEnd);
        }
        onEnd?.Invoke();
    }

    // 변경되는 맵 타일 이벤트 순차 실행
    public IEnumerator OnSwitchMap(Action onEnd = null)
    {
        bool isEnd = false;


        StartCoroutine( RandomDestroyMap(3, true, ()=> isEnd = true));
        yield return new WaitUntil(() => isEnd);

        int count = 1;

        for (int i = switchEnableMapList.Count - 1; i >= 0; i--)
        {
            Map map = switchEnableMapList[i];

            mapCvsFollow.targetTrm = map.transform;
            float speed = 1f + Mathf.Clamp((float)count, 0f, 3f);

            isEnd = false;
            mapCvsFollow.Follow(speed, onEndAnim: () => {
                StartCoroutine(SetActiveMap(map, true, 0.5f / speed, () => isEnd = true));
            });
            yield return new WaitUntil(() => isEnd);
            count++;
        }

        if (mapCvsFollow.targetTrm != playerTrm)
        {
            mapCvsFollow.targetTrm = playerTrm;
            isEnd = false;
            mapCvsFollow.Follow(onEndAnim: () => { isEnd = true; });
            yield return new WaitUntil(() => isEnd);
        }
        onEnd?.Invoke();
    }


    public IEnumerator SetActiveMap(Map map, bool enable, float time, Action onEnd)
    {
        map.gameObject.SetActive(enable);
        yield return new WaitForSeconds(time);
        onEnd?.Invoke();
    }

    // 맵 시작
    public void StartMap(mapNode mapType)
    {
        StartCoroutine(ChageMapDeration( mapType == mapNode.BOSS,()=>
        {
            switch (mapType)
            {
                case mapNode.TELEPORT:
                    MovePlayer(curMap.teleportMap, null, false, moveType.TELEPORT);
                    break;
                case mapNode.SWITCH:
                    StartCoroutine(OnSwitchMap(() =>
                    {
                        SetAllInteracteble(true);
                        SetInteractebleCanSelectMap();
                    }));
                    break;
                default:
                    ZoomCamera(15f, time: 0.7f, ease: Ease.InOutSine);
                    encounterHandler.StartEncounter(mapType);
                    break;
            }
        }));

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
        CopyFixed();
        LinkMap();
        RandomRangeSpecialMapSet();
        RandomRangeMapSet();
        StartCoroutine(RandomDestroyMap());
        SetTileType();
        SetRandomTileSprite();
        SetMapType();
        InitMap();
    }

    private void CopyFixed()
    {
        useFixedPosMapType = new Dictionary<Vector2, FixedMap>(fixedPosMapType);
        blinkMapList = new List<Map>();
        timeLimitMapList = new List<Map>();
        switchEnableMapList = new List<Map>();
    }

    // 맵 변수 초기화
    public void InitMap()
    {
        curMap = null;
        bossCount = defaultBossCount;
        bossCountTxt.text = bossCount.ToString();
        Debug.Log("Stage :" + GameManager.Instance.StageIdx);
        bossCloudImage.sprite = bossCloudSpriteList[GameManager.Instance.StageIdx];
        bossEffectAnimator.SetInteger("Stage", GameManager.Instance.StageIdx);
        //SetAllBlink();
        SetBossAndMapBG();
    }

    private void SetBossAndMapBG()
    {
        BattleInfo bInfo = battleInfoHandler.GetRandomBossInfo();
        battleHandler._bossInfo = bInfo;

        int spriteIdx = -1;

        switch (bInfo.enemyInfos[0])
        {
            case EnemyType.QUEEN:
                spriteIdx = 0;
                break;
            case EnemyType.REDFOX:
                spriteIdx = 1;
                break;
            case EnemyType.TAROS:
                spriteIdx = 2;
                break;
            case EnemyType.WOODENDOLL:
                spriteIdx = 3;
                break;
            case EnemyType.SARO:
            case EnemyType.GAR:
            case EnemyType.DNAM:
                spriteIdx = 4;
                break;
            default:
                break;
        }
        if(spriteIdx<0)
        {
            Debug.LogError("해당 보스의 스프라이트가 설정되지 않았습니다.");
        }

        mapBg.sprite = mapBgSpriteList[spriteIdx];
    }

    public void SetTileType()
    {
        List<Map> mapList = tiles.Values.ToList();
        for (int i = mapList.Count-1; i >= 0; i--)
        {
            Map map = mapList[Random.Range(0, mapList.Count)];
            mapList.Remove(map);

            if (map.MapType == mapNode.TELEPORT)
            {
                continue;
            }

            Vector2 mapPos = GetTilesKeyToValue(map);
            map.tileType = mapPos.Equals(new Vector2(-1f, gridHeight - 1)) || mapPos.Equals(new Vector2(0f, gridHeight - 1))
                ? mapTileEvent.NONE
                : GetCanSetTileType(map);

            switch (map.tileType)
            {
                case mapTileEvent.BLINK:
                    blinkMapList.Add(map);
                    break;
                case mapTileEvent.TIMELIMIT:
                    timeLimitMapList.Add(map);
                    break;
                default:
                    break;
            }
        }
        //for (int i = 0; i < fixedRangeTileType.Count; i++)
        //{
        //    FixedTileRangeRandom fm = fixedRangeTileType[i];
        //    Vector2 targetPos;
        //    bool isFixedMapAndTeleport = false;
        //    do
        //    {
        //        isFixedMapAndTeleport = false;
        //        int targetX = Random.Range((int)fm.minPos.x, (int)fm.maxPos.x + 1);
        //        int targetY = Random.Range((int)fm.minPos.y, (int)fm.maxPos.y + 1);
        //        targetPos = new Vector2(targetX, targetY);
        //        if (useFixedPosMapType.ContainsKey(targetPos))
        //            isFixedMapAndTeleport = true;
        //    } while (targetPos == new Vector2(0, gridHeight - 1) || targetPos == new Vector2(-1, gridHeight - 1) || isFixedMapAndTeleport || 
        //    useFixedPosTileType.Keys.Contains(targetPos) || !tiles[targetPos].gameObject.activeSelf);

        //    useFixedPosTileType.Add(targetPos, new FixedTile() { mapType = fm.tile.mapType, limitTime= fm.tile.limitTime }) ;
        //}

        //List<Vector2> mapTileEventList = useFixedPosTileType.Keys.ToList();
        //for (int i = 0; i < mapTileEventList.Count; i++)
        //{
        //    switch (useFixedPosTileType[mapTileEventList[i]].mapType)
        //    {
        //        case mapTileEvent.NONE:
        //            break;
        //        case mapTileEvent.BLINK:
        //            blinkMapList.Add(tiles[mapTileEventList[i]]);
        //            break;
        //        case mapTileEvent.TIMELIMIT:
        //            int limitTime = useFixedPosTileType[mapTileEventList[i]].limitTime;
        //            if(limitTime<=0)
        //            {
        //                Debug.LogError("타임리미트 타일의 리미트타임 설정이 0 보다 작습니다.");
        //                continue;
        //            }
        //            Map map = tiles[mapTileEventList[i]];
        //            map.SetLimitTime(limitTime);
        //            timeLimitMapList.Add(map);
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

    public void RandomRangeSpecialMapSet()
    {
        Dictionary<int, int> teleportDic = new Dictionary<int, int>();
        for (int i = 0; i < fixedRangeSpecialMapType.Count; i++)
        {
            FixedSpecialMapRangeRandom fsm = fixedRangeSpecialMapType[i];

            int random = Random.Range(0, 100);
            if(random < fsm.randomVlaue)
            {
                //teleport
                for (int j = 0; j < fsm.teleportMapList.Count; j++)
                {
                    FixedMapRangeRandom fm = fsm.teleportMapList[j];
                    Vector2 targetPos;
                    int idx;
                    do
                    {
                        int targetX = Random.Range((int)fm.minPos.x, (int)fm.maxPos.x + 1);
                        int targetY = Random.Range((int)fm.minPos.y, (int)fm.maxPos.y + 1);
                        targetPos = new Vector2(targetX, targetY);
                    } while (useFixedPosMapType.Keys.Contains(targetPos));

                    idx = useFixedPosMapType.Count;
                    int k = j;
                    if (fm.map.teleportTargetIdx > k)
                    {
                        teleportDic.Add(k, idx);
                    }
                    else if (fm.map.teleportTargetIdx == idx)
                    {
                        Debug.LogError("자신에게 텔포 위치 되어있음");
                    }
                    else
                    {
                        if (teleportDic.ContainsKey(fm.map.teleportTargetIdx))
                        {
                            List<Vector2> fixedPosMapList = useFixedPosMapType.Keys.ToList();
                            idx = teleportDic[fm.map.teleportTargetIdx];
                            useFixedPosMapType[fixedPosMapList[idx]].teleportTargetIdx = useFixedPosMapType.Count;
                            teleportDic.Remove(fm.map.teleportTargetIdx);
                        }
                        else
                        {
                            Debug.LogError("텔레포트 랜덤 좌표 설정 idx가 잘못되었습니다");
                        }
                    }
                    FixedMap fixedMap = new FixedMap() { mapType = fm.map.mapType, teleportTargetIdx = idx };

                    useFixedPosMapType.Add(targetPos, fixedMap);
                }
            }
            else
            {
                //switch
                FixedMapRangeRandom fm = fsm.switchMap;
                Vector2 targetPos;
                do
                {
                    int targetX = Random.Range((int)fm.minPos.x, (int)fm.maxPos.x + 1);
                    int targetY = Random.Range((int)fm.minPos.y, (int)fm.maxPos.y + 1);
                    targetPos = new Vector2(targetX, targetY);
                } while (useFixedPosMapType.Keys.Contains(targetPos));

                useFixedPosMapType.Add(targetPos, fm.map);
            }
        }
    }

    public void RandomRangeMapSet()
    {
        Dictionary<int, int> teleportDic = new Dictionary<int, int>();
        for (int i = 0; i < fixedRangeMapType.Count; i++)
        {
            FixedMapRangeRandom fm = fixedRangeMapType[i];
            Vector2 targetPos;
            int idx;
            do
            {
                int targetX = Random.Range((int)fm.minPos.x, (int)fm.maxPos.x + 1);
                int targetY = Random.Range((int)fm.minPos.y, (int)fm.maxPos.y + 1);
                targetPos = new Vector2(targetX, targetY);
            } while (useFixedPosMapType.Keys.Contains(targetPos));

            if(fm.map.mapType == mapNode.TELEPORT)
            {
                idx = useFixedPosMapType.Count;
                int j = i;
                if (fm.map.teleportTargetIdx > j)
                {
                    teleportDic.Add(j, idx);
                }
                else if(fm.map.teleportTargetIdx == idx)
                {
                    Debug.LogError("자신에게 텔포 위치 되어있음");
                }
                else
                {
                    if (teleportDic.ContainsKey(fm.map.teleportTargetIdx))
                    {
                        List<Vector2> fixedPosMapList = useFixedPosMapType.Keys.ToList();
                        idx = teleportDic[fm.map.teleportTargetIdx];
                        useFixedPosMapType[fixedPosMapList[idx]].teleportTargetIdx = useFixedPosMapType.Count;
                        teleportDic.Remove(fm.map.teleportTargetIdx);
                    }
                    else
                    {
                        Debug.LogError("텔레포트 랜덤 좌표 설정 idx가 잘못되었습니다");
                    }
                }
                FixedMap fixedMap = new FixedMap() { mapType = fm.map.mapType, teleportTargetIdx = idx };

                useFixedPosMapType.Add(targetPos, fixedMap);
            }
            else
            {
                useFixedPosMapType.Add(targetPos, fm.map);
            }
        }
    }

    // 맵 구조 변경
    public IEnumerator RandomDestroyMap(int count = -1, bool breakAnim = false, Action onEnd = null)
    {
        if (count < 0)
            count = maxDestroyCount;

        List<Map> mapList = tiles.Values.ToList();


        for (int i = 0; i < switchEnableMapList.Count; i++)
        {
            mapList.Remove(switchEnableMapList[i]);
        }

        List<Vector2> fixedPosMapList = useFixedPosMapType.Keys.ToList();
        for (int i = 0; i < fixedPosMapList.Count; i++)
        {
            mapList.Remove(tiles[fixedPosMapList[i]]);
        }

        int defaultCount = count+1;

        int mapCount = mapList.Count;
        for (int i = 0; i < mapCount && count > 0; i++)
        {
            Map map = mapList[Random.Range(0, mapList.Count)];
            mapList.Remove(map);
            if (GetTilesKeyToValue(map) != new Vector2(0, gridHeight - 1) && GetTilesKeyToValue(map) != new Vector2(-1, gridHeight - 1))
            {
                if (CheckCanDestroy(map))
                {
                    count--;
                    if(breakAnim)
                    {
                        bool isEnd = false;
                        float speed = 1f + Mathf.Clamp((float)(defaultCount - count), 0f, 3f);

                        mapCvsFollow.targetTrm = map.transform;
                        mapCvsFollow.Follow(speed, onEndAnim: () => {
                            BreakMap(map, 0.7f / speed, () => { isEnd = true; });
                        });
                        yield return new WaitUntil(() => isEnd);
                        //BreakMap(map);
                    }
                    else
                    {
                        switchEnableMapList.Add(map);
                        //DestroyMap(map);
                        map.gameObject.SetActive(false);
                        //Destroy(map.gameObject);
                    }
                }
            }
        }
        onEnd?.Invoke();
        yield return null;
    }
    //  부술수 있는지 체크
    public bool CheckCanDestroy(Map map)
    {
        if (!CheckDestroyLinkMap(map))
        {
            return false;
        }

        for (int i = 0; i < map.linkedMoveAbleMap.Count; i++)
        {
            Map linkedMap = map.linkedMoveAbleMap[i];
            if (!IsLinkedAllNode(linkedMap, new List<Map>() { map }))
            {
                return false;
            }
        }
        return true;
    }

    // 최소 연결 갯수에 적합한지 체크
    private bool CheckDestroyLinkMap(Map map)
    {
        if (minLinkedMap < 1)
        {
            Debug.LogError("최소 연결 노드를 1이상으로 설정해야 합니다");
        }

        for (int j = 0; j < map.linkedMoveAbleMap.Count; j++)
        {
            Map linkedMap = map.linkedMoveAbleMap[j];
            if (linkedMap.linkedMoveAbleMap.Count - 1 <= minLinkedMap)
            {
                //print("이거 부수면 좆됨");
                return false;
            }
        }
        return true;
    }

    // 시작지점과 연결되어있는지 체크
    private bool IsLinkedAllNode(Map map, List<Map> checkedMapList)
    {
        for (int j = 0; j < map.linkedMoveAbleMap.Count; j++)
        {
            Map linkedMap = map.linkedMoveAbleMap[j];
            if (!checkedMapList.Contains(linkedMap))
            {
                checkedMapList.Add(linkedMap);
                if (linkedMap == (curMap == null ? tiles[new Vector2(0, gridHeight - 1)] : curMap))
                {
                    //print("모든 노드와 연결되어있음");
                    return true;
                }
                if (IsLinkedAllNode(linkedMap, checkedMapList))
                {
                    return true;
                }
            }
        }
        //print("모든 노드와 연결되어있지 않음");
        return false;
    }

    // 맵 열어주고 줌하는 연출
    public void OpenMap(bool enable, float time = 0.5f, bool first = false)
    {
        blockPanel.raycastTarget = false;
        if (enable)
        {
            SetAllInteracteble(false);
            ZoomCamera(1, true);
            //playerFollowCam.gameObject.SetActive(true);
            if (first)
            {
                SetInteractebleAllMap();
                SetPlayerStartPos(tiles[new Vector2(-1, gridHeight - 1)]);
            }
            OpenMapPanel(true, first, time: time, OnComplete: () =>
            {
                StartCoroutine(PlayDirection(() =>
                {
                    SetInteractebleCanSelectMap();
                    blockPanel.raycastTarget = false;
                }, first));
            });
        }
        else
        {
            SetAllInteracteble(false);
            //playerFollowCam.gameObject.SetActive(false);
            OpenMapPanel(false, time: time, OnComplete: () =>
            {
                ZoomCamera(1, true);
            });
        }
    }

    // 맵 열리는 연출
    public void OpenMapPanel(bool open, bool skip = false, float time = 0.5f, Action OnComplete = null)
    {
        playerTrm.localScale = Vector3.one;
        playerTrm.GetComponent<Image>().color = Color.white;
        List<CanvasGroup> cvsGroupList = new List<CanvasGroup>()
        {
            mapCvsGroup,
            mapBGCvsGroup
        };

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
                    .Append(cvsGroup.DOFade(open ? 1 : 0, time).OnComplete(() =>
                    {
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
        if (open)
        {
            SoundHandler.Instance.PlayBGMSound("Battle_2");
        }
    }

    // 플레이어 이동 연출
    public void MovePlayer(Map map, Action onComplete = null, bool counting = false, moveType type = moveType.JUMP)
    {
        SetInteractebleAllMap();
        if (counting)
        {
            bossCount--;
            bossCountTxt.DOText(bossCount.ToString(), 0.5f);
        }

        Action onEnd = () =>
        {
            BreakMap(curMap);
            curMap = map;
            curMap.mapIcon.DOFade(0, 0.3f);
            mapCvsFollow.Follow(onEndAnim: () =>
            {
                onComplete?.Invoke();
            });
        };

        switch (type)
        {
            case moveType.JUMP:
                DOTween.Sequence()
                    .Append(playerTrm.DOLocalJump(map.transform.localPosition, 35f, 1, 0.5f).SetEase(Ease.InQuad).SetDelay(counting ? 0.5f : 0.3f))
                    .Append(playerTrm.DOLocalMoveY(map.transform.localPosition.y + 40f, 0.08f))
                    .Append(playerTrm.DOLocalMoveY(map.transform.localPosition.y, 0.3f).SetEase(Ease.OutBounce))
                    .OnComplete(() =>
                    {
                        onEnd?.Invoke();
                    });
                break;
            case moveType.TELEPORT:
                playerTrm.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        playerTrm.localPosition = map.transform.localPosition;
                        mapCvsFollow.Follow(onEndAnim: () =>
                        {
                            playerTrm.DOScale(Vector3.one, 0.5f).SetEase(Ease.InQuad)
                            .OnComplete(() =>
                            {
                                BreakMap(curMap);
                                curMap = map;
                                curMap.mapIcon.DOFade(0, 0.3f);

                                SetAllInteracteble(true);
                                SetInteractebleCanSelectMap();
                            });
                        });
                    });
                break;
            default:
                Debug.LogError("잘못된 이동 타입으로 이동");
                break;
        }
    }

    // 맵열리고 줌 되기 전에 해야될 연출
    public IEnumerator PlayDirection(Action onEndDirection, bool first = false)
    {
        blockPanel.raycastTarget = true;
        yield return new WaitForSeconds(0.5f);
        //yield return null;

        if (first) // 맨처음 부셔지는 맵 연출
        {
            yield return new WaitForSeconds(0.7f);

            ZoomCamera(2f, time: 0.65f, ease: Ease.OutQuad, onComplete: () =>
            {
                MovePlayer(tiles[new Vector2(0, gridHeight - 1)], () =>
                {
                    onEndDirection?.Invoke();
                });
            });
        }
        else // 보스 카운팅 연출, 연결된 맵 없을시 떨어지면서 죽어야됨
        {
            if (bossCount == 0 || CheckHasLinckedMap())
            {
                BossCountDirection(onEndDirection);
            }
            else
            {
                bossCount = 0;
                BossCountDirection(onEndDirection);
                //CanNotMoveGameOverDirection();
            }
        }
    }

    // 이동할 맵이 없으면 떨어져 죽는 연출
    public void CanNotMoveGameOverDirection()
    {
        ZoomCamera(2f, time: 0.65f, ease: Ease.OutQuad);
        BreakMap(curMap);
        DOTween.Sequence()
            .AppendInterval(0.3f)
            .Append(playerTrm.DOMoveY(playerTrm.position.y - 5f, 1f).SetEase(Ease.InBack))
            .Join(playerTrm.GetComponent<Image>().DOFade(0, 1f).SetEase(Ease.InBack))
            .AppendInterval(0.7f)
            .OnComplete(() =>
            {
                //ToDO 게임 오버
                GameManager.Instance.ResetGame();
            });
    }

    // 현제 맵에서 연결된 맵이 있는지 확인
    public bool CheckHasLinckedMap()
    {
        for (int i = 0; i < curMap.linkedMoveAbleMap.Count; i++)
        {
            if(!curMap.linkedMoveAbleMap[i].isBlinked)
            {
                return true;
            }
        }
        return false;
        //return curMap.linkedMoveAbleMap.Count > 0;
    }

    // 보스 카운팅 연출 및 보스 올라가는 연출
    public void BossCountDirection(Action onEndDirection)
    {
        if (bossCount > 0)
        {
            ZoomCamera(2, time: 0.65f, ease: Ease.OutQuad, onComplete: () =>
            {
                onEndDirection?.Invoke();
            });
        }
        else
        {
            //playerFollowCam.gameObject.SetActive(false);
            mapCvsFollow.targetTrm = null;
            Vector2 bossCloudPos = new Vector2(0, bossEffectTrm.position.y);
            Vector2 dir = bossCloudPos - (Vector2)playerTrm.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(playerTrm.DORotate(new Vector3(playerTrm.rotation.x, playerTrm.rotation.y - (360f * 10), playerTrm.rotation.z), 1.3f).SetEase(Ease.OutCubic))
                //.Join(playerTrm.DOMove(bossCloudPos, 1f).SetDelay(0.3f).SetEase(Ease.OutCubic))
                .Join(playerTrm.DOPath(new Vector3[] { playerTrm.position, new Vector3(Random.Range(0, 2) < 1 ? -1 : 1 * Random.Range(0.5f, 1.5f), (bossCloudPos.y + playerTrm.position.y) / 2, 0), bossCloudPos }, 1f).SetEase(Ease.InCubic))
                //.Join(playerTrm.DORotateQuaternion(Quaternion.AngleAxis(angle - 90, Vector3.forward), 1f).SetDelay(0.3f).SetEase(Ease.OutCubic))
                .Append(playerTrm.DOScale(0, 0.5f))
                .Join(playerTrm.GetComponent<Image>().DOFade(0, 0.5f))
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
                                   where (tile.Key - mapPos).x >= -1 && (tile.Key - mapPos).x <= 1 && (tile.Key - mapPos).y >= -1 && (tile.Key - mapPos).y <= 1 && (tile.Key - mapPos).x + (tile.Key - mapPos).y != 0
                                   select tile.Value;
            mapList[i].linkedMoveAbleMap.AddRange(qry);
        }
    }

    private mapTileEvent GetCanSetTileType(Map map)
    {
        if(!map.gameObject.activeSelf)
        {
            return mapTileEvent.NONE;
        }
        Dictionary<mapTileEvent, float> curMapTypeProportionDic = GetTileTypeProportion();
        List<mapTileEvent> sortedTileTypeList = tileTypeProportionDic.Keys.ToList();

        sortedTileTypeList.Sort((x, y) => (tileTypeProportionDic[x] - curMapTypeProportionDic[x]) > (tileTypeProportionDic[y] - curMapTypeProportionDic[y]) ? -1 : 1);

        mapTileEvent tileType;
        int i = 0;
        do
        {
            if (i >= sortedTileTypeList.Count)
            {
                //Debug.LogError("비율 세팅이 외 안되누");
                return mapTileEvent.NONE;
            }
            tileType = sortedTileTypeList[i];
            i++;
        } while (!CanSetTileType(map, tileType));

        /*
        for (int j = 0; j < sortedTileTypeList.Count; j++)
        {
            mapTileEvent mt = sortedTileTypeList[j];
            //Debug.Log(mt + ":" + mapTypeProportionDic[mt].ToString() + ", " + curMapTypeProportionDic[mt].ToString());
        }
        //Debug.Log(mapType + "가 비율로 세팅됨");*/

        return tileType;
    }

    // 연결 불가능한 맵 처리해서 랜덤한 맵 타입 반환
    private mapNode GetCanSetType(Map map)
    {
        Dictionary<mapNode, float> curMapTypeProportionDic = GetMapTypeProportion();
        List<mapNode> sortedMapTypeList = mapTypeProportionDic.Keys.ToList();

        sortedMapTypeList.Sort((x, y) => (mapTypeProportionDic[x] - curMapTypeProportionDic[x]) > (mapTypeProportionDic[y] - curMapTypeProportionDic[y]) ? -1 : 1);

        mapNode mapType;
        int i = 0;
        do
        {
            if (i >= sortedMapTypeList.Count)
            {
                //Debug.LogError("비율 세팅이 외 안되누");
                return mapNode.NONE;
            }
            mapType = sortedMapTypeList[i];
            i++;
        } while (!CanSetType(map, mapType));

        for (int j = 0; j < sortedMapTypeList.Count; j++)
        {
            mapNode mt = sortedMapTypeList[j];
            //Debug.Log(mt + ":" + mapTypeProportionDic[mt].ToString() + ", " + curMapTypeProportionDic[mt].ToString());
        }
        //Debug.Log(mapType + "가 비율로 세팅됨");

        return mapType;
    }
    private bool CanSetTileType(Map map, mapTileEvent tileType)
    {
        if (canNotLinkTileType.Contains(tileType))
        {
            for (int i = 0; i < map.linkedMoveAbleMap.Count; i++)
            {
                if (map.linkedMoveAbleMap[i].tileType == tileType)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CanSetType(Map map, mapNode mapType)
    {
        if (canNotLinkMapType.Contains(mapType))
        {
            for (int i = 0; i < map.linkedMoveAbleMap.Count; i++)
            {
                if (map.linkedMoveAbleMap[i].MapType == mapType)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void SetRandomTileSprite()
    {
        List<Map> maps = tiles.Values.ToList();
        for (int i = 0; i < maps.Count; i++)
        {
            Map map = maps[i];
            int idx = 0;
            if (blinkMapList.Contains(map))
            {
                idx = 5;
            }
            else
            {
                idx = Random.Range(0, 6);
            }
            maps[i].SetTileSprite(tileSpriteList[idx  + (GameManager.Instance.StageIdx * 7)]);
        }
    }

    // 모든 맵에 타입 세팅
    public void SetMapType()
    {
        // 고정 좌표 맵 타일 설정
        List<Vector2> fixedPosMapList = useFixedPosMapType.Keys.ToList();
        for (int i = 0; i < fixedPosMapList.Count; i++)
        {
            mapNode mapType = useFixedPosMapType[fixedPosMapList[i]].mapType;
            /*
            if (fixedMapTypeCount.Keys.Contains(mapType))
            {
                fixedMapTypeCount[mapType]--;
            }*/
            Debug.Log($"{tiles[fixedPosMapList[i]].name}에 {mapType} 좌표 고정됨");
            if(mapType == mapNode.TELEPORT)
            {
                tiles[fixedPosMapList[i]].teleportMap = tiles[fixedPosMapList[useFixedPosMapType[fixedPosMapList[i]].teleportTargetIdx]];
            }
            tiles[fixedPosMapList[i]].MapType = mapType;
        }
        /*
        // 고정 갯수 맵 타일 설정
        List<mapNode> fixedMapTypeCountKeys = fixedMapTypeCount.Keys.ToList();
        fixedMapTypeCountKeys.Sort((x, y) => canNotLinkMapType.Contains(x) ? -1 : 1);
        for (int j = 0; j < fixedMapTypeCount.Count; j++)
        {
            List<Map> radomMapList = tiles.Values.ToList();

            for (int i = 0; i < fixedPosMapList.Count; i++)
            {
                radomMapList.Remove(tiles[fixedPosMapList[i]]);
                radomMapList.Remove(tiles[new Vector2(-1, gridHeight - 1)]);
                radomMapList.Remove(tiles[new Vector2(0, gridHeight - 1)]);
            }

            int mapCount = radomMapList.Count;

            mapNode mapType = fixedMapTypeCountKeys[j];
            int count = fixedMapTypeCount[mapType];

            for (int i = 0; i < mapCount && count > 0; i++)
            {
                Map map = radomMapList[Random.Range(0, radomMapList.Count)];
                radomMapList.Remove(map);
                if(CanSetType(map, mapType))
                {
                    Debug.Log($"{map.name}에 {mapType} 갯수 고정됨");
                    map.MapType = mapType;
                    count--;
                }
            }
        }*/

        // 랜덤 맵 타일 설정 <- 비율로 변경
        List<Map> mapList = tiles.Values.ToList();
        for (int i = mapList.Count-1; i >= 0 ; i--)
        {
            Map map = mapList[Random.Range(0, mapList.Count)];
            mapList.Remove(map);
            if (map.MapType != mapNode.NONE)
            {
                continue;
            }

            Vector2 mapPos = GetTilesKeyToValue(map);
            map.MapType = mapPos.Equals(new Vector2(-1f, gridHeight - 1)) || mapPos.Equals(new Vector2(0f, gridHeight - 1))
                ? mapNode.NONE
                : GetCanSetType(map);
        }
    }
    public Dictionary<mapTileEvent, float> GetTileTypeProportion()
    {
        Dictionary<mapTileEvent, float> tileTypeProportion = new Dictionary<mapTileEvent, float>();

        Dictionary<mapTileEvent, float> tileTypeCountDic = new Dictionary<mapTileEvent, float>();

        List<mapTileEvent> tileTypes = tileTypeProportionDic.Keys.ToList();
        for (int i = 0; i < tileTypes.Count; i++)
        {
            mapTileEvent tileType = tileTypes[i];
            tileTypeCountDic.Add(tileType, 0);
        }

        int count = 0;
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            Map map = mapList[i];
            if (map.tileType == mapTileEvent.NONE || map.tileType == mapTileEvent.BLINK)
            {
                tileTypeCountDic[map.tileType]++;
                count++;
            }
        }

        for (int i = 0; i < tileTypes.Count; i++)
        {
            mapTileEvent mapType = tileTypes[i];
            tileTypeProportion.Add(mapType, tileTypeCountDic[mapType] / count * 100);
        }
        return tileTypeProportion;
    }

    public Dictionary<mapNode, float> GetMapTypeProportion()
    {
        Dictionary<mapNode, float> mapTypeProportion = new Dictionary<mapNode, float>();

        Dictionary<mapNode, float> mapTypeCountDic = new Dictionary<mapNode, float>();

        List<mapNode> mapTypes = mapTypeProportionDic.Keys.ToList();
        for (int i = 0; i < mapTypes.Count; i++)
        {
            mapNode mapType = mapTypes[i];
            mapTypeCountDic.Add(mapType, 0);
        }

        int count = 0;
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            Map map = mapList[i];
            if (!(map.MapType == mapNode.NONE || map.MapType == mapNode.START || map.MapType == mapNode.BOSS || map.MapType == mapNode.TELEPORT|| map.MapType == mapNode.SWITCH))
            {
                mapTypeCountDic[map.MapType]++;
                count++;
            }
        }

        for (int i = 0; i < mapTypes.Count; i++)
        {
            mapNode mapType = mapTypes[i];
            mapTypeProportion.Add(mapType, mapTypeCountDic[mapType] / count * 100);
        }
        return mapTypeProportion;
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
                spriteIdx = 3;
                break;
            case mapNode.REST:
                spriteIdx = 2;
                break;
            case mapNode.RANDOMENCOUNTER:
                //spriteIdx = 2;
                break;
            case mapNode.TELEPORT:
                spriteIdx = 4;
                break;
            case mapNode.SWITCH:
                spriteIdx = 5;
                break;
            default:
                break;
        }
        return spriteIdx < 0 ? null : mapIconSpriteList[spriteIdx];
    }

    // 맵의 관련한 모든거 터치 불가능
    public void SetAllInteracteble(bool enable)
    {
        mapCvsGroup.interactable = enable;
        mapCvsGroup.blocksRaycasts = enable;
    }

    public void SetInteractebleAllMap()
    {
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            Color c = mapList[i].mapIcon.color;
            mapList[i].SetInteracteble(false);
            mapList[i].mapIcon.color = c.a == 0 ? c : mapList[i].mapIcon.color;
        }
    }

    // 연결된 맵만 활성화 해주는거
    public void SetInteractebleCanSelectMap()
    {
        for (int i = 0; i < curMap.linkedMoveAbleMap.Count; i++)
        {
            Map map = curMap.linkedMoveAbleMap[i];
            Color c = curMap.linkedMoveAbleMap[i].mapIcon.color;
            map.SetInteracteble(true);
            map.mapIcon.color = c.a == 0 ? c : map.mapIcon.color;
        }
    }

    // 현재 맵에 연결된 맵들 선택한거 제외하고, 선택 해제
    public void UnSelectedLinkedMap(Map selectedMap)
    {
        for (int i = 0; i < curMap.linkedMoveAbleMap.Count; i++)
        {
            Map map = curMap.linkedMoveAbleMap[i];
            if (map != selectedMap)
            {
                map.OnSelected(false);
            }
        }
    }

    // 디렉토리에서 맵에 해당되는 키값을 반환
    private Vector2 GetTilesKeyToValue(Map value)
    {
        return tiles.FirstOrDefault(x => x.Value == value).Key;
    }

    // 맵과 연결되있는 리스트에서 다빼줌 tiles에서도 빼줌
    private void DestroyMap(Map map)
    {
        map.SetInteracteble(false);
        for (int i = 0; i < map.linkedMoveAbleMap.Count; i++)
        {
            map.linkedMoveAbleMap[i].linkedMoveAbleMap.Remove(map);
        }
        Vector2 mapKey = GetTilesKeyToValue(map);
        tiles.Remove(mapKey);
        useFixedPosMapType.Remove(mapKey);
    }

    // 맵 부셔지는 연출
    public void BreakMap(Map map, float time = 0.5f, Action onEnd = null)
    {
        if (map == null)
        {
            return;
        }

        DestroyMap(map);
        Vector2 mapPosition = map.transform.position;

        DOTween.Sequence()
            .Append(map.GetComponent<CanvasGroup>().DOFade(0, time))
            .Join(map.transform.DOLocalMoveY(map.transform.localPosition.y - 0.5f, time))
            .OnComplete(() =>
            {
                map.gameObject.SetActive(false);
                //Destroy(map.gameObject);
                onEnd?.Invoke();
            });
    }

    // 카메라 줌되는 연출
    public void ZoomCamera(float zoomScale, bool skip = false, float time = 0.5f, Ease ease = Ease.Unset, Action onComplete = null)
    {
        mapCvsFollow.targetTrm = zoomScale <= 1 ? null : playerTrm;
        MoveBossCloud(zoomScale, time, ease, skip);
        //if (!skip)
        //{
        //    zoomTween.Kill();
        //    zoomTween = DOTween.To(() => playerFollowCam.m_Lens.OrthographicSize, x => playerFollowCam.m_Lens.OrthographicSize = x, orthographicSize, time).SetEase(ease)
        //        .OnComplete(() =>
        //        {
        //            onComplete?.Invoke();
        //        });
        //}
        //else
        //{
        //    playerFollowCam.m_Lens.OrthographicSize = orthographicSize;
        //    onComplete?.Invoke();
        //}

        mapCvsFollow.Follow(1f, () =>
        {
            mapCvsFollow.Zoom(zoomScale, skip, time, ease, onComplete);
        });
    }

    // 카메라 줌에 따라 움직이는 보스 구름
    public void MoveBossCloud(float zoomSize, float time, Ease ease, bool skip)
    {
        float movePower = zoomSize <= 1 ? 0 : 5;
        if (!skip)
        {
            DOTween.Sequence()
                .Append(bossCloudTrm.DOMoveY(defaultBossCloudPos.y + movePower, time).SetEase(ease))
                .Join(bossEffectTrm.DOMoveY(defaultBossEffectPos.y + movePower, time).SetEase(ease))
                ;
        }
        else
        {
            bossCloudTrm.position = new Vector3(bossCloudTrm.position.x, defaultBossCloudPos.y + movePower, bossCloudTrm.position.z);
            bossEffectTrm.position = new Vector3(bossEffectTrm.position.x, defaultBossEffectPos.y + movePower, bossCloudTrm.position.z);
        }
    }
}
