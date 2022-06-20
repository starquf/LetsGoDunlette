using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

public class LobbyUIPanel : MainUIPanel
{
    public Button startBtn;

    public Transform playerTrans;
    public Transform endPos;

    protected void Start()
    {
        startBtn.onClick.AddListener(() =>
        {
            SetInteract(false);

            PlayAnim();

            mainUIHandler.SetFade(true, false, () =>
            {
                GameManager.Instance.ResetScene();
                LoadingSceneHandler.LoadScene("SeunghwanScene");
            });
        });
    }

    private void PlayAnim()
    {
        Vector2 startPos = playerTrans.position;

        Vector3 randomPos = startPos + (Random.insideUnitCircle * 50f);

        
    }
}
