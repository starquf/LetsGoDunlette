using UnityEngine;
using UnityEngine.UI;

public class ComboSlot : MonoBehaviour
{
    // ���� �޺� ������ Ÿ��
    public ElementalType comboType;
    // �������� �̹���
    public Image iconImage;

    // ������ Ÿ������ ������ �ٲ��ִ� �Լ�
    public void SetData(RulletPiece piece)
    {
        comboType = piece.currentType;
        iconImage.sprite = piece.skillImg.sprite;
    }

    // ������ ������ �Լ�
    public void Delete()
    {
        Destroy(gameObject);
    }
}
