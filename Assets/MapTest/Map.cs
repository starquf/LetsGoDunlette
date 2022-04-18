using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [HideInInspector]
    public MapTest mapTest;
    [HideInInspector]
    public mapNode type;
    [HideInInspector]
    public List<mapNode> randomNodesList = new List<mapNode>();
    [HideInInspector]
    public List<mapNode> cannotBeNodesList = new List<mapNode>();
    [HideInInspector]
    public int mapIdx;
    [HideInInspector]
    public bool isFixedType;

    public mapNode GetMapNode()
    {
        return type;
    }

    public void StartMap()
    {
        // 인카운터 핸들러 받아서 실행해줄예정
    }

    public void SetIcon()
    {
        Sprite iconSpr = mapTest.GetTypeIcon(type);
        Image img = transform.GetChild(0).GetComponent<Image>();
        switch (type)
        {
            case mapNode.BOSS:
                img.transform.localScale = Vector2.one * 1.5f;
                break;
            default:
                break;
        }
        if(iconSpr == null)
        {
            img.color = Color.clear;
        }
        else
        {
            img.sprite = iconSpr;
        }
    }

    public Button GetBtn(int idx)
    {
        return transform.GetChild(idx).GetComponent<Button>();
    }

    public void SetBtnInteractable(bool enble)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GetBtn(i).interactable = enble;
        }
    }

    public void SetMapNode(mapNode type)
    {
        if(type == mapNode.EMONSTER || type == mapNode.REST)
        {
            List<Map> mapList = mapTest.GetUpDownMap(mapIdx);
            for (int i = 0; i < mapList.Count; i++)
            {
                mapList[i].cannotBeNodesList.Add(type);
                print(type+"적용됨");
            }
        }
        this.type = type;
        SetIcon();
    }

    public void SetRandomNode()
    {
        randomNodesList.Clear();

        int count = Random.Range(0, 100) < 50 ? 2 : 3;

        for (int i = 0; i < count; i++)
        {
            mapNode mNode = mapNode.NONE;
            do
            {
                int rand = Random.Range(0, 100);
                if (rand < 12)
                {
                    mNode = mapNode.REST;
                }
                else if (rand < 34)
                {
                    mNode = mapNode.RandomEncounter;
                }
                else if (rand < 42)
                {
                    mNode = mapNode.SHOP;
                }
                else if (rand < 50)
                {
                    mNode = mapNode.EMONSTER;
                }
                else
                {
                    mNode = mapNode.MONSTER;
                }
            } while (cannotBeNodesList.Contains(mNode));

            randomNodesList.Add(mNode);
        }
    }
}
