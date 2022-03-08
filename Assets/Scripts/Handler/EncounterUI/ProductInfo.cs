using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductInfo : MonoBehaviour
{
    public ProductType productType;

    [HideInInspector]public Scroll scroll;
    [HideInInspector]public RulletPiece rulletPiece;

    public Image productImg;
    public Text productPriceTxt;
    [HideInInspector] public string productName, productDes;

    public void SetProduct(ProductType type, Scroll scroll = null, RulletPiece rulletPiece = null)
    {
        if((scroll == null) == (rulletPiece == null))
        {
            Debug.LogError("상품은 한 종류만 되어야됩니다.");
        }
        switch (type)
        {
            case ProductType.Scroll:
                productImg.GetComponent<RectTransform>().sizeDelta = Vector2.one*250;
                productImg.sprite = scroll.GetComponent<Image>().sprite;
                productName = scroll.ScrollName;
                productDes = scroll.ScrollDes;
                productPriceTxt.text = "???";
                break;
            case ProductType.RulletPiece:
                productImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * 320;
                productImg.sprite = rulletPiece.skillImg.sprite;
                productName = rulletPiece.PieceName;
                productDes = rulletPiece.PieceDes;
                productPriceTxt.text = "???";
                break;
            default:
                Debug.LogError("상품타입이 없습니다");
                break;
        }
    }
}
