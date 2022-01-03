using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum mapNode
{
    NONE = 0,
    START = 1,
    BOSS = 2,
    MONSTER = 3,
    SHOP = 4,
    REST = 5,
    TREASURE = 6,
}

public class Node
{
    public mapNode mapNode;
    public List<int> pointNodeIdx;

    public Node()
    {
        mapNode = mapNode.NONE;
        pointNodeIdx = new List<int>();
    }
}

public class MapCreater : MonoBehaviour
{
    public int mapRows;
    public int mapCols;
    public List<List<Node>> map = new List<List<Node>>();

    private void Awake()
    {
        Init();
        CreateMap();
        for (int i = 0; i < mapCols; i++)
        {
            for (int r = 0; r < mapRows; r++)
            {
                print($"cols : {i}, rows : {r}, {map[i][r].mapNode}");
            }
        }
    }
    void Start()
    {
        GameManager.Instance.mapHandler.map = this.map;
        GameManager.Instance.mapHandler.isSetting = true;
    }

    public void Init()
    {
        for (int c = 0; c < mapCols; c++)
        {
            map.Add(new List<Node>());
            for (int r = 0; r < mapRows; r++)
            {
                map[c].Add(new Node());
            }
        }
    }

    public void CreateMap(int curDeep = 0)
    {
        int beforeIdx = curDeep - 1;
        if(curDeep == 0) // 맨 처음일 떄 스타트로 만듬
        {
            map[0][3].mapNode = mapNode.START;
        }
        else if(curDeep == mapCols-1) // 맨끝일때 보스로만듬
        {
            map[curDeep][3].mapNode = mapNode.BOSS;
            List<int> list = GetNotNoneIdx(beforeIdx);
            foreach (int idx in list)
            {
                map[beforeIdx][idx].pointNodeIdx.Add(3);
            }
            return;
        }
        else if (curDeep == 1)
        {
            List<int> list = GetNotNoneIdx(beforeIdx);
            foreach (int idx in list)
            {
                int[] plusIdx = GetRandomIdx(2);
                for (int i = 0; i < plusIdx.Length; i++)
                {
                    int randIdx = Mathf.Clamp(plusIdx[i] + idx, 0, mapRows - 1);
                    map[curDeep][randIdx].mapNode = mapNode.MONSTER;
                    map[beforeIdx][idx].pointNodeIdx.Add(randIdx);
                }
            }
        }
        else if(curDeep == mapCols - 2)
        {
            List<int> list = GetNotNoneIdx(beforeIdx);
            foreach (int idx in list)
            {
                int[] plusIdx = GetRandomIdx(Random.Range(1, 2));
                for (int i = 0; i < plusIdx.Length; i++)
                {
                    int randIdx = Mathf.Clamp(plusIdx[i] + idx, 0, mapRows - 1);
                    map[curDeep][randIdx].mapNode = mapNode.REST;
                    map[beforeIdx][idx].pointNodeIdx.Add(randIdx);
                }
            }
        }
        else
        {
            List<int> list = GetNotNoneIdx(beforeIdx);
            foreach (int idx in list)
            {
                int[] plusIdx = GetRandomIdx(Random.Range(1,3));
                for (int i = 0; i < plusIdx.Length; i++)
                {
                    int randIdx = Mathf.Clamp(plusIdx[i] + idx, 0, mapRows-1);
                    map[curDeep][randIdx].mapNode = GetRandomNode();
                    map[beforeIdx][idx].pointNodeIdx.Add(randIdx);
                }
            }
        }
        CreateMap(++curDeep);
    }

    public mapNode GetRandomNode()
    {
        mapNode nodeType;
        int rand = Random.Range(0, 100);
        if(rand<5)
        {
            nodeType = mapNode.SHOP;
        }
        else if(rand<17)
        {
            nodeType = mapNode.REST;
        }
        else if(rand < 39)
        {
            nodeType = mapNode.TREASURE;
        }
        else
        {
            nodeType = mapNode.MONSTER;
        }
        return nodeType;
    }

    public List<int> GetNotNoneIdx(int idx)
    {
        List<int> notNoneIdxList = new List<int>();
        for (int i = 0; i < map[idx].Count; i++)
        {
            if(map[idx][i].mapNode != mapNode.NONE)
            {
                notNoneIdxList.Add(i);
            }
        }
        return notNoneIdxList;
    }

    public int[] GetRandomIdx(int count)
    {
        int[] result = new int[count];
        List<int> randList = new List<int>() { -1, 0, 1 };
        for (int i = 0; i < count; i++)
        {
            int randIdx = Random.Range(0, randList.Count);
            result[i] = randList[randIdx];
            randList.RemoveAt(randIdx);
        }
        return result;
    }
}
