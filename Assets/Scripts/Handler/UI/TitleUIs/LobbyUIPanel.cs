using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyUIPanel : MainUIPanel
{
    public Button startBtn;

    public Transform playerTrans;
    public Transform endPos;

    public CanvasGroup playerInfoCG;

    protected void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            SetInteract(false);

            StartGame();
        });
    }

    private void StartGame()
    {
        Vector2 startPos = playerTrans.position;
        Vector3 randomPos = startPos + (Random.insideUnitCircle * 1f);

        playerTrans.SetParent(endPos);

        GameManager.Instance.Gold = 99;

        float t = 0f;

        Vector2 bossCloudPos = endPos.position;
        Vector2 dir = bossCloudPos - (Vector2)playerTrans.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .Append(playerTrans.DORotate(new Vector3(playerTrans.rotation.x, playerTrans.rotation.y - (360f * 10), playerTrans.rotation.z), 1.3f).SetEase(Ease.OutCubic))
            .Insert(0.85f, DOTween.To(() => t, value =>
            {
                t = value;
                playerTrans.position = QuadraticBezierPoint(t, startPos, randomPos, endPos.position);
            }
            , 1f, 1f))
            .Append(playerTrans.DOScale(0f, 0.5f))
            .Join(playerTrans.GetComponent<Image>().DOFade(0f, 0.5f))
            .InsertCallback(1.5f, () =>
            {
                mainUIHandler.SetFade(true, false, () =>
                {
                    GameManager.Instance.ResetScene();
                    LoadingSceneHandler.LoadScene("SeunghwanScene");
                });
            });
    }

    private Vector3 QuadraticBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * start;
        p += 2 * u * t * p1;
        p += tt * end;

        return p;
    }

    private void ShowPanel(CanvasGroup cg, bool enable, bool isSkip)
    {
        cg.DOKill();

        if (isSkip)
        {
            cg.alpha = enable ? 1f : 0f;
        }
        else
        {
            cg.DOFade(enable ? 1f : 0f, 0.35f);
        }
    }

    public void ShowPlayerInfoCG(bool enable, bool isSkip)
    {
        ShowPanel(playerInfoCG, enable, isSkip);
    }
}
