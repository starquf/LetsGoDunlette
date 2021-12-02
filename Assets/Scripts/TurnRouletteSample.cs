using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;  

public class TurnRouletteSample : MonoBehaviour
{
    [Header("References")]
    public Image playerImage;
    public Image enemyImage;
    public RectTransform pinRectTrm;

    private RectTransform rectTransform;

    [Header("Setting")]
    [Space]
    public float playerChance;
    public float pinMoveSpeed;

    private bool IsPlayer;
    private float delta;
    private int moveDir;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Start()
    {
        GenerateImage();

        StartCoroutine(StartPinMove());
    }

    private IEnumerator StartPinMove()
    {
        float maxX = rectTransform.rect.width;
        delta = 0;
        moveDir = 1; // 1 오른 -1 왼

        while (true)
        {
            delta += Time.deltaTime * moveDir * pinMoveSpeed;

            if (delta >= 1)
                moveDir = -1;
            else if (delta <= 0)
                moveDir = 1;

            pinRectTrm.anchoredPosition = new Vector3(Mathf.Lerp(0, maxX, delta), -25, 0);
            yield return null;
        }
    }

    public void OnClickBtn()
    {
        StartCoroutine(StartGetResult());
    }

    private IEnumerator StartGetResult()
    {
        IsPlayer = GetIsPlayer();

        yield return StartCoroutine(StopPin(IsPlayer));
        print("넘어옴");
    }

    private IEnumerator StopPin(bool result)
    {
        float destination = 0;
        if(result)
        {
            destination = UnityEngine.Random.Range(0, playerChance * 0.01f);
        }
        else
        {
            destination = UnityEngine.Random.Range(playerChance * 0.01f, 1f);
        }

        float tempDelta = 0;
        bool breakPoint = false;
        while (!breakPoint)
        {
            tempDelta += Time.deltaTime * 0.001f;
            pinMoveSpeed = Mathf.Lerp(pinMoveSpeed, 0, tempDelta);

            if (Mathf.Abs(delta - destination) <= 0.01f && moveDir == -1)
            {
                DOTween.To(() => pinMoveSpeed, x => pinMoveSpeed = x, 0, 0.3f).OnComplete(() =>
                {
                    breakPoint = true;
                });
            }

            yield return null;
        }
        yield return null;
    }

    private bool GetIsPlayer() // 플레이어가 걸린다면  true 를 반환함
    {
        float randNum = UnityEngine.Random.Range(0, 100);
        if(randNum <= playerChance)
        {
            print("Player");
            return true;
        }
        else
        {
            print("Enemy");
            return false;
        }
    }

    private void GenerateImage()
    {
        float playerImageFillValue = playerChance * 0.01f;
        playerImage.fillAmount = Mathf.Clamp01(playerImageFillValue);

    }

    private void OnValidate() //스크립트 에서 값이 바뀔때 호출됨 Editor 함수임
    {

    }
}
