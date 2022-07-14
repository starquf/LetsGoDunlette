using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CustomDic;
using DG.Tweening;

[Serializable]
public class MapDes
{
    public Sprite icon;
    public string name;
    public string des;
}

public class MapDesUIHandler : MonoBehaviour
{
    public SerializableDictionary<mapNode, MapDes> mapDesDic;

    [Space(10f)]
    private MapManager mapManager;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI desText;
    public Image mapIconImg;
    public Image mapTileImg;

    [Space(10f)]
    private CanvasGroup cg;
    private RectTransform rect;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();

        mapManager = GameManager.Instance.mapManager;

        ShowPanel(false, true);
    }

    public void ShowDescription(mapNode mapType)
    {
        MapDes mapDes = mapDesDic[mapType];
        Sprite mapIcon = mapDes.icon;
        string name = mapDes.name;
        string des = mapDes.des;

        mapIconImg.sprite = mapIcon;
        mapTileImg.sprite = mapManager.tileSpriteList[5 + (GameManager.Instance.StageIdx * 7)];
        nameText.text = name;
        desText.text = des;

        if (des.Equals(""))
        {
            desText.gameObject.SetActive(false);
        }
        else
        {
            desText.gameObject.SetActive(true);
        }

        if(mapIcon == null)
        {
            mapIconImg.color = Color.clear;
        }
        else
        {
            mapIconImg.color = Color.white;
        }

        ShowPanel(true);
    }

    public void ShowPanel(bool enable, bool skip = false)
    {
        cg.blocksRaycasts = enable;
        cg.interactable = enable;
        if(skip)
        {
            cg.alpha = enable ? 1f : 0f;
        }
        else
        {
            cg.DOFade(enable ? 1f : 0f, 0.5f);
        }
    }
}
