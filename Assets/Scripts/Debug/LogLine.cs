using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogLine : MonoBehaviour
{
    public Image startImg;
    public Text processText;
    public Image targetImg;
    public Image processImg;
    public Text msgText;
    public Image lineImg;

    public void Init(LogType type, bool hasLine)
    {
        ResetLog();

        switch (type)
        {
            case LogType.OnlyText:
                msgText.gameObject.SetActive(true);

                break;

            case LogType.ImgImgToTarget:
                startImg.gameObject.SetActive(true);
                processImg.gameObject.SetActive(true);
                targetImg.gameObject.SetActive(true);

                break;

            case LogType.ImgTextToTarget:
                startImg.gameObject.SetActive(true);
                processText.gameObject.SetActive(true);
                targetImg.gameObject.SetActive(true);
                break;

            case LogType.ImageText:
                startImg.gameObject.SetActive(true);
                processText.gameObject.SetActive(true);

                break;
        }

        if (hasLine) lineImg.gameObject.SetActive(true);
    }

    private void ResetLog()
    {
        startImg.gameObject.SetActive(false);
        processText.gameObject.SetActive(false);
        targetImg.gameObject.SetActive(false);
        processImg.gameObject.SetActive(false);
        msgText.gameObject.SetActive(false);
        lineImg.gameObject.SetActive(false);
    }
}
