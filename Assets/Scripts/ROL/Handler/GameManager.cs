using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    print("���ӸŴ����� �������� �ʽ��ϴ�!!");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [HideInInspector]
    public BattleHandler battleHandler;
    [HideInInspector]
    public InventoryHandler inventoryHandler;

    [HideInInspector]
    public event Action<SkillPiece, Sprite> RewardEvent;

    public List<GameObject> rewardObjs;

    public void OnReward(SkillPiece reward, Sprite rewardSpr)
    {
        RewardEvent?.Invoke(reward, rewardSpr);
    }
}
