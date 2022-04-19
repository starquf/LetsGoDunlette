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

        confirmButton.onClick.AddListener(() => LoadingSceneHandler.LoadScene("SeungHwanScene"));
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
                des = "기본 조합입니다. 플레이하면서 원하는 조합을 맞춰보세요!";
                break;

            case 1:
                des = "불 속성으로 이루어진 조합입니다. 적에게 한 번에 큰 피해를 입힐 수 있습니다.";
                break;

            case 2:
                des = "물 속성으로 이루어진 조합입니다. 안정적으로 플레이 할수있습니다.";
                break;

            case 3:
                des = "번개 속성으로 이루어진 조합입니다. 스킬간의 연계가 자주 발생합니다.";
                break;

            default:
                break;
        }
        desText.DOText(des, 1f).OnComplete(() => canClick = true);

        GameManager.Instance.deckIdx = num;
    }
}
