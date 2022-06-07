using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                    //print("���ӸŴ����� �������� �ʽ��ϴ�!!");
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
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        Screen.SetResolution(1080, 1920, true);

        SetResolution();

        print(StageIdx);
    }

    public void SetResolution()
    {
        int setWidth = 1080; // ����� ���� �ʺ�
        int setHeight = 1920; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)((float)deviceHeight / deviceWidth * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)                      // ����� �ػ� �� �� ū ���
        {
            float newWidth = (float)setWidth / setHeight / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);                  // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = (float)deviceWidth / deviceHeight / ((float)setWidth / setHeight);// ���ο� ����
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
    public MapManager mapManager;
    [HideInInspector]
    public ShakeHandler shakeHandler;
    [HideInInspector]
    public SkillPrefabContainer skillContainer;
    [HideInInspector]
    public BottomUIHandler bottomUIHandler;
    [HideInInspector]
    public GoldUIHandler goldUIHandler;
    [HideInInspector]
    public ToBeContinueHandler tbcHandler;
    [HideInInspector]
    public EncounterHandler encounterHandler;
    [HideInInspector]
    public AnimHandler animHandler;
    [HideInInspector]
    public BuffParticleHandler buffParticleHandler;
    [HideInInspector]
    public GetPieceHandler getPieceHandler;
    [HideInInspector]
    public UILevelUPPopUp uILevelUPPopUp;
    [HideInInspector]
    public YesOrNoPanel YONHandler;

    [HideInInspector]
    public event Action OnUpdateUI;
    [HideInInspector]
    public event Action OnEndEncounter;
    [HideInInspector]
    public event Action OnResetGame;
    [HideInInspector]
    public event Action OnNextStage;

    [HideInInspector]
    public Transform enemyEffectTrm;

    [Header("�����ܵ�")]
    public List<Sprite> ccIcons = new List<Sprite>();
    public List<Sprite> buffIcons = new List<Sprite>();

    public mapNode curEncounter = mapNode.NONE;

    public int StageIdx { get; set; } = 0;

    [Header("���డ���� ��������")]
    public int progressiveStageIdx = 1;

    private int gold = 100;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnUpdateUI?.Invoke();
        }
    }

    #region TEST
    public int deckIdx = -1;

    #endregion

    public PlayerHealth GetPlayer() //�÷��̾ �����ɴϴ�.
    {
        return battleHandler.player;
    }

    public void NextStage()
    {
        StageIdx++;
        OnNextStage();
    }

    public void EndEncounter()
    {
        OnEndEncounter();
    }

    public void ResetGame()
    {
        OnResetGame();
    }

    public int TryStillGold(int max) //�ִ� max ��ŭ ��带 ��ħ ��ģ ��带 ����
    {
        int remain = gold - max;
        return remain >= 0 ? max : gold;
    }

    public void LoadScene(int sceneIdx)
    {
        ResetScene();
        SceneManager.LoadScene(sceneIdx);
    }

    public void LoadScene(string sceneName)
    {
        ResetScene();
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScene()
    {
        DOTween.KillAll();
        PoolManager.ResetPool();

        OnUpdateUI = null;
        OnEndEncounter = null;
        OnResetGame = null;
    }

    public bool IsEndStage()
    {
        return StageIdx == progressiveStageIdx;
    }
}
