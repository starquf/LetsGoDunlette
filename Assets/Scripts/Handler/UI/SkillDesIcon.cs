using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDesIcon : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI value;

    public void SetIcon(Sprite img, string msg)
    {
        icon.sprite = img;
        value.text = msg;
    }
}
