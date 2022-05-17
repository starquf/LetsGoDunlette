using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIHandler : MonoBehaviour
{
    [Header("����� ��� Ȱ��ȭ ����")]
    public bool isDebug = true;

    [Header("����� UI ������")]
    [Space(30f)]
    public CanvasGroup mainPanel;

    [Header("���� ��ư��")]
    public Button openBtn;
    public Button closeBtn;
    public Button resetBtn;

    [Header("�г� �ڵ鷯��")]
    public List<GameObject> debugPanels = new List<GameObject>();
    private List<IDebugPanel> handlers;

    private void Start()
    {
        if (!isDebug)
        {
            gameObject.SetActive(false);
            return;
        }

        GetHandlers();
        Init();

        SetDebugPanel(false);
    }

    #region Inits
    private void GetHandlers()
    {
        handlers = new List<IDebugPanel>();

        for (int i = 0; i < debugPanels.Count; i++)
        {
            handlers.Add(debugPanels[i].GetComponent<IDebugPanel>());
        }
    }

    private void Init()
    {
        MainBtnInit();

        for (int i = 0; i < handlers.Count; i++)
        {
            handlers[i].Init();
        }
    }

    private void MainBtnInit()
    {
        openBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 0f;

            ResetDebugPanel();
            SetDebugPanel(true);
        });

        closeBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;

            SetDebugPanel(false);
        });

        resetBtn.onClick.AddListener(ResetDebugPanel);
    }
    #endregion

    #region Reset Func
    private void ResetDebugPanel()
    {
        for (int i = 0; i < handlers.Count; i++)
        {
            handlers[i].OnReset();
        }
    }
    #endregion

    private void SetDebugPanel(bool enable)
    {
        mainPanel.alpha = enable ? 1f : 0f;
        mainPanel.blocksRaycasts = enable;
        mainPanel.interactable = enable;
    }
}
