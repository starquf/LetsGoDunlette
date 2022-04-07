using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
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
    public CanvasGroup selectPanel;

    public Button exitBtn, purchaseBtn;

    public Image selectProductImg;
    public Text selectProductNameTxt, selectProductDesTxt;

    private bool isSelectPanelEnable;
    private int selectIdx;

    [SerializeField] private List<ProductInfo> products;

    [Header("랜덤 상점 리스트")]
    public List<Scroll> scrollShopList;

    private List<SkillPiece> randomRulletPiece;
    private List<Scroll> randomScroll;

    private void Awake()
    {
        mainPanel = GetComponent<CanvasGroup>();
    }

    public void Start()
    {
        exitBtn.onClick.AddListener(OnExitBtnClick);
        purchaseBtn.onClick.AddListener(OnPurchaseBtnClick);

        InitBtn();
    }

    //상점 열렸을때 호출
    public void StartEvent()
    {
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
            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = Vector3.zero;
            textEffect.Play("골드가 부족합니다!");
            return;
        }
        else
        {
            SetButtonInterval(false);
            GameManager.Instance.Gold -= selectProduct.price;


            switch (selectProduct.productType)
            {
                case ProductType.Scroll:
                    Scroll scroll = PoolManager.GetScroll(selectProduct.scroll.scrollType);
                    Image scrollImg = scroll.GetComponent<Image>();
                    scrollImg.color = new Color(1, 1, 1, 0);
                    scroll.transform.SetParent(this.transform);
                    scroll.GetComponent<RectTransform>().sizeDelta = Vector2.one * 400f;
                    scroll.transform.position = selectProductImg.transform.position;
                    scroll.transform.localScale = Vector3.one;

                    GameManager.Instance.battleHandler.GetComponent<BattleScrollHandler>().GetScroll(scroll, () => {
                        selectPanel.DOFade(0, 0.5f);
                        SetProductSold(selectIdx);
                        SetButtonInterval(true);
                    }, true);

                    break;
                case ProductType.RulletPiece:
                    SkillPiece skillPiece = Instantiate(selectProduct.rulletPiece, Vector3.zero, Quaternion.identity).GetComponent<SkillPiece>();

                    Image skillImg = skillPiece.GetComponent<Image>();
                    skillImg.color = new Color(1, 1, 1, 0);
                    skillPiece.transform.SetParent(this.transform);
                    skillPiece.transform.localScale = Vector3.one;
                    skillPiece.transform.position = Vector3.zero;
                    skillPiece.transform.rotation = Quaternion.Euler(0, 0, 30f);
                    

                    BattleHandler battleHandler = GameManager.Instance.battleHandler;
                    Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;

                    DOTween.Sequence()
                     .Append(skillImg.DOFade(1, 0.5f))
                    .Append(skillPiece.transform.DOMove(unusedInventoryTrm.position, 0.3f).SetDelay(0.3f))
                    .Join(skillPiece.transform.DOScale(Vector2.one * 0.1f, 0.3f))
                    .Join(skillPiece.GetComponent<Image>().DOFade(0f, 0.3f))
                    .OnComplete(() =>
                    {
                        Inventory owner = battleHandler.player.GetComponent<Inventory>();
                        skillPiece.gameObject.SetActive(false);
                        skillPiece.owner = owner;
                        GameManager.Instance.inventoryHandler.AddSkill(skillPiece);
                        skillImg.color = Color.white;

                        selectPanel.DOFade(0, 0.5f);
                        SetProductSold(selectIdx);
                        SetButtonInterval(true);
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
        products[selectIdx].SetProductSold();
        selectIdx = -1;
        products[selectProductIdx].GetComponent<Button>().interactable = false;
    }

    private void SetButtonInterval(bool enable)
    {
        for (int i = 0; i < products.Count; i++)
        {
            int idx = i;
            products[idx].GetComponent<Button>().interactable = enable;
        }
        exitBtn.interactable = enable;
        purchaseBtn.interactable = enable;
    }

    // 품목 선택시 애니메이션, 보여주기
    private void SelectProduct(int selectIdx){
        if (isSelectPanelEnable)
        {
            if (this.selectIdx == selectIdx)
                return;
            DOTween.Sequence()
                   .Append(selectPanel.DOFade(0, 0.3f).OnComplete(() => {
                       SetSelectPanel(products[selectIdx]);
                   }))
                   .Append(selectPanel.DOFade(1, 0.3f));
        }
        else
        {
            SetSelectPanel(products[selectIdx]);
            selectPanel.DOFade(1, 0.3f);
            isSelectPanelEnable = true;
        }
        this.selectIdx = selectIdx;
    }

    public void SetSelectPanel(ProductInfo product)
    {
        selectProductImg.sprite = product.productImg.sprite;
        selectProductNameTxt.text = product.productName;
        selectProductDesTxt.text = product.productDes;
    }

    private void EndEvent()
    {
        isSelectPanelEnable = false;
        ShowPanel(false, ()=>
        {
            selectPanel.alpha = 0;
        });

        GameManager.Instance.EndEncounter();
    }

    public void ShowPanel(bool enable , Action onComplecteEvent = null)
    {
        mainPanel.DOFade(enable ? 1 : 0, 0.5f)
            .OnComplete(()=> {
                mainPanel.blocksRaycasts = enable;
                mainPanel.interactable = enable;
                onComplecteEvent?.Invoke();
        });
    }
}