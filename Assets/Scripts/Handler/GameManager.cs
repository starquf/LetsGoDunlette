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
    public event Action<SkillPiece, Sprite, Sprite> RewardEvent;
    [HideInInspector]
    public event Action OnUpdateUI;

    private int gold = 50;
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

    public void OnReward(SkillPiece reward, Sprite rewardSpr, Sprite rewardIconSpr)
    {
        RewardEvent?.Invoke(reward, rewardSpr, rewardIconSpr);
    }
}
