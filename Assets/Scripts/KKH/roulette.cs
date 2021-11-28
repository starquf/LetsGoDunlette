using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RType 
{ 
    rEnemyAttack = 0,
    rDefault = 1,
    rWater = 2,
    rlightning = 3
}

[System.Serializable]
public class rouletteProperty
{
    public RType type;
    public int size;
    public int startPoint;
    public int endPoint;
    //public bool ex; // 18 이하에서 0을 넘었을 때 true
}

public class roulette : MonoBehaviour
{
    public GameObject roulettePrefab;
    // 최소 단위 18
    public List<rouletteProperty> roulettes;
    public List<roulettepiece> roulettePieces; 
    public RType[] rouletteArr;
    public Transform arrow;

    //룰렛 돌리기
    private bool rolling;
    private bool isStop;
    private float rotSpeed;
    private int rollType;

    void Start()
    {
        Init();


        InsertRoulette(5, 3, RType.rEnemyAttack);
    }

    void Update()
    {
        if(rolling)
        {
            transform.Rotate(0, 0, rotSpeed);
            if(isStop)
                rotSpeed *= 0.99f;
            if(Mathf.Abs(rotSpeed)<0.05f)
            {
                rolling = false;
                isStop = false;
                GetResultRoulette(rollType);
            }
        }
    }

    public void RollRoulette(int type)
    {
        rollType = type;
        rotSpeed = Random.Range(-50.0f,-30.0f);
        rolling = true;
        isStop = true;
    }

    private void GetResultRoulette(int type)
    {
        foreach (roulettepiece rp in roulettePieces)
        {
            foreach (rouletteCollision rc in rp.rouletteCollisions)
            {
                if(rc.isCollision)
                {
                    switch (type)
                    {
                        // 공격
                        case 0:
                            GameManager.instance.Attack(rc.roulette.type);
                            break;
                        //교체
                        case 1:
                            RType rt = (RType)Random.Range(2, 4);
                            int s = Random.Range(1, 6);
                            print(rt);
                            print(s);
                            InsertRoulette(rc.propertyIdx, s, rt);
                            print(rc.propertyIdx);
                            GameManager.instance.AddRoulette();
                            break;
                        default:
                            break;
                    }
                    return;
                }
            }
        }
    }

    private void Init()
    {
        rouletteArr = new RType[18];
        for (int i = 0; i < rouletteArr.Length; i++)
        {
            rouletteArr[i] = RType.rDefault;
        }

        SetUpList();
    }

    public void ShowRoulette()
    {
        // todo
        // fillAmout 0.05555556 = 1조각
        for (int i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        roulettePieces.Clear();
        float oneFillAmount = 0.05555556f;
        foreach (rouletteProperty roulette in roulettes)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, -20 * (roulette.startPoint))) * transform.localRotation;
            Image r = Instantiate(roulettePrefab, Vector3.zero, rotation, transform).GetComponent<Image>();
            roulettepiece rp = r.GetComponent<roulettepiece>();
            roulettePieces.Add(rp);
            //rp.rouletteCollisionArr = new rouletteCollision[roulette.size];
            rp.target = arrow;
            rp.distance = 350f;
            for (int i = 0; i < roulette.size; i++)
            {
                int idx = roulette.startPoint + i;
                rp.rouletteCollisions.Add(new rouletteCollision {roulette = roulette ,propertyIdx= idx > 17? idx - 18: idx, middleAngle = Quaternion.Euler(new Vector3(0, 0, (i * -20f) -10f)), isCollision = false });
            }

            r.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; 
            r.fillAmount = roulette.size * oneFillAmount;

            switch (roulette.type)
            {
                case RType.rEnemyAttack:
                    r.color = Color.red;
                    break;
                case RType.rDefault:
                    r.color = Color.white;
                    break;
                case RType.rWater:
                    r.color = Color.blue;
                    break;
                case RType.rlightning:
                    r.color = Color.yellow;
                    break;
                default:
                    break;
            }

            rp.isSetting = true;
        }
    }
    public void InsertRoulette(int middlePos, int size, RType type = RType.rDefault)
    {
        int deleteSize = (size-1) / 2; // size == 2 0, size == 3 = 1, size == 4 1
        print($"insert: size{size}, delete{deleteSize}");
        if (deleteSize < 0)
        {
            Debug.LogError("InsertRouletteError");
            deleteSize = 0;
        }

        //생성될 룰렛 속성
        int startPoint = middlePos - deleteSize < 0 ? middlePos - deleteSize + 18 : middlePos - deleteSize;
        int endPoint = startPoint + size;
        bool ex = middlePos - deleteSize < 0 || startPoint + size > 18;

        for (int i = startPoint; i < endPoint; i++)
        {
            int changeIdx = i > 17 ? i - 18 : i;

            rouletteArr[changeIdx] = type;
        }

        SetUpList();
    }

    public void SetUpList()
    {
        roulettes.Clear();
        int beforeIdx = 0;
        RType beforeType = rouletteArr[0];

        for (int i = 0; i < rouletteArr.Length; i++)
        {
            int size = i - beforeIdx;
            if (beforeType != rouletteArr[i])
            {
                roulettes.Add(new rouletteProperty { type = beforeType, size = size, startPoint = beforeIdx, endPoint = i - 1 });
                beforeIdx = i;
                beforeType = rouletteArr[i];
            }
            if(i == rouletteArr.Length - 1)
            {
                if (rouletteArr[i] == 0)
                {
                    foreach (rouletteProperty roulette in roulettes)
                    {
                        if (roulette.startPoint == 0)
                        {
                            roulette.startPoint = beforeIdx;
                            roulette.size += size + 1;
                        }
                    }
                }
                else
                {
                    roulettes.Add(new rouletteProperty { type = beforeType, size = size + 1, startPoint = beforeIdx, endPoint = i });
                }
            }
        }

        ShowRoulette();
    }
}
