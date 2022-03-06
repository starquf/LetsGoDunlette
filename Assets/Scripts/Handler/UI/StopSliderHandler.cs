using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopSliderHandler : MonoBehaviour
{
    private CanvasGroup cg;
    private Button tapBtn;

    private bool isStopped = false;
    public bool IsStopped => isStopped;

    [HideInInspector]
    public Rullet rullet;

    public Action<RulletPiece, int> onStopRullet;
    public Action onStartStop;

    private void Start()
    {
        cg = GetComponent<CanvasGroup>();
        tapBtn = GetComponent<Button>();

        tapBtn.onClick.AddListener(() =>
        {
            if (!isStopped)
            {
                onStartStop?.Invoke();
                StopRullet();
            }
        });
        cg.interactable = false;
    }

    public void Init(Rullet rullet, Action<RulletPiece, int> onStopRullet, Action onStartStop = null)
    {
        this.rullet = rullet;
        rullet.onResult = onStopRullet;
        this.onStartStop = onStartStop;
    }

    private void StopRullet()
    {
        isStopped = true;

        rullet.StopRullet();
    }

    public void SetInteract(bool enable)
    {
        isStopped = !enable;
        cg.interactable = enable;
        cg.blocksRaycasts = enable;
    }


    // 리롤 기능을 켜주게 하는 함수 (예전 기능)
    /*
    private void SetReroll()
    {
        rerollGold = 5;
        rerollGoldText.text = rerollGold.ToString();

        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(-245f, 0.2f);

        // 만약 현재 골드가 리롤 골드보다 작다면
        if (GameManager.Instance.Gold < rerollGold)
        {
            blinkTween.Kill();
            tapGroup.GetComponent<Image>().color = Color.black;

            canReroll = false;
        }
        else
        {
            canReroll = true;

            /*
             * blinkTween = tapGroup.GetComponent<Image>().DOColor(Color.black, 1f)
                  .SetEase(Ease.Flash, 20, 0)
                  .SetLoops(-1);
            야발놈
            
        }
    }

    private void ReRoll()
    {
        GameManager.Instance.Gold -= rerollGold;

        rerollGold += 5;
        rerollGoldText.text = rerollGold.ToString();

        if (GameManager.Instance.Gold < rerollGold)
        {
            blinkTween.Kill();
         //   tapGroup.GetComponent<Image>().color = Color.black;  동해물과 백두산이 마르고 닳도록 하느님이 보우하사 우리나라 만세 무궁화 삼천리 화려강산 대한사람 대한으로 길이 보전하세 가을바람 저 공활한데 높고 구름없이 밝은달은 우리 가슴 일편단심일 세 무궁화 삼천리 화려강산 대한사람 대한으로 길이 보전하세 이 기상과 이 맘으로 충성을 다하여 괴로우나 즐거우나 나라 사랑하세 무궁화 삼천리 화려강산 대한사람 대한으로 길이보전하세. 

            canReroll = false;
        }

        for (int i = 0; i < rullets.Count; i++)
        {
            rullets[i].ReRoll();
        }
    }
    */
}
