using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Anim_TextUp : AnimObj
{
    public Text textValue;

    public void SetType(TextUpAnimType type)
    {
        switch (type)
        {
            case TextUpAnimType.Damage:
                textValue.GetComponent<RectTransform>().localScale = new Vector2(0.0025f, 0.0025f);
                break;
            case TextUpAnimType.GetMoney:
                textValue.GetComponent<RectTransform>().localScale = new Vector2(0.002f, 0.002f);
                break;
            default:
                break;
        }
    }

    public void Play(string value)
    {
        textValue.text = value;
        base.Play();
    }
}
