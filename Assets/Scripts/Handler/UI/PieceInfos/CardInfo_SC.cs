using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo_SC : MonoBehaviour
{
    public Image cardBgImg;
    public Image strokeImg;
    public GradeInfoHandler gradeHandler;
    public Image targetImg;
    public Image targetBGImg;
    public TextMeshProUGUI nameText;

    [HideInInspector]
    public Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void SetInfo(Sprite cardBg, Sprite stroke, GradeInfo grade, Sprite target, Sprite targetBG, string name)
    {
        cardBgImg.sprite = cardBg;
        strokeImg.sprite = stroke;
        gradeHandler.SetGrade(grade);
        targetImg.sprite = target;
        targetBGImg.sprite = targetBG;
        nameText.text = name;
    }
}
