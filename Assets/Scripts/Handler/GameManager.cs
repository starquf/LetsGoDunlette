using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    print("게임매니져가 존재하지 않습니다!!");
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        Application.targetFrameRate = 60;
        Screen.SetResolution(1440, 2560, true);

        SetResolution();
    }

    private void SetResolution()
    {
        int setWidth = 1440; // 사용자 설정 너비
        int setHeight = 2560; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)                      // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);                  // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);// 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);                // 새로운 Rect 적용
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = default;
        }
    }
    #endregion

    [HideInInspector]
    public BattleHandler battleHandler;
    [HideInInspector]
    public CameraHandler cameraHandler;
    [HideInInspector]
    public InventoryHandler inventoryHandler;
    [HideInInspector]
    public InventoryInfoHandler invenInfoHandler;
    [HideInInspector]
    public MapHandler mapHandler;
    [HideInInspector]
    public ShakeHandler shakeHandler;
    [HideInInspector]
    public SkillPrefabContainer skillContainer;
    [HideInInspector]
    public BattleFieldHandler battleFieldHandler;

    [HideInInspector]
    public event Action OnUpdateUI;
    [HideInInspector]
    public event Action OnEndEncounter;

    private int gold = 1000;
    public int Gold 
    {
        get 
        {
            return gold;
        } 
        set 
        {
            gold = value;
            OnUpdateUI?.Invoke();
        }
    }

    public PlayerHealth GetPlayer() //플레이어를 가져옵니다.
    {
        return battleHandler.player;
    }

    public void EndEncounter()
    {
        OnEndEncounter();
    }

    public int TryStillGold(int max) //최대 max 만큼 골드를 훔침 훔친 골드를 리턴
    {
        int remain = gold - max;

        if (remain >= 0)
        {
            return max;
        }
        else
        {
            return gold;
        }
    }

}
