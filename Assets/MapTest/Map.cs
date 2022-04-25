using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Map : MonoBehaviour
{
    private mapNode mapType;

    public mapNode MapType
    {
        get
        {
            return mapType;
        }
        set
        {
            mapType = value;
        }
    }

    public Image mapOutLine;
    public Image mapIcon;
    public List<Map> linkedMoveAbleMap;

    private Sequence selectSequence;

    private Vector2 defaltPos;

    private void Awake()
    {
        selectSequence = DOTween.Sequence();
    }

    public void InitMap()
    {
        //defaltPos
    }

    public void StartMap()
    {
        // 인카운터 핸들러 받아서 실행해줄예정
    }

    public void SetIcon(Sprite iconSpr)
    {
        switch (mapType)
        {
            case mapNode.BOSS:
                mapIcon.transform.localScale = Vector2.one * 1.5f;
                break;
            default:
                break;
        }
        if(iconSpr == null)
        {
            mapIcon.color = Color.clear;
        }
        else
        {
            mapIcon.sprite = iconSpr;
        }
    }

    public Button GetBtn(int idx)
    {
        return transform.GetChild(idx).GetComponent<Button>();
    }

    public void OnSelected(bool enable, Action onComplete)
    {
        selectSequence.Kill();

        selectSequence
            .Append(mapIcon.DOFade(enable ? 0 : 1, 0.5f))
            .Join(mapOutLine.DOFade(enable ? 0 : 1, 0.5f))
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }
}
