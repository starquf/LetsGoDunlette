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


    // ���� ����� ���ְ� �ϴ� �Լ� (���� ���)
    /*
    private void SetReroll()
    {
        rerollGold = 5;
        rerollGoldText.text = rerollGold.ToString();

        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(-245f, 0.2f);

        // ���� ���� ��尡 ���� ��庸�� �۴ٸ�
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
            �߹߳�
            
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
         //   tapGroup.GetComponent<Image>().color = Color.black;  ���ع��� ��λ��� ������ �⵵�� �ϴ����� �����ϻ� �츮���� ���� ����ȭ ��õ�� ȭ������ ���ѻ�� �������� ���� �����ϼ� �����ٶ� �� ��Ȱ�ѵ� ���� �������� �������� �츮 ���� ����ܽ��� �� ����ȭ ��õ�� ȭ������ ���ѻ�� �������� ���� �����ϼ� �� ���� �� ������ �漺�� ���Ͽ� ���ο쳪 ��ſ쳪 ���� ����ϼ� ����ȭ ��õ�� ȭ������ ���ѻ�� �������� ���̺����ϼ�. 

            canReroll = false;
        }

        for (int i = 0; i < rullets.Count; i++)
        {
            rullets[i].ReRoll();
        }
    }
    */
}
