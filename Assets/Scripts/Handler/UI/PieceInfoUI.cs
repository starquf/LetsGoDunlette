using UnityEngine;
using UnityEngine.UI;

public class PieceInfoUI : MonoBehaviour
{
    private Image icon;
    private Image stroke;

    public Button button;

    private void Awake()
    {
        icon = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        stroke = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        button = GetComponent<Button>();
    }

    public void SetSkillIcon(Sprite skillSpr, Sprite strokeSpr)
    {
        icon.sprite = skillSpr;
        stroke.sprite = strokeSpr;
    }
}
