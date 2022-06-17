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
    RandomEncounter = 7,
}

[Serializable]
public class FixedMapRangeRandom
{
    public Vector2 minPos;
    public Vector2 maxPos;
    public mapNode mapType;
}

public class MapManager : MonoBehaviour
{
    private Sequence mapOpenSequence;
    private Tween zoomTween;

    private Vector2 defaultBossCloudPos;
    private Vector2 defaultBossEffectPos;

    [HideInInspector] public Dictionary<Vector2, Map> tiles;
    private Map curMap;
    private int bossCount;

    [Header("�� ���� ����")]
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

    private Image bossCloudImage;
    [SerializeField] private List<Sprite> bossCloudSpriteList = new List<Sprite>();
    [SerializeField] private Animator bossEffectAnimator;

    [Header("�� �뷱�� ����")]
    [SerializeField] private int defaultBossCount;
    [SerializeField] private List<mapNode> canNotLinkMapType = new List<mapNode>();
    //[SerializeField] SerializableDictionary<mapNode, int> fixedMapTypeCount = new SerializableDictionary<mapNode, int>();
    private Dictionary<Vector2, mapNode> useFixedPosMapType = new Dictionary<Vector2, mapNode>();
    [SerializeField] private SerializableDictionary<Vector2, mapNode> fixedPosMapType = new SerializableDictionary<Vector2, mapNode>();
    [SerializeField] private List<FixedMapRangeRandom> fixedRangeMapType = new List<FixedMapRangeRandom>();
    [SerializeField] private SerializableDictionary<mapNode, float> mapTypeProportionDic = new SerializableDictionary<mapNode, float>();

    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private int minLinkedMap = 3;
    [SerializeField] private int maxDestroyCount = 5;

