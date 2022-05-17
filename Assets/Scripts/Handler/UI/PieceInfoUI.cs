using UnityEngine;
using UnityEngine.UI;

public class PieceInfoUI : MonoBehaviour
{
    private Image image;
    public Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    public void SetSkillIcon(Sprite skillSpr)
    {
        image.sprite = skillSpr;
    }
}
