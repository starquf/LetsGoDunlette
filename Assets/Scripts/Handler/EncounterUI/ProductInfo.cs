using UnityEngine;
using UnityEngine.UI;

public class ProductInfo : MonoBehaviour
{
    public ProductType productType = ProductType.None;

    [HideInInspector] public Scroll scroll;
    [HideInInspector] public RulletPiece rulletPiece;

    public Image productImg;
    public Image productPriceImg;
    public Text productPriceTxt;
    [HideInInspector] public string productName, productDes;
    public int price;

    public void SetProduct(ProductType type, Scroll scroll = null, SkillPiece rulletPiece = null)
    {
        productImg.color = Color.white;
        productPriceImg.gameObject.SetActive(true);
        productType = type;
        productPriceTxt.GetComponent<RectTransform>().sizeDelta = new Vector2(155f, productPriceTxt.GetComponent<RectTransform>().sizeDelta.y);
        if ((scroll == null) == (rulletPiece == null))
        {
            Debug.LogError("��ǰ�� �� ������ �Ǿ�ߵ˴ϴ�.");
        }
        switch (productType)
        {
            case ProductType.Scroll:
                this.scroll = scroll;
                productImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * 200;
                productImg.sprite = scroll.GetComponent<Image>().sprite;
                productName = scroll.ScrollName;
                productDes = scroll.ScrollDes;
                price = 10;
                productPriceTxt.text = price.ToString();
                break;
            case ProductType.RulletPiece:
                this.rulletPiece = rulletPiece;
                productImg.GetComponent<RectTransform>().sizeDelta = Vector2.one * 250;
                productImg.sprite = rulletPiece.transform.Find("SkillIcon").GetComponent<Image>().sprite;
                productName = rulletPiece.PieceName;
                productDes = rulletPiece.PieceDes;
                price = 10;
                productPriceTxt.text = price.ToString();
                break;
            default:
                Debug.LogError("��ǰŸ���� �����ϴ�");
                break;
        }
    }

    public void SetProductSold()
    {
        if ((scroll == null) == (rulletPiece == null))
        {
            Debug.LogError("��ǰ�� �� ������ �Ǿ�ߵ˴ϴ�.");
            return;
        }

        scroll = null;
        rulletPiece = null;
        productImg.color = Color.clear;
        productPriceImg.gameObject.SetActive(false);
        productDes = "";
        productName = "";
        productPriceTxt.text = "SOLD";
        productPriceTxt.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, productPriceTxt.GetComponent<RectTransform>().sizeDelta.y);
        productType = ProductType.None;
    }
}
