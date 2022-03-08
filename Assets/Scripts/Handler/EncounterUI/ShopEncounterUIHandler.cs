using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public enum ProductType
{
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
    public List<RulletPiece> pieceShopList;
    public List<Scroll> scrollShopList;

    private List<RulletPiece> randomRulletPiece;
    private List<Scroll> randomScrollPiece;

    private void Awake()
    {
        mainPanel = GetComponent<CanvasGroup>();
    }

    public void Start()
    {
        exitBtn.onClick.AddListener(OnExitBtnClick);
        purchaseBtn.onClick.AddListener(OnPurchaseBtnClick);
    }

    public void StartEvent()
    {
        isSelectPanelEnable = false;
        selectIdx = -1;

        randomRulletPiece = new List<RulletPiece>(pieceShopList);
        randomScrollPiece = new List<Scroll>(scrollShopList);

        InitShop();
        ShowPanel(true);
    }

    private void InitShop()
    {
        for (int i = 0; i < products.Count; i++)
        {
            if(i<3)
            {
                RulletPiece rulletPiece = randomRulletPiece[Random.Range(0, randomRulletPiece.Count)];
                randomRulletPiece.Remove(rulletPiece);
                products[i].SetProduct(ProductType.RulletPiece, null, rulletPiece);
            }
            else
            {
                Scroll scroll = randomScrollPiece[Random.Range(0, randomScrollPiece.Count)];
                randomScrollPiece.Remove(scroll);
                products[i].SetProduct(ProductType.Scroll, scroll);
            }
            int idx = i;
            products[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectProduct(idx);
            });
        }
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

    }

    private void SelectProduct(int selectIdx)
    {
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
