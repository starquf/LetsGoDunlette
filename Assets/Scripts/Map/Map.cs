using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [HideInInspector] public MapManager mapManager;

    [HideInInspector] public mapTileEvent tileType;
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

    public TextMeshProUGUI limitTimeTmp;
    public Image blinkMapOutLine;
    public Image mapOutLine;
    public Image mapIcon;
    public List<Map> linkedMoveAbleMap;

    private Sequence selectSequence;

    private float defaultLocalPosY;
    private Button button;
    private bool isSelected;

    [HideInInspector] private int timeLimit;
    [HideInInspector] public bool isBlinked;
    [HideInInspector] public Map teleportMap;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Genragte 직후
    public void InitMap(MapManager mapManager, Action onEnd = null)
    {
        mapType = mapNode.NONE;
        tileType = mapTileEvent.NONE;
        timeLimit = -1;
        button.onClick.RemoveAllListeners();
        this.mapManager = mapManager;
        defaultLocalPosY = GetComponent<RectTransform>().localPosition.y;
        isSelected = false;
        mapIcon.color = Color.white;
        teleportMap = null;
        limitTimeTmp.gameObject.SetActive(false);
        button.onClick.AddListener(OnClickButton);
        StartCoroutine(Blink(false, 0, true, onEnd));
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
        if (isBlinked && enable)
        {
            return;
        }

        button.interactable = enable;

        if (mapManager.GetMapIcon(mapType) != null)
        {
            mapIcon.color = enable ? Color.white : new Color(0.7f, 0.7f, 0.7f, 1f);
        }
    }

    private void OnClickButton()
    {
        mapManager.FollowZeroCam();
        if (!isSelected) // 처음에 버튼 선택했을 때
        {
            OnSelected(true);
            mapManager.UnSelectedLinkedMap(this);
            mapManager.mapDesUIHandler.ShowDescription(mapType);
        }
        else // 맵 입장 선택
        {
            mapManager.SetAllInteracteble(false);
            OnSelected(false, 0.1f, () =>
            {
                mapManager.MovePlayer(this,
                    () =>
                    {
                        mapManager.StartMap(mapType);
                    }, true);
                mapManager.mapDesUIHandler.ShowPanel(false);
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
        {
            return;
        }

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

    public void SetLimitTime(int time)
    {
        limitTimeTmp.gameObject.SetActive(true);
        timeLimit = time;
        UpdateLimitTime(true);
    }

    public void UpdateLimitTime(bool isSet = false, float time = 0.5f, Action onEnd = null)
    {
        if (!isSet)
        {
            timeLimit--;
            StartCoroutine(UpdateLimitTimeAnim(time, onEnd));
        }
        else
        {
            limitTimeTmp.text = timeLimit.ToString();
        }
    }

    private IEnumerator UpdateLimitTimeAnim(float time, Action onEnd)
    {
        limitTimeTmp.text = timeLimit.ToString();

        yield return new WaitForSeconds(time);

        if (timeLimit <= 0)
        {
            mapManager.BreakMap(this, time, onEnd);
        }
        else
        {
            onEnd?.Invoke();
        }
    }

    public void BlinkMap(float time = 0.5f, Action onEndEvent = null)
    {
        StartCoroutine(Blink(!isBlinked, time, false, onEndEvent));
    }

    public IEnumerator Blink(bool enable, float time = 0.5f, bool skip = false, Action onEndEvent = null)
    {
        Image tileImage = GetComponent<Image>();
        // 켜지는거
        if (!enable)
        {
            blinkMapOutLine.color = Color.clear;
            tileImage.color = Color.white;
            if (mapManager.GetMapIcon(mapType) != null)
            {
                mapIcon.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            }
        }
        // 꺼지는거
        else
        {
            blinkMapOutLine.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            tileImage.color = Color.clear;
            if (mapManager.GetMapIcon(mapType) != null)
            {
                mapIcon.color = Color.clear;
            }
        }
        isBlinked = enable;

        if (skip)
        {
            yield return new WaitForSeconds(time);
        }

        onEndEvent?.Invoke();
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
