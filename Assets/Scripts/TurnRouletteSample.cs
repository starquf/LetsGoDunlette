using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TurnRouletteSample : MonoBehaviour
{
    [Header("�ɼ�")]
    [SerializeField][Range(0f, 100f)] private float playerChance = 50f; //�÷��̾� Ȯ��
    [SerializeField] private AnimationCurve randCurve;
    [Header("���۷���")]
    [SerializeField] private Image rouletteImage;
    [SerializeField] private Sprite rouletteDefault;
    [SerializeField] private Sprite[] rouletteLight; //¦�� �Ͼ� Ȧ�� ����

    private int startIndex = default;
    private bool result;

    private int rouletteSpriteLength; //�귿 ��������Ʈ ����

    private void Start()
    {
        if (rouletteImage == null)
        {
            rouletteImage = GetComponent<Image>();
        }

        rouletteSpriteLength = rouletteLight.Length; //����ȭ�� ���� ĳ��

        PlayRoulette();
    }

    public void PlayRouletteBtn()
    {
        PlayRoulette();
    }

    public void PlayRoulette(Action<bool> action = null) // true = �÷��̾�, false = ��
    {
        StopAllCoroutines();

        result = ChoosePlayerOrEnemy();

        StartCoroutine(ShowRoulette(action));
    }

    private IEnumerator ShowRoulette(Action<bool> action)
    {
        startIndex = Random.Range(0, rouletteSpriteLength); //�Һ��� ������ Index
        
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
                if (result) //true �� �÷��̾� �ϱ� �÷��̾� ¦�� index ������
                {
                    if (rouletteIndex % 2 != 0)  //¦���� �ƴ϶��
                    {
                        yield return new WaitForSeconds(waitTime); //��ٸ���
                        continue;
                    }
                }
                else
                {
                    if (rouletteIndex % 2 == 0)  //¦�����
                    {
                        yield return new WaitForSeconds(waitTime); //��ٸ���
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
