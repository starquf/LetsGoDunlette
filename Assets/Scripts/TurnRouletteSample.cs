using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TurnRouletteSample : MonoBehaviour
{
    [Header("옵션")]
    [SerializeField][Range(0f, 100f)] private float playerChance = 50f; //플레이어 확률
    [SerializeField] private AnimationCurve randCurve;
    [Header("레퍼런스")]
    [SerializeField] private Image rouletteImage;
    [SerializeField] private Sprite rouletteDefault;
    [SerializeField] private Sprite[] rouletteLight; //짝수 하양 홀수 빨강

    private int startIndex = default;
    private bool result;

    private int rouletteSpriteLength; //룰렛 스프라이트 개수

    private void Start()
    {
        if (rouletteImage == null)
        {
            rouletteImage = GetComponent<Image>();
        }

        rouletteSpriteLength = rouletteLight.Length; //최적화를 위해 캐싱

        PlayRoulette();
    }

    public void PlayRouletteBtn()
    {
        PlayRoulette();
    }

    public void PlayRoulette(Action<bool> action = null) // true = 플레이어, false = 적
    {
        StopAllCoroutines();

        result = ChoosePlayerOrEnemy();

        StartCoroutine(ShowRoulette(action));
    }

    private IEnumerator ShowRoulette(Action<bool> action)
    {
        startIndex = Random.Range(0, rouletteSpriteLength); //불빛이 시작할 Index
        
        float waitTime = 0.05f;
        float maxWaitTime = 1f;

        rouletteImage.sprite = rouletteLight[startIndex];

        int rouletteIndex = startIndex;

        int randNum = Random.Range(10, 50);
        for (int i = 0; i < randNum; i++)
        {
            rouletteIndex = (rouletteIndex + 1) % 12;
            SetRouletteLight(rouletteIndex);
            yield return new WaitForSeconds(waitTime);
        }

        while (true)
        {
            rouletteIndex = (rouletteIndex + 1) % 12;
            SetRouletteLight(rouletteIndex);

            if (waitTime > maxWaitTime)
            {
                if (result) //true 면 플레이어 니깐 플레이어 짝수 index 여야함
                {
                    if (rouletteIndex % 2 != 0)  //짝수가 아니라면
                    {
                        yield return new WaitForSeconds(waitTime); //기다리고
                        continue;
                    }
                }
                else
                {
                    if (rouletteIndex % 2 == 0)  //짝수라면
                    {
                        yield return new WaitForSeconds(waitTime); //기다리고
                        continue;
                    }
                }

                action?.Invoke(result);

                //print(result);

                break;
            }

            waitTime += waitTime;

            yield return new WaitForSeconds(waitTime);
        }

        StartCoroutine(BlinkLight(rouletteIndex));
    }

    private IEnumerator BlinkLight(int rouletteIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            SetRouletteLightDefault();
            yield return new WaitForSeconds(0.5f);
            SetRouletteLight(rouletteIndex);
        }
    }

    private void SetRouletteLightDefault()
    {
        rouletteImage.sprite = rouletteDefault;
    }

    private void SetRouletteLight(int index)
    {
        rouletteImage.sprite = rouletteLight[index];
    }

    private bool ChoosePlayerOrEnemy()
    {
        if (CurveWeightedRandom() <= playerChance)
        {
            return true;
        }

        return false;
    }

    private float CurveWeightedRandom()
    {
        return randCurve.Evaluate(Random.value) * 100;
    }
}
