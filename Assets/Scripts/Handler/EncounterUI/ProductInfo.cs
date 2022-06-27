using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductInfo : MonoBehaviour
{
    public ProductType productType = ProductType.NONE;

    [HideInInspector] public PlayerSkill skill;
    [HideInInspector] public SkillPiece rulletPiece;

    public Image scrollImg;

    public CanvasGroup skillCardCvsGroup;
    public Transform skillIconsTrm;
    [HideInInspector]public List<SkillDesIcon> desIcons = new List<SkillDesIcon>();
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDesText;
    public Image strokeImg;
    public Image targetBGImg;
    public Image targetImg;
    public Image skillImg;
    public GradeInfoHandler gradeHandler;

    public Image productPriceImg;
    public Text productPriceTxt;
    [HideInInspector] public string productName, productDes;
    public int price;

    private BattleHandler battleHandler;

    private void Start()
    {
        battleHandler = GameManager.Instance.battleHandler;
        skillIconsTrm.GetComponentsInChildren(desIcons);
    }

    public void SetProduct(ProductType type, PlayerSkill skill = null, SkillPiece rulletPiece = null)
    {
        productPriceImg.gameObject.SetActive(true);
        productType = type;
        if ((skill == null) == (rulletPiece == null))
        {
            Debug.LogError("스크롤 혹은 스킬로 상점 품목을 설정해주세요");
        }
        switch (productType)
        {
            case ProductType.SKILL:
                skillCardCvsGroup.alpha = 0;
                scrollImg.gameObject.SetActive(true);

                this.skill = skill;

                productName = skill.skillName;
                productDes = skill.skillDes;
                price = 10;

                scrollImg.sprite = skill.iconSpr;
                productPriceTxt.text = price.ToString();
                break;
            case ProductType.RULLETPIECE:
                SkillPiece skillPiece = Instantiate(rulletPiece, Vector3.zero, Quaternion.identity).GetComponent<SkillPiece>();
                skillPiece.Owner = GameManager.Instance.GetPlayer().GetComponent<Inventory>();
                skillPiece.gameObject.SetActive(false);

                scrollImg.gameObject.SetActive(false);
                skillCardCvsGroup.alpha = 1;

                this.rulletPiece = skillPiece;

                productName = skillPiece.PieceName;
                productDes = skillPiece.PieceDes;
                switch (skillPiece.skillGrade)
                {
                    case GradeInfo.Normal:
                        price = Random.Range(10, 21);
                        break;
                    case GradeInfo.Epic:
                        price = Random.Range(26, 36);
                        break;
                    case GradeInfo.Legend:
                        price = Random.Range(72, 80);
                        break;
                    default:
                        price = 10;
                        break;
                }

                skillImg.sprite = skillPiece.cardBG;
                cardNameText.text = productName;
                cardDesText.text = productDes;
                productPriceTxt.text = price.ToString();

                List<DesIconInfo> desInfos = skillPiece.GetDesIconInfo();
                ShowDesIcon(desInfos, skillPiece);

                InventoryHandler inven = GameManager.Instance.inventoryHandler;

                strokeImg.sprite = inven.pieceBGStrokeSprDic[skillPiece.currentType];
                targetBGImg.sprite = inven.targetBGSprDic[skillPiece.currentType];
                targetImg.sprite = inven.targetIconSprDic[skillPiece.skillRange];
                gradeHandler.SetGrade(skillPiece.skillGrade);

                break;
            default:
                Debug.LogError("��ǰŸ���� �����ϴ�");
                break;
        }
    }

    public void SetProductSold()
    {
        if ((skill == null) == (rulletPiece == null))
        {
            Debug.LogError("��ǰ�� �� ������ �Ǿ�ߵ˴ϴ�.");
            return;
        }

        //scroll = null;
        rulletPiece = null;
        skillCardCvsGroup.alpha = 0;
        scrollImg.gameObject.SetActive(false);
        productPriceImg.gameObject.SetActive(false);
        productDes = "";
        productName = "";
        productPriceTxt.text = "SOLD";
        productPriceTxt.GetComponent<RectTransform>().sizeDelta = new Vector2(240f, productPriceTxt.GetComponent<RectTransform>().sizeDelta.y);
        productType = ProductType.NONE;
    }

    private void ShowDesIcon(List<DesIconInfo> desInfos, SkillPiece skillPiece)
    {
        for (int i = 0; i < 3; i++)
        {
            DesIconType type = desInfos[i].iconType;

            if (type.Equals(DesIconType.None))
            {
                desIcons[i].gameObject.SetActive(false);
                continue;
            }
            else
            {
                desIcons[i].gameObject.SetActive(true);
            }

            Sprite icon = battleHandler.battleUtil.GetDesIcon(skillPiece, type);

            desIcons[i].SetIcon(icon, desInfos[i].value);
        }
    }
}
