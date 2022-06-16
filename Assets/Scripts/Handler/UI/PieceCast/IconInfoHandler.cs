using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconInfoHandler : MonoBehaviour
{
    public CanvasGroup infoPanel;

    public CanvasGroup fadeBG;
    public Button infoBtn;
    public Button closePanel;
    public Transform infoBGTrans;

    private bool isOpened = false;

    private BattleHandler bh;

    private void Start()
    {
        ShowPanel(infoPanel, isOpened);
        ShowPanel(fadeBG, isOpened);

        infoBtn.onClick.AddListener(() =>
        {
            isOpened = !isOpened;
            ShowPanel(infoPanel, isOpened);
            ShowPanel(fadeBG, isOpened);
        });

        closePanel.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        bh = GameManager.Instance.battleHandler;
    }

    public void InitInfo(SkillPiece sp, List<DesIconType> icons)
    {
        ClosePanel();

        for (int i = 0; i < infoBGTrans.childCount; i++)
        {
            infoBGTrans.GetChild(i).gameObject.SetActive(false);
            infoBGTrans.GetChild(i).GetComponent<IconInfo>().ShowPanel(false);
        }

        for (int i = 0; i < icons.Count; i++)
        {
            IconInfo iconInfo = PoolManager.GetItem<IconInfo>();

            Sprite icon = bh.battleUtil.GetDesIcon(sp, icons[i]);
            string name = "";
            string des = "";

            switch (icons[i])
            {
                case DesIconType.Attack:

                    string elemental = bh.battleUtil.GetElementalName(sp.currentType);

                    name = $"{elemental}";
                    des = $"{elemental} 대미지를 준다.";

                    break;

                case DesIconType.Stun:
                    name = "기절";
                    des = "확률에 따라 대상의 조각을 전부 무덤으로 이동시킨다.";
                    break;

                case DesIconType.Silence:
                    name = "침묵";
                    des = "턴 수 동안 대상이 사용한 카드가 무효화된다.";
                    break;

                case DesIconType.Exhausted:
                    name = "피로";
                    des = "턴 수 동안 대상의 공격력을 25% 감소시킨다.";
                    break;

                case DesIconType.Wound:
                    name = "상처";
                    des = "턴 종료 시 상처 수만큼의 대미지를 받으면서 상처가 1 감소된다.";
                    break;

                case DesIconType.Heal:
                    name = "힐";
                    des = "숫자 만큼 체력을 회복한다.";
                    break;

                case DesIconType.Upgrade:
                    name = "힘";
                    des = "숫자 만큼 공격력을 증가시킨다.";
                    break;

                case DesIconType.Shield:
                    name = "보호막";
                    des = "보호막이 있는 상태에서 대미지를 받을 경우 보호막이 먼저 소모된다.";
                    break;
            }

            iconInfo.Init(icon, name, des);

            iconInfo.transform.SetParent(infoBGTrans);
            iconInfo.transform.SetAsLastSibling();

            iconInfo.transform.localScale = Vector3.one;
        }
    }


    private void ShowPanel(CanvasGroup cvsGroup, bool enable)
    {
        cvsGroup.alpha = enable ? 1 : 0;
        cvsGroup.interactable = enable;
        cvsGroup.blocksRaycasts = enable;

        if (enable)
        {
            for (int i = 0; i < infoBGTrans.childCount; i++)
            {
                infoBGTrans.GetChild(i).GetComponent<IconInfo>().ShowHighlight(i * 0.035f);
            }
        }
        else
        {
            for (int i = 0; i < infoBGTrans.childCount; i++)
            {
                infoBGTrans.GetChild(i).GetComponent<IconInfo>().ShowPanel(false);
            }
        }
    }

    public void ClosePanel()
    {
        ShowPanel(fadeBG, false);
        ShowPanel(infoPanel, false);

        isOpened = false;
    }
}
