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
                    //print("게임매니져가 존재하지 않습니다!!");
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
        int setWidth = 1080; // 사용자 설정 너비
        int setHeight = 1920; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)((float)deviceHeight / deviceWidth * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight)                      // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = (float)setWidth / setHeight / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);                  // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = (float)deviceWidth / deviceHeight / ((float)setWidth / setHeight);// 새로운 높이
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

    [Header("아이콘들")]
    public List<Sprite> ccIcons = new List<Sprite>();
    public List<Sprite> buffIcons = new List<Sprite>();

    public mapNode curEncounter = mapNode.NONE;

    public int StageIdx { get; set; } = 0;

    [Header("진행가능한 스테이지")]
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

    public PlayerHealth GetPlayer() //플레이어를 가져옵니다.
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

    public int TryStillGold(int max) //최대 max 만큼 골드를 훔침 훔친 골드를 리턴
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
