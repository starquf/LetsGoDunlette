using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetSelectHandler : MonoBehaviour
{
    public CanvasGroup selectCvs;

    public GameObject targetIconObj;

    public Transform contextTrans;

    private BattleHandler bh;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        PoolManager.CreatePool<TargetIcon>(targetIconObj, contextTrans, 3);

        ShowPanel(false);
    }

    public void SelectTarget(Action<EnemyHealth> onEndSelect)
    {
        ShowPanel(true);

        InitTarget(onEndSelect);
    }

    private void InitTarget(Action<EnemyHealth> onEndSelect)
    {
        for (int i = 0; i < contextTrans.childCount; i++)
        {
            contextTrans.GetChild(i).gameObject.SetActive(false);
        }

        List<EnemyHealth> enemys = bh.enemys;

        for (int i = 0; i < enemys.Count; i++)
        {
            EnemyHealth enemy = enemys[i];
            TargetIcon targetIcon = PoolManager.GetItem<TargetIcon>();

            targetIcon.transform.SetAsLastSibling();

            targetIcon.Init(enemy.iconSpr, () => 
            {
                EndSelect();
                onEndSelect?.Invoke(enemy);
            });
        }
    }

    private void EndSelect()
    {
        ShowPanel(false);
    }

    public void ShowPanel(bool enable)
    {
        selectCvs.DOFade(enable ? 1f : 0f, 0.33f);
        selectCvs.blocksRaycasts = enable;
        selectCvs.interactable = enable;
    }
}
