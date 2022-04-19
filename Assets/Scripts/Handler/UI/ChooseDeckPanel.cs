using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ChooseDeckPanel : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();
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

        confirmButton.gameObject.SetActive(false);
        confirmButton.onClick.AddListener(() => {
            confirmButton.interactable = false;
            LoadingSceneHandler.LoadScene("SeungHwanScene");
        });
    }

    private void OnClickButton(int num)
    {
        if(!canClick) return;

        canClick = false;

        confirmButton.gameObject.SetActive(true);

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
                des = "������ �귿 ������� �̷���� �⺻ �����Դϴ�. �÷����ϸ鼭 ���ϴ� ������ ���纸����!";
                break;

            case 1:
                des = "�� �Ӽ� �귿 ������ ���ַ� �̷���� ���Դϴ�. �� �����Ѵٸ� ������ �ѹ��� ū ���ظ� ���� �� �ֽ��ϴ�.";
                break;

            case 2:
                des = "�� �Ӽ��� �ڿ� �Ӽ� �귿 ������� �̷���� ���Դϴ�. ���� ��ȣ������ �������� �÷��̰� �����մϴ�.";
                break;

            case 3:
                des = "���� �Ӽ����� �̷���� ���Դϴ�. ������ ������ �ο��ϴ� �Ͱ� ���� �� ��ȯ�� �̿��� ������ �����ϰ� �̲��� �� �� �ֽ��ϴ�.";
                break;

            default:
                break;
        }
        desText.DOText(des, 1f).OnComplete(() => canClick = true);

        GameManager.Instance.deckIdx = num;
    }
}
