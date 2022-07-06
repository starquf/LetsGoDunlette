using UnityEngine;

public class Encounter_005 : RandomEncounter
{
    private bool IsWin()
    {
        int rand = Random.Range(0, 99);
        return rand < 40;
    }

    public override void ResultSet(int resultIdx)
    {
        if (IsWin())
        {
            choiceIdx = 0;
            showText = en_End_TextList[0];
            showImg = en_End_Image[0];
            en_End_Result = "�¸�\n3���� �ϳ� ������ �ּ���";

            /*
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

                    Debug.LogWarning("����â �����ߵ�");
                    // ����â ��� �ְ� �Ʒ��ִ� �Լ� ����
                    GetScroll(scrolls[idx]);
                });
            }*/
        }
        else
        {
            choiceIdx = 1;
            showText = en_End_TextList[1];
            showImg = en_End_Image[1];
            en_End_Result = "�й�";
        }
        ShowEndEncounter?.Invoke();
    }

    /*
    private void GetScroll(Scroll _scroll)
    {
        BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();

        Scroll scroll = PoolManager.GetScroll(_scroll.scrollType);
        MakeScroll(_scroll.scrollType, out scroll);

        battleScrollHandler.GetScroll(scroll, () =>
        {
            OnExitEncounter?.Invoke(true);
        }, true);
    }*/

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
