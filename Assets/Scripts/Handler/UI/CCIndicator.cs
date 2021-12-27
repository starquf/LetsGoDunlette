using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCIndicator : MonoBehaviour
{
    private Image ccImg;
    private Text turnText;

    private void Awake()
    {
        ccImg = GetComponent<Image>();
        turnText = transform.GetChild(0).GetComponent<Text>();
    }

    public void SetImg(Sprite img)
    {
        ccImg.sprite = img;
    }

    public void SetText(int turn)
    {
        turnText.text = turn.ToString();
    }
}
