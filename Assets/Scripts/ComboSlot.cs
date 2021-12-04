using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSlot : MonoBehaviour
{
    // ���� �޺� ������ Ÿ��
    public EComboType comboType;
    // �������� �̹���
    public Image iconImage;

    // ������ Ÿ������ ������ �ٲ��ִ� �Լ�
    public void SetData(RulletPiece piece)
    {
        this.comboType = piece.comboType;
        this.iconImage.sprite = piece.skillImg.sprite;
    }

    // ������ ������ �Լ�
    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
