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
                des = "간단한 룰렛 조각들로 이루어진 기본 조합입니다. 플레이하면서 원하는 조합을 맞춰보세요!";
                break;

            case 1:
                des = "불 속성 룰렛 조각들 위주로 이루어진 덱입니다. 잘 조작한다면 적에게 한번에 큰 피해를 입힐 수 있습니다.";
                break;

            case 2:
                des = "물 속성과 자연 속성 룰렛 조각들로 이루어진 덱입니다. 힐과 보호막으로 안정적인 플레이가 가능합니다.";
                break;

            case 3:
                des = "번개 속성으로 이루어진 덱입니다. 적에게 기절을 부여하는 것과 빠른 덱 순환을 이용해 전투를 유리하게 이끌어 갈 수 있습니다.";
                break;

            default:
                break;
        }
        desText.DOText(des, 1f).OnComplete(() => canClick = true);

        GameManager.Instance.deckIdx = num;
    }
}
