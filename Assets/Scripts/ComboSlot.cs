using UnityEngine;
using UnityEngine.UI;

public class ComboSlot : MonoBehaviour
{
    // 현재 콤보 슬롯의 타입
    public ElementalType comboType;
    // 아이콘의 이미지
    public Image iconImage;

    // 조각의 타입으로 슬롯을 바꿔주는 함수
    public void SetData(RulletPiece piece)
    {
        comboType = piece.currentType;
        iconImage.sprite = piece.skillImg.sprite;
    }

    // 슬롯을 없에는 함수
    public void Delete()
    {
        Destroy(gameObject);
    }
}
