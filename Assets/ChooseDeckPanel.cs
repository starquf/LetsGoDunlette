using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ChooseDeckPanel : MonoBehaviour
{
    public List<Button> buttons;
    public Button confirmButton;

    public Text desText;

    private bool canClick = false;

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int a = i;
            buttons[i].onClick.AddListener(() => OnClickButton(a));
        }

        canClick = true;

        confirmButton.onClick.AddListener(() => SceneManager.LoadScene(1));
    }

    private void OnClickButton(int num)
    {
        if(!canClick) return;

        canClick = false;

        for (int i = 0; i < buttons.Count; i++)
        {
            if(num == i)
            {
                buttons[i].GetComponent<Image>().DOFade(1f,1f);
            }
            else
            {
                buttons[i].GetComponent<Image>().DOFade(0f, 0f);
            }
        }

        string des = "";
        desText.text = des;
        switch (num)
        {
            case 0:
                des = "�ҷ� �̷���� �����Դϴ�. �� ���� ������ ū ���ظ� ���� �� �ֽ��ϴ�.";
                break;
            case 1:
                des = "���� �̷���� �����Դϴ�. ���������� �÷��� �Ҽ��ֽ��ϴ�.";
                break;
            case 2:
                des = "�ڿ����� �̷���� �����Դϴ�. �پ��� ������� ������ �� �ֽ��ϴ�.";
                break;
            case 3:
                des = "������ �̷���� �����Դϴ�. �� ���� ������ �� �ֽ��ϴ�.";
                break;
            default:
                break;
        }
        desText.DOText(des, 1f).OnComplete(() => canClick = true);

        FindObjectOfType<StoreIndex>().index = num;
    }
}
