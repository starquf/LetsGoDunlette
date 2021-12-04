using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSlot : MonoBehaviour
{
    public EComboType comboType;
    public Image iconImage;

    public void SetData(RulletPiece piece)
    {
        this.comboType = piece.comboType;
        this.iconImage.sprite = piece.skillImg.sprite;
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }
}
