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
    public int idx;
    public int depth;

    public mapNode mapNode;
    public List<Node> pointNodeList;

    public Node(int idx, int depth)
    {
        mapNode = mapNode.NONE;
        pointNodeList = new List<Node>();
        this.idx = idx;
        this.depth = depth;
    }
}

public class MapCreater : MonoBehaviour
{
    public GameObject MapNodePrefab;

    public int mapRows;
    public int mapCols;
    public List<List<Node>> map = new List<List<Node>>();
    private bool firstCreate = false;

    private void Awake()
    {
        Init();
        CreateMap();
    }

    public void MapCreateComplete()
    {
        print("맵 다만듬");
        MapHandler mapHandler = GameManager.Instance.mapHandler;

        if(!(mapHandler.Content.transform.childCount > 0))
        {
            for (int c = 0; c < mapCols; c++)
            {
                for (int r = 0; r < mapRows; r++)
                {
                    GameObject newNode = GameObject.Instantiate(MapNodePrefab, mapHandler.Content.transform);
                    newNode.name += $"{r},{c}";
                }
            }
        }

        mapHandler.map = this.map;
        mapHandler.ShowMap();
        mapHandler.OnSelectNode(map[0][3]);
    }

    // 대충 초기화 해주는 거
    public void Init()
    {
        for (int c = 0; c < mapCols; c++)
        {
            map.Add(new List<Node>());
            for (int r = 0; r < mapRows; r++)
            {
                map[c].Add(new Node(r, c));
            }
        }
    }

    public void MapReset()
    {
        for (int c = 0; c < mapCols; c++)
        {
            map.Add(new List<Node>());
            for (int r = 0; r < mapRows; r++)
            {
                map[c][r].pointNodeList.Clear();
                map[c][r].mapNode = mapNode.NONE;
            }
        }
    }

    // 맵 만드는 함수
    public void CreateMap(int curDepth = 0)
    {
        int beforeIdx = curDepth - 1;
        if(curDepth == 0) // 맨 처음일 떄 스타트로 만듬
        {
            map[0][3].mapNode = mapNode.START;
        }
        else if(curDepth == mapCols-1) // 맨끝일때 보스로만듬
        {
            map[curDepth][3].mapNode = mapNode.BOSS;
            List<int> list = GetNotNoneIdx(beforeIdx);
            foreach (int idx in list)
            {
                map[beforeIdx][idx].pointNodeList.Add(map[curDepth][3]);
            }
            MapCreateComplete();
            return;
        }
        else if (curDepth == 1)
        {
            List<int> list = GetNotNoneIdx(beforeIdx);
            foreach (int idx in list)
            {
                int[] plusIdx = GetRandomIdx(2);
                for (int i = 0; i < plusIdx.Length; i++)
                {
                    int randIdx = Mathf.Clamp(plusIdx[i] + idx, 0, mapRows - 1);
                    map[curDepth][randIdx].mapNode = mapNode.MONSTER;
                    map[beforeIdx][idx].pointNodeList.Add(map[curDepth][randIdx]);
                }
            }
            //SetNode(curDepth, mapNode.MONSTER);
        }
        else if(curDepth == mapCols - 2)
        {
            SetNode(curDepth, mapNode.REST);
        }
        else
        {
            SetNode(curDepth);
        }

        CreateMap(++curDepth);
    }

    // 노드에 종류 추가 + 전노드에서 연결
    public void SetNode(int curDepth, mapNode mapType = mapNode.NONE)
    {
        int maxLine = 0;

        List<int> list = GetNotNoneIdx(curDepth-1);

        int nodeCount = list.Count;

        while(maxLine < nodeCount)
        {
            maxLine = Random.Range(mapRows - 3, mapRows);
        }

        int lineCount = maxLine / nodeCount;
        int exLine = maxLine - (lineCount * nodeCount);

        List<int> lineCountList = new List<int>();
        for (int i = 0; i < nodeCount; i++)
        {
            lineCountList.Add(lineCount);
        }
        int j = 0;
        while(exLine-j > 0)
        {
            lineCountList[j/ lineCountList.Count]++;
            if(lineCountList[(j/ lineCountList.Count)]>3)
            {
                Debug.LogError("맵밸런스 ㅈ됨");
            }
            j++;
        }

        for (int k = 0; k < list.Count; k++)
        {
            int[] plusIdx = GetRandomIdx(lineCountList[k]);
            for (int i = 0; i < plusIdx.Length; i++)
            {
                int randIdx = Mathf.Clamp(plusIdx[i] + list[k], 0, mapRows - 1);
                map[curDepth][randIdx].mapNode = mapType != mapNode.NONE ? mapType : GetRandomNode();
                map[curDepth - 1][list[k]].pointNodeList.Add(map[curDepth][randIdx]);
            }
        }
    }

    // 랜덤으로 노드 값 가져오기
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

    // 노드 값이 NONE이 아닌 맵 노드(존재하는 노드)의 Idx배열 가져오기
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

    // 왼쪽 가운데 오른족 중 연결될 노드 가중치 랜덤으로 가져오기
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
