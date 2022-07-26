using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyIconInfoHandler : IconInfoHandler
{
    [Header("UI ������")]
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
                    des = $"{elemental} ������� �ش�.";

                    break;

                case DesIconType.Stun:
                    name = "����";
                    des = "Ȯ���� ���� ����� ������ ���� �������� �̵���Ų��.";
                    break;

                case DesIconType.Silence:
                    name = "ħ��";
                    des = "�� �� ���� ����� ����� ī�尡 ��ȿȭ�ȴ�.";
                    break;

                case DesIconType.Exhausted:
                    name = "�Ƿ�";
                    des = "�� �� ���� ����� ���ݷ��� 25% ���ҽ�Ų��.";
                    break;

                case DesIconType.Wound:
                    name = "��ó";
                    des = "�� ���� �� ��ó ����ŭ�� ������� �����鼭 ��ó�� 1 ���ҵȴ�.";
                    break;

                case DesIconType.Heal:
                    name = "��";
                    des = "���� ��ŭ ü���� ȸ���Ѵ�.";
                    break;

                case DesIconType.Upgrade:
                    name = "��";
                    des = "���� ��ŭ ���ݷ��� ������Ų��.";
                    break;

                case DesIconType.Shield:
                    name = "��ȣ��";
                    des = "��ȣ���� �ִ� ���¿��� ������� ���� ��� ��ȣ���� ���� �Ҹ�ȴ�.";
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
                name = "���Ӽ�";
                break;

            case ElementalType.Nature:
                name = "�ڿ�";
                break;

            case ElementalType.Electric:
                name = "����";
                break;

            case ElementalType.Fire:
                name = "��";
                break;

            case ElementalType.Water:
                name = "��";
                break;

            case ElementalType.Monster:
                name = "��";
                break;
        }

        return name;
    }
}
