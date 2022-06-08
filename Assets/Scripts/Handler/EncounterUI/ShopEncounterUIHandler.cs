using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ProductType
{
    None,
    Scroll,
    RulletPiece,
}

public class ShopEncounterUIHandler : MonoBehaviour
{
    private CanvasGroup mainPanel;
    public CanvasGroup buyPanel;

    public Button exitBtn, purchaseBtn, unselectBtn;

    public Image selectProductImg;
    public TextMeshProUGUI selectProductNameTxt, selectProductDesTxt;
    public Image strokeImg;
    public Image targetBGImg;
    public Image targetImg;
    public Transform skillIconTrans;
    [HideInInspector]public List<SkillDesIcon> desIcons = new List<SkillDesIcon>();
    public GradeInfoHandler gradeHandler;

    private bool isSelectPanelEnable;
    private int selectIdx;

    [SerializeField] private List<ProductInfo> products = new List<ProductInfo>();

    [Header("랜덤 상점 리스트")]
    public List<Scroll> scrollShopList = new List<Scroll>();

    private List<SkillPiece> randomRulletPiece = new List<SkillPiece>();
    private List<Scroll> randomScroll = new List<Scroll>();

    private List<int> soldIdxList = new List<int>();

    private GoldUIHandler goldUIHandler;
    private BattleScrollHandler battleScrollHandler;

    private BattleHandler bh;
    private InventoryHandler invenHandler;

    private void Awake()
    {
        mainPanel = GetComponent<CanvasGroup>();
        mainPanel.alpha = 0;
        mainPanel.blocksRaycasts = false;
        mainPanel.interactable = false;
    }

    public void Start()
    {
        skillIconTrans.GetComponentsInChildren(desIcons);
        mainPanel.alpha = 0;
        buyPanel.alpha = 0;

        bh = GameManager.Instance.battleHandler;
        goldUIHandler = GameManager.Instance.goldUIHandler;
        battleScrollHandler = bh.battleScroll;

        exitBtn.onClick.AddListener(OnExitBtnClick);
        purchaseBtn.onClick.AddListener(OnPurchaseBtnClick);
        unselectBtn.onClick.AddListener(()=>SelectProduct(-1));

        InitBtn();
    }

    //상점 열렸을때 호출
    public void StartEvent()
    {
        //goldUIHandler.ShowGoldUI(true);
        //battleScrollHandler.ShowScrollUI(open: true);
        isSelectPanelEnable = false;
        selectIdx = -1;

        InitShop();
        ShowPanel(true);
    }

