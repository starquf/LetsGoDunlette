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
                    print("���ӸŴ����� �������� �ʽ��ϴ�!!");
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
        int setWidth = 1440; // ����� ���� �ʺ�
        int setHeight = 2560; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)                      // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);                  // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);// ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);                // ���ο� Rect ����
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

    public PlayerHealth GetPlayer() //�÷��̾ �����ɴϴ�.
    {
        return battleHandler.player;
    }

    public void EndEncounter()
    {
        OnEndEncounter();
    }

    public int TryStillGold(int max) //�ִ� max ��ŭ ��带 ��ħ ��ģ ��带 ����
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
