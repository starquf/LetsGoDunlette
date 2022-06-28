using System.Collections;
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
            return;

        for (int i = useCountImg.Count - 1; i >= count; i--)
        {
            useCountImg[i].gameObject.SetActive(false);
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
}
