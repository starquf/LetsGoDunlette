using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_005 : RandomEncounter
{
    private bool IsWin()
    {
        int rand = Random.Range(0, 99);
        if(rand < 33)
        {
            return true;
        }
        return false;
    }

    public override void ResultSet(int resultIdx)
    {
        if(IsWin())
        {
            choiceIdx = 0;
            showText = en_End_TextList[0];
            showImg = en_End_Image[0];
            en_End_Result = "승리\n3개중 하나 선택해 주세요";

            RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
            Transform parent = randomEncounterUIHandler.imgButtonRowsCvs.transform;

            randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);
            randomEncounterUIHandler.ShowPanel(true, randomEncounterUIHandler.imgButtonRowsCvs, 1f);

            List<Scroll> scrolls = encounterInfoHandler.GetRandomScrollRewards(3);
            for (int i = 0; i < 3; i++)
            {
                int idx = i;
                GameObject item = parent.GetChild(idx).gameObject;
                Image image = item.GetComponent<Image>();
                image.sprite = scrolls[idx].GetComponent<Image>().sprite;
                
                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int idx2 = j;
                        parent.GetChild(idx2).GetComponent<Button>().onClick.RemoveAllListeners();
                    }
                    randomEncounterUIHandler.imgButtonRowsCvs.interactable = false;
                    randomEncounterUIHandler.ShowPanel(false, randomEncounterUIHandler.imgButtonRowsCvs);

                    Debug.LogWarning("설명창 띄워줘야됨");
                    // 설명창 띄워 주고 아래있는 함수 실행
                    GetScroll(scrolls[idx]);
                });
            }
        }
        else
        {
            choiceIdx = 1;
            showText = en_End_TextList[1];
            showImg = en_End_Image[1];
            en_End_Result = "패배";
        }
    }

    private void GetScroll(Scroll _scroll)
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        BattleScrollHandler battleScrollHandler = battleHandler.GetComponent<BattleScrollHandler>();

        GameObject scroll = Instantiate(_scroll, Vector3.zero, Quaternion.identity).gameObject;
        Image scrollImg = scroll.GetComponent<Image>();
        scrollImg.color = new Color(1, 1, 1, 0);
        scroll.transform.SetParent(encounterInfoHandler.transform);
        scroll.GetComponent<RectTransform>().sizeDelta = Vector2.one * 400f;
        scroll.transform.position = Vector2.zero;
        scroll.transform.localScale = Vector3.one;

        battleScrollHandler.GetScroll(_scroll, () =>
        {
            Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
            DOTween.Sequence().Append(scrollImg.DOFade(1, 0.5f)).SetDelay(1f)
            .Append(scroll.transform.DOMove(unusedInventoryTrm.position, 0.5f))
            .Join(scroll.transform.DOScale(Vector2.one * 0.1f, 0.5f))
            .Join(scroll.GetComponent<Image>().DOFade(0f, 0.5f))
            .OnComplete(() =>
            {
                Destroy(scroll);
                OnExitEncounter?.Invoke(true);
            });
        });
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:
                OnExitEncounter?.Invoke(true);
                break;
            case 1:
                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