    // 상점 버튼에 함수 추가
    private void InitBtn()
    {
        for (int i = 0; i < products.Count; i++)
        {
            int idx = i;
            products[idx].GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectProduct(idx);
            });
        }
    }

    //상점 랜덤으로 만들어줌
    private void InitShop()
    {
        SetAllButtonInterval(true);
        soldIdxList.Clear();
        for (int j = 0; j < GameManager.Instance.skillContainer.playerSkillPrefabs.Count; j++)
        {
            randomRulletPiece.Add(GameManager.Instance.skillContainer.playerSkillPrefabs[j].GetComponent<SkillPiece>());
        }
        randomScroll = new List<Scroll>(scrollShopList);

        for (int i = 0; i < products.Count; i++)
        {
            int idx = i;
            if (idx < 3)
            {
                SkillPiece rulletPiece = randomRulletPiece[Random.Range(0, randomRulletPiece.Count)];
                randomRulletPiece.Remove(rulletPiece);
                products[idx].SetProduct(ProductType.RulletPiece, null, rulletPiece);
            }
            else
            {
                Scroll scroll = randomScroll[Random.Range(0, randomScroll.Count)];
                randomScroll.Remove(scroll);
                products[idx].SetProduct(ProductType.Scroll, scroll);
            }
        }
        randomRulletPiece.Clear();
        randomScroll.Clear();
        SetButtonInterval(purchaseBtn, false);
    }

    #region OnButtonClick
    // 굳이 함수로 뺀이유 -> 구매됨 같은 효과 넣을꺼면 이 함수에 넣으라고
    private void OnPurchaseBtnClick()
    {
        PurchaseProduct();
    }

    private void OnExitBtnClick()
    {
        EndEvent();
    }
    #endregion

    private void PurchaseProduct()
    {
        ProductInfo selectProduct = products[selectIdx];
        if (GameManager.Instance.Gold < selectProduct.price)
        {
            GameManager.Instance.animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Fixed)
            .SetPosition(Vector3.zero)
            .Play("골드가 부족합니다!");
            return;
        }
        else
        {
            SetAllButtonInterval(false);
            GameManager.Instance.Gold -= selectProduct.price;


            switch (selectProduct.productType)
            {
                case ProductType.Scroll:
                    Scroll scroll = PoolManager.GetScroll(selectProduct.scroll.scrollType);
                    Image scrollImg = scroll.GetComponent<Image>();
                    scrollImg.color = new Color(1, 1, 1, 0);
                    scroll.transform.SetParent(transform);
                    scroll.GetComponent<RectTransform>().sizeDelta = Vector2.one * 300f;
                    scroll.transform.position = selectProductImg.transform.position + Vector3.down;
                    scroll.transform.localScale = Vector3.one;

                    bh.GetComponent<BattleScrollHandler>()
                         .GetScroll(scroll, () =>
                         {
                             buyPanel.DOFade(0, 0.5f).OnComplete(() =>
                             {
                                 buyPanel.blocksRaycasts = false;
                                 buyPanel.interactable = false;
                                 SetProductSold(selectIdx);
                                 SetAllButtonInterval(true, true);
                             });
                         }, true);
                    break;
                case ProductType.RulletPiece:

                    SkillPiece skillPiece = selectProduct.rulletPiece;//Instantiate(selectProduct.rulletPiece, Vector3.zero, Quaternion.identity).GetComponent<SkillPiece>();
                    GameManager.Instance.getPieceHandler.GetPiecePlayer(skillPiece,
                        () => SetAllButtonInterval(true, true),
                        () =>
                        {
                            Image skillImg = skillPiece.GetComponent<Image>();
                            skillImg.color = new Color(1, 1, 1, 0);
                            skillPiece.transform.SetParent(transform);
                            skillPiece.transform.localScale = Vector3.one;
                            skillPiece.transform.position = Vector3.zero;
                            skillPiece.transform.rotation = Quaternion.Euler(0, 0, 30f);
                            skillPiece.gameObject.SetActive(true);
                            Transform unusedInventoryTrm = bh.player.GetComponent<Inventory>().indicator.transform;
                            DOTween.Sequence()
                            .Append(skillImg.DOFade(1, 0.5f))
                            .Append(skillPiece.transform.DOMove(unusedInventoryTrm.position, 0.3f).SetDelay(0.3f))
                            .Join(skillPiece.transform.DOScale(Vector2.one * 0.1f, 0.3f))
                            .Join(skillPiece.GetComponent<Image>().DOFade(0f, 0.3f))
                            .OnComplete(() =>
                            {
                                buyPanel.DOFade(0, 0.5f).OnComplete(() =>
                                {
                                    buyPanel.blocksRaycasts = false;
                                    buyPanel.interactable = false;
                                    SetProductSold(selectIdx);
                                    SetAllButtonInterval(true, true);
                                });
                            });
                        });
                    break;
                default:
                    Debug.LogError("선택된 상품의 타입이 스크롤이나, 룰렛조각이 아닙니다.");
                    return;
            }
        }
    }

    // 상점 팔렸을때 
    private void SetProductSold(int selectProductIdx)
    {
        soldIdxList.Add(selectIdx);
        products[selectIdx].SetProductSold();
        selectIdx = -1;
        products[selectProductIdx].GetComponent<Button>().interactable = false;
    }

    private void SetButtonInterval(Button btn, bool enalbe)
    {
        btn.interactable = enalbe;
    }

    private void SetAllButtonInterval(bool enable, bool soldIsFalse = false)
    {
        for (int i = 0; i < products.Count; i++)
        {
            int idx = i;
            products[idx].GetComponent<Button>().interactable = enable;
        }
        exitBtn.interactable = enable;
        purchaseBtn.interactable = enable;

        if (soldIsFalse)
        {
            for (int i = 0; i < soldIdxList.Count; i++)
            {
                products[soldIdxList[i]].GetComponent<Button>().interactable = false;
            }
            isSelectPanelEnable = false;
            SetButtonInterval(purchaseBtn, false);
        }
    }

    // 품목 선택시 애니메이션, 보여주기
    private void SelectProduct(int selectIdx)
    {
        if (selectIdx < 0)
        {
            if (this.selectIdx == selectIdx)
            {
                return;
            }

            DOTween.Sequence()
                   .Append(buyPanel.DOFade(0, 0.3f).OnComplete(() =>
                   {
                       buyPanel.blocksRaycasts = false;
                       buyPanel.interactable = false;
                       isSelectPanelEnable = false;
                   }));
        }
        else
        {
            SetSelectPanel(products[selectIdx]);
            buyPanel.blocksRaycasts = true;
            buyPanel.interactable = true;
            buyPanel.DOFade(1, 0.3f)
            .OnComplete(() =>
            {
                isSelectPanelEnable = true;
                SetButtonInterval(purchaseBtn, true);
            });
        }
        this.selectIdx = selectIdx;
    }

    public void SetSelectPanel(ProductInfo product)
    {
        Sprite productSpr = null;

        if (invenHandler == null)
        {
            invenHandler = GameManager.Instance.inventoryHandler;
        }

        if (product.productType.Equals(ProductType.RulletPiece))
        {
            productSpr = product.skillImg.sprite;
            Sprite stroke = invenHandler.pieceBGStrokeSprDic[product.rulletPiece.currentType];
            Sprite targetBG = invenHandler.targetBGSprDic[product.rulletPiece.currentType];
            Sprite targetIcon = invenHandler.targetIconSprDic[product.rulletPiece.skillRange];


            skillIconTrans.gameObject.SetActive(true);
            strokeImg.gameObject.SetActive(true);
            targetBGImg.gameObject.SetActive(true);
            targetImg.gameObject.SetActive(true);

            strokeImg.sprite = stroke;
            targetBGImg.sprite = targetBG;
            targetImg.sprite = targetIcon;

            List<DesIconInfo> desInfos = product.rulletPiece.GetDesIconInfo();
            ShowDesIcon(desInfos, product.rulletPiece);

            gradeHandler.SetGrade(product.rulletPiece.skillGrade);
        }
        else
        {
            productSpr = product.scrollImg.sprite;


            skillIconTrans.gameObject.SetActive(false);
            strokeImg.gameObject.SetActive(false);
            targetBGImg.gameObject.SetActive(false);
            targetImg.gameObject.SetActive(false);
        }
        selectProductImg.sprite = productSpr;
        selectProductNameTxt.text = product.productName;
        selectProductDesTxt.text = product.productDes;
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

            Sprite icon = bh.battleUtil.GetDesIcon(skillPiece, type);

            desIcons[i].SetIcon(icon, desInfos[i].value);
        }
    }

    private void EndEvent()
    {
        isSelectPanelEnable = false;
        ShowPanel(false, () =>
        {
            buyPanel.alpha = 0;
        });

        //goldUIHandler.ShowGoldUI(false);
        //battleScrollHandler.ShowScrollUI(open: false);

        for (int i = 0; i < products.Count; i++)
        {
            int idx = i;
            if (idx < 3)
            {
                SkillPiece sp = products[idx].rulletPiece;
                if (sp != null)
                {
                    Destroy(products[idx].rulletPiece.gameObject);
                }
            }
            else
            {
                break;
            }
        }
        GameManager.Instance.EndEncounter();
    }

    public void ShowPanel(bool enable, Action onComplecteEvent = null)
    {
        mainPanel.DOFade(enable ? 1 : 0, 0.5f)
        .OnComplete(() =>
        {
            mainPanel.blocksRaycasts = enable;
            mainPanel.interactable = enable;
            onComplecteEvent?.Invoke();
        });
    }
}