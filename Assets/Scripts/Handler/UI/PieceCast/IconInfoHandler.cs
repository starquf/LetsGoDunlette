using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconInfoHandler : MonoBehaviour
{
    private CanvasGroup infoPanel;

    public Button infoBtn;
    public Transform infoBGTrans;

    private bool isOpened = false;

    private BattleHandler bh;

    private void Start()
    {
        infoPanel = GetComponentInChildren<CanvasGroup>();
        ShowPanel(infoPanel, isOpened);

        infoBtn.onClick.AddListener(() =>
        {
            isOpened = !isOpened;
            ShowPanel(infoPanel, isOpened);
        });

        bh = GameManager.Instance.battleHandler;
    }

    public void InitInfo(SkillPiece sp, List<DesIconType> icons)
    {
        for (int i = 0; i < infoBGTrans.childCount; i++)
        {
            infoBGTrans.GetChild(i).gameObject.SetActive(false);
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
            iconInfo.transform.localScale = Vector3.one;
        }
    }


    private void ShowPanel(CanvasGroup cvsGroup, bool enable)
    {
        cvsGroup.alpha = enable ? 1 : 0;
        cvsGroup.interactable = enable;
        cvsGroup.blocksRaycasts = enable;
    }

    public void ClosePanel()
    {
        ShowPanel(infoPanel, false);
        isOpened = false;
    }
}
