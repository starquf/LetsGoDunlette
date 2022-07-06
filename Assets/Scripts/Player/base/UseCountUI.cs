using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseCountUI : MonoBehaviour
{
    private List<Image> useCountImg = new List<Image>();

    public Sprite enableSpr;
    public Sprite disableSpr;

    private void Awake()
    {
        GetComponentsInChildren(useCountImg);

        gameObject.SetActive(false);
    }

    public void Init(int count)
    {
        if (count > 5)
        {
            return;
        }

        for (int i = 0; i < useCountImg.Count; i++)
        {
            useCountImg[i].gameObject.SetActive(i < count);
        }

        SetUseCount(0);
    }

    public void SetUseCount(int remainCount)
    {
        for (int i = 0; i < useCountImg.Count; i++)
        {
            useCountImg[i].sprite = i >= remainCount ? disableSpr : enableSpr;
        }
    }

    public void ShowHighlight()
    {
        for (int i = 0; i < useCountImg.Count; i++)
        {
            if (useCountImg[i].sprite == disableSpr)
            {
                DOTween.Sequence()
                    .Append(useCountImg[i].transform.DOScale(Vector3.one * 1.6f, 0.15f))
                    .Append(useCountImg[i].transform.DOScale(Vector3.one, 0.15f));

                return;
            }
        }
    }
}
