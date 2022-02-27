using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EnemyIndicator : MonoBehaviour
{
    public Transform indicatorTrans;

    private List<Text> indicatorList = new List<Text>();

    public void ShowText(string text,Action action = null)
    {
        Text indiText = PoolManager.GetItem<EnemyIndicatorText>().GetComponent<Text>();
        indiText.transform.SetParent(indicatorTrans);
        indiText.transform.localScale = Vector3.one;
        indiText.text = text;

        indiText.DOFade(1f, 0.5f)
            .From(0f);

        indiText.transform.localPosition = new Vector2(0f, (indicatorList.Count + 1) * indiText.rectTransform.rect.height);
        indiText.transform.DOLocalMoveY(-100f, 0.5f)
            .From(true);
        GameManager.Instance.battleHandler.battleUtil.SetTimer(1.0f, action);

        indicatorList.Add(indiText);
    }


    public void HideText()
    {
        for (int i = indicatorList.Count - 1; i >= 0; i--)
        {
            GameObject indicator = indicatorList[i].gameObject;

            indicatorList[i].DOFade(0f, 0.3f)
                .From(1f)
                .OnComplete(() =>
                {
                    indicator.SetActive(false);
                });

            indicatorList.RemoveAt(i);
        }
    }
}
