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
                des = "불로 이루어진 조합입니다. 한 번에 적에게 큰 피해를 입힐 수 있습니다.";
                break;
            case 1:
                des = "물로 이루어진 조합입니다. 안정적으로 플레이 할수있습니다.";
                break;
            case 2:
                des = "자연으로 이루어진 조합입니다. 다양한 방식으로 공격할 수 있습니다.";
                break;
            case 3:
                des = "번개로 이루어진 조합입니다. 한 번에 역전할 수 있습니다.";
                break;
            default:
                break;
        }
        desText.DOText(des, 1f).OnComplete(() => canClick = true);

        FindObjectOfType<StoreIndex>().index = num;
    }
}
