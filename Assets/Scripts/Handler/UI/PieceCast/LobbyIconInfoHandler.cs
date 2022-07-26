using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyIconInfoHandler : IconInfoHandler
{
    [Header("UI 프리팹")]
    public GameObject iconInfoPrefab;

    private void Awake()
    {
        PoolManager.CreatePool<IconInfo>(iconInfoPrefab, transform, 3);
    }

    protected override void Start()
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
    }

    public override void InitInfo(SkillPiece sp, List<DesIconType> icons)
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

            Sprite icon = GetDesIcon(sp, icons[i]);
            string name = "";
            string des = "";

            switch (icons[i])
            {
                case DesIconType.Attack:

                    string elemental = GetElementalName(sp.currentType);

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

    private Sprite GetDesIcon(SkillPiece skillPiece, DesIconType type)
    {
        Sprite icon = null;

        icon = type switch
        {
            DesIconType.Attack => GameManager.Instance.inventoryHandler.effectSprDic[skillPiece.currentType],
            DesIconType.Stun => GameManager.Instance.ccIcons[0],
            DesIconType.Silence => GameManager.Instance.ccIcons[1],
            DesIconType.Exhausted => GameManager.Instance.ccIcons[2],
            DesIconType.Wound => GameManager.Instance.ccIcons[3],
            DesIconType.Invincibility => GameManager.Instance.ccIcons[4],
            DesIconType.Fascinate => GameManager.Instance.ccIcons[5],
            DesIconType.Heating => GameManager.Instance.ccIcons[6],
            DesIconType.Shield => GameManager.Instance.buffIcons[0],
            DesIconType.Heal => GameManager.Instance.buffIcons[1],
            DesIconType.Upgrade => GameManager.Instance.buffIcons[2],
            _ => null,
        };
        return icon;
    }

    private string GetElementalName(ElementalType elemental)
    {
        string name = "";

        switch (elemental)
        {
            case ElementalType.None:
                name = "무속성";
                break;

            case ElementalType.Nature:
                name = "자연";
                break;

            case ElementalType.Electric:
                name = "전기";
                break;

            case ElementalType.Fire:
                name = "불";
                break;

            case ElementalType.Water:
                name = "물";
                break;

            case ElementalType.Monster:
                name = "적";
                break;
        }

        return name;
    }
}
