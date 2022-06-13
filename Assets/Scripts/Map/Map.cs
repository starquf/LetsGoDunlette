using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [HideInInspector] public MapManager mapManager;

    private mapNode mapType;

    public mapNode MapType
    {
        get => mapType;
        set
        {
            mapType = value;
            SetIcon(mapManager.GetMapIcon(value));
        }
    }

    public Image mapOutLine;
    public Image mapIcon;
    public List<Map> linkedMoveAbleMap;

    private Sequence selectSequence;

    private float defaultLocalPosY;
    private Button button;
    private bool isSelected;

    private void Awake()
    {
        button = GetComponent<Button>();
        mapType = mapNode.NONE;
    }

    public void InitMap(MapManager mapManager)
    {
        button.onClick.RemoveAllListeners();
        this.mapManager = mapManager;
        defaultLocalPosY = GetComponent<RectTransform>().localPosition.y;
        isSelected = false;
        mapIcon.color = Color.white;
        button.onClick.AddListener(OnClickButton);
    }

    public void SetTileSprite(Sprite tileSpr)
    {
        GetComponent<Image>().sprite = tileSpr;
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
        if (iconSpr == null)
        {
            mapIcon.color = Color.clear;
        }
        else
        {
            mapIcon.sprite = iconSpr;
        }
    }

    public void SetInteracteble(bool enable)
    {
        button.interactable = enable;

        if (mapManager.GetMapIcon(mapType) != null)
        {
            mapIcon.color = enable ? Color.white : new Color(0.7f, 0.7f, 0.7f, 1f);
        }
    }

    private void OnClickButton()
    {
        if (!isSelected) // 처음에 버튼 선택했을 때
        {
            OnSelected(true);
            mapManager.UnSelectedLinkedMap(this);
        }
        else // 맵 입장 선택
        {
            mapManager.SetAllInteracteble(false);
            OnSelected(false, 0.1f, () =>
            {
                mapManager.MovePlayer(this,
                    () =>
                    {
                        mapManager.ZoomCamera(15f, time: 0.7f, ease: Ease.InOutSine, onComplete: () =>
                        {
                            SetIcon(null);
                        });
                        mapManager.StartMap(mapType);
                    }, true);
            });
        }
    }

    public void OnUnSelected()
    {
        OnSelected(false);
    }

    public void OnSelected(bool enable, float time = 0.5f, Action onComplete = null)
    {
        if (!isSelected && !enable)
            return;
        isSelected = enable;
        selectSequence.Kill();
        RectTransform rect = GetComponent<RectTransform>();

        selectSequence = DOTween.Sequence()
            .Append(rect.DOLocalMoveY(enable ? rect.localPosition.y + (rect.sizeDelta.x / 5f) : defaultLocalPosY, time))
            .Join(mapOutLine.DOFade(enable ? 1 : 0, time))
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public void OnDisable()
    {
        mapType = mapNode.NONE;
        for (int i = 0; i < linkedMoveAbleMap.Count; i++)
        {
            linkedMoveAbleMap[i].linkedMoveAbleMap.Remove(this);
        }
        linkedMoveAbleMap.Clear();
    }
}