    private EncounterHandler encounterHandler;

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
        CopyFixed();
        LinkMap();
        RandomDestroyMap();
        SetRandomTileSprite();
        SetMapType();
        InitMap();
    }

    private void CopyFixed()
    {
        useFixedPosMapType = new Dictionary<Vector2, mapNode>(fixedPosMapType);
    }

    // �� ���� �ʱ�ȭ
    public void InitMap()
    {
        curMap = null;
        bossCount = defaultBossCount;
        bossCountTxt.text = bossCount.ToString();
        Debug.Log("Stage :" + GameManager.Instance.StageIdx);
        bossCloudImage.sprite = bossCloudSpriteList[GameManager.Instance.StageIdx];
        bossEffectAnimator.SetInteger("Stage", GameManager.Instance.StageIdx);
    }

    public void RandomRangeMapSet()
    {
        for (int i = 0; i < fixedRangeMapType.Count; i++)
        {
            FixedMapRangeRandom fm = fixedRangeMapType[i];
            Vector2 targetPos;
            do
            {
                int targetX = Random.Range((int)fm.minPos.x, (int)fm.maxPos.x + 1);
                int targetY = Random.Range((int)fm.minPos.y, (int)fm.maxPos.y + 1);
                targetPos = new Vector2(targetX, targetY);
            } while (useFixedPosMapType.Keys.Contains(targetPos));

            useFixedPosMapType.Add(targetPos, fm.mapType);
        }
    }

    // �� ���� ����
    public void RandomDestroyMap(int count = -1, bool breakAnim = false)
    {
        RandomRangeMapSet();

        if (count < 0)
            count = maxDestroyCount;

        List<Map> mapList = tiles.Values.ToList();

        List<Vector2> fixedPosMapList = useFixedPosMapType.Keys.ToList();
        for (int i = 0; i < fixedPosMapList.Count; i++)
        {
            mapList.Remove(tiles[fixedPosMapList[i]]);
        }

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
                        BreakMap(map);
                    }
                    else
                    {
                        DestroyMap(map);
                        map.gameObject.SetActive(false);
                        //Destroy(map.gameObject);
                    }
                }
            }
        }
    }
    //  �μ��� �ִ��� üũ
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

    // �ּ� ���� ������ �������� üũ
    private bool CheckDestroyLinkMap(Map map)
    {
        if (minLinkedMap < 1)
        {
            Debug.LogError("�ּ� ���� ��带 1�̻����� �����ؾ� �մϴ�");
        }

        for (int j = 0; j < map.linkedMoveAbleMap.Count; j++)
        {
            Map linkedMap = map.linkedMoveAbleMap[j];
            if (linkedMap.linkedMoveAbleMap.Count - 1 <= minLinkedMap)
            {
                //print("�̰� �μ��� ����");
                return false;
            }
        }
        return true;
    }

    // ���������� ����Ǿ��ִ��� üũ
    private bool IsLinkedAllNode(Map map, List<Map> checkedMapList)
    {
        for (int j = 0; j < map.linkedMoveAbleMap.Count; j++)
        {
            Map linkedMap = map.linkedMoveAbleMap[j];
            if (!checkedMapList.Contains(linkedMap))
            {
                checkedMapList.Add(linkedMap);
                if (linkedMap == tiles[new Vector2(0, gridHeight - 1)])
                {
                    //print("��� ���� ����Ǿ�����");
                    return true;
                }
                if (IsLinkedAllNode(linkedMap, checkedMapList))
                {
                    return true;
                }
            }
        }
        //print("��� ���� ����Ǿ����� ����");
        return false;
    }

    // �� �����ְ� ���ϴ� ����
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
                SetPlayerStartPos(tiles[new Vector2(-1, gridHeight - 1)]);
            }
            OpenMapPanel(true, first, time: time, OnComplete: () =>
            {
                StartCoroutine(PlayDirection(() =>
                {
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

    // �� ������ ����
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

    // �÷��̾� �̵� ����
    public void MovePlayer(Map map, Action onComplete = null, bool counting = false)
    {
        if (counting)
        {
            bossCount--;
            bossCountTxt.DOText(bossCount.ToString(), 0.5f);
        }
        DOTween.Sequence()
            .Append(playerTrm.DOLocalJump(map.transform.localPosition, 35f, 1, 0.5f).SetEase(Ease.InQuad).SetDelay(counting ? 0.5f : 0.3f))
            .Append(playerTrm.DOLocalMoveY(map.transform.localPosition.y + 40f, 0.08f))
            .Append(playerTrm.DOLocalMoveY(map.transform.localPosition.y, 0.3f).SetEase(Ease.OutBounce))
            .OnComplete(() =>
            {
                BreakMap(curMap);
                curMap = map;
                curMap.mapIcon.DOFade(0, 0.3f);
                mapCvsFollow.Follow(onEndAnim: () =>
                {
                    SetInteractebleCanSelectMap();
                    onComplete?.Invoke();
                });
            });
    }

    // �ʿ����� �� �Ǳ� ���� �ؾߵ� ����
    public IEnumerator PlayDirection(Action onEndDirection, bool first = false)
    {
        blockPanel.raycastTarget = true;
        yield return new WaitForSeconds(0.5f);
        //yield return null;

        if (first) // ��ó�� �μ����� �� ����
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
        else // ���� ī���� ����, ����� �� ������ �������鼭 �׾�ߵ�
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

    // �̵��� ���� ������ ������ �״� ����
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
                //ToDO ���� ����
                GameManager.Instance.ResetGame();
            });
    }

    // ���� �ʿ��� ����� ���� �ִ��� Ȯ��
    public bool CheckHasLinckedMap()
    {
        return curMap.linkedMoveAbleMap.Count > 0;
    }

    // ���� ī���� ���� �� ���� �ö󰡴� ����
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


    // �پ��ִ� �ʼ��� ����
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

    // ���� �Ұ����� �� ó���ؼ� ������ �� Ÿ�� ��ȯ
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
                //Debug.LogError("���� ������ �� �ȵǴ�");
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
        //Debug.Log(mapType + "�� ������ ���õ�");

        return mapType;
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
            maps[i].SetTileSprite(tileSpriteList[Random.Range(0, 6) + (GameManager.Instance.StageIdx * 7)]);
        }
    }

    // ��� �ʿ� Ÿ�� ����
    public void SetMapType()
    {
        // ���� ��ǥ �� Ÿ�� ����
        List<Vector2> fixedPosMapList = useFixedPosMapType.Keys.ToList();
        for (int i = 0; i < fixedPosMapList.Count; i++)
        {
            mapNode mapType = useFixedPosMapType[fixedPosMapList[i]];
            /*
            if (fixedMapTypeCount.Keys.Contains(mapType))
            {
                fixedMapTypeCount[mapType]--;
            }*/
            Debug.Log($"{tiles[fixedPosMapList[i]].name}�� {mapType} ��ǥ ������");
            tiles[fixedPosMapList[i]].MapType = mapType;
        }
        /*
        // ���� ���� �� Ÿ�� ����
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
                    Debug.Log($"{map.name}�� {mapType} ���� ������");
                    map.MapType = mapType;
                    count--;
                }
            }
        }*/

        // ���� �� Ÿ�� ���� <- ������ ����
        List<Map> mapList = tiles.Values.ToList();
        for (int i = 0; i < mapList.Count; i++)
        {
            if (mapList[i].MapType != mapNode.NONE)
            {
                continue;
            }

            Vector2 mapPos = GetTilesKeyToValue(mapList[i]);
            mapList[i].MapType = mapPos.Equals(new Vector2(-1f, gridHeight - 1)) || mapPos.Equals(new Vector2(0f, gridHeight - 1))
                ? mapNode.NONE
                : GetCanSetType(mapList[i]);
        }
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
            if (!(map.MapType == mapNode.NONE || map.MapType == mapNode.START || map.MapType == mapNode.BOSS))
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
                spriteIdx = 3;
                break;
            case mapNode.REST:
                spriteIdx = 2;
                break;
            case mapNode.RandomEncounter:
                //spriteIdx = 2;
                break;
            default:
                break;
        }
        return spriteIdx < 0 ? null : mapIconSpriteList[spriteIdx];
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
            Color c = mapList[i].mapIcon.color;
            mapList[i].SetInteracteble(curMap.linkedMoveAbleMap.Contains(mapList[i]));
            mapList[i].mapIcon.color = c.a == 0 ? c : mapList[i].mapIcon.color;
        }
    }

    // ���� �ʿ� ����� �ʵ� �����Ѱ� �����ϰ�, ���� ����
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

    // ���丮���� �ʿ� �ش�Ǵ� Ű���� ��ȯ
    private Vector2 GetTilesKeyToValue(Map value)
    {
        return tiles.FirstOrDefault(x => x.Value == value).Key;
    }

    // �ʰ� ������ִ� ����Ʈ���� �ٻ��� tiles������ ����
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

    // �� �μ����� ����
    public void BreakMap(Map map)
    {
        if (map == null)
        {
            return;
        }

        DestroyMap(map);
        Vector2 mapPosition = map.transform.position;

        DOTween.Sequence()
            .Append(map.GetComponent<CanvasGroup>().DOFade(0, 0.5f))
            .Join(map.transform.DOLocalMoveY(map.transform.localPosition.y - 0.5f, 0.5f))
            .OnComplete(() =>
            {
                map.gameObject.SetActive(false);
                //Destroy(map.gameObject);
            });
    }

    // ī�޶� �ܵǴ� ����
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

    // ī�޶� �ܿ� ���� �����̴� ���� ����
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
