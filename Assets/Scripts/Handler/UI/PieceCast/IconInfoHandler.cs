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
