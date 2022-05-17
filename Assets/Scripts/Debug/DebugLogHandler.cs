using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LogType
{
    OnlyText,
    ImgTextToTarget,
    ImgImgToTarget,
    ImageText
}

public class LogCon
{
    public string text;

    public Sprite selfSpr;
    public Sprite processSpr;
    public Sprite targetSpr;

    public LogType logType;

    public bool hasLine = false;
}

public class DebugLogHandler : MonoBehaviour, IDebugPanel
{
    private static List<LogCon> logs = new List<LogCon>();

    private List<LogLine> logLines = new List<LogLine>();

    [Header("로그 관련")]
    public Button logOpenBtn;
    public Button logCloseBtn;
    public CanvasGroup logPanel;

    public Transform content;

    public void Init()
    {
        SetLogPanel(false);

        logOpenBtn.onClick.AddListener(() =>
        {
            ResetLog();
            ShowLog();

            SetLogPanel(true);
        });

        logCloseBtn.onClick.AddListener(() =>
        {
            SetLogPanel(false);
        });
    }

    public void OnReset()
    {

    }

    public static void AddLog(LogType logType, LogCon logCon)
    {
        logCon.logType = logType;
        logs.Add(logCon);
    }

    private void ShowLog()
    {
        for (int i = 0; i < logs.Count; i++)
        {
            LogCon log = logs[i];

            LogLine line = PoolManager.GetItem<LogLine>();
            line.transform.SetParent(content);
            line.transform.SetAsLastSibling();

            line.transform.position = Vector3.zero;
            line.transform.localScale = Vector3.one;

            line.Init(log.logType, log.hasLine);

            line.startImg.sprite = log.selfSpr;
            line.targetImg.sprite = log.targetSpr;
            line.msgText.text = log.text;
            line.processText.text = log.text;
            line.processImg.sprite = log.processSpr;

            logLines.Add(line);
        }
    }

    private void ResetLog()
    {
        for (int i = 0; i < logLines.Count; i++)
        {
            logLines[i].gameObject.SetActive(false);
        }

        logLines.Clear();
    }

    private void SetLogPanel(bool enable)
    {
        logPanel.alpha = enable ? 1f : 0f;
        logPanel.blocksRaycasts = enable;
        logPanel.interactable = enable;
    }
}
