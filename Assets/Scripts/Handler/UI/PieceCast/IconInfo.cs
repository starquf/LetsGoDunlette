using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI iconName;
    [SerializeField] private TextMeshProUGUI iconDes;

    public void Init(Sprite icon, string name, string des)
    {
        this.icon.sprite = icon;
        iconName.text = name;
        iconDes.text = des;
    }
}
