using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_020 : RandomEncounter
{
    public int giveMoneyValue = 10;
    public int hitedDamage = 20;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
        switch (resultIdx)
        {
            case 0:
                BattleScrollHandler battleScrollHandler = bh.GetComponent<BattleScrollHandler>();
                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);

                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "회복 스크롤 1개 획득";
                GameManager.Instance.Gold -= giveMoneyValue;

                Scroll scroll = PoolManager.GetScroll(ScrollType.Heal);
                scroll.transform.position = Vector2.zero;
                Image scrollImg = scroll.GetComponent<Image>();
                scrollImg.color = new Color(1, 1, 1, 0);
                scroll.transform.SetParent(encounterInfoHandler.transform);
                scroll.GetComponent<RectTransform>().sizeDelta = Vector2.one * 400f;
                scroll.transform.localScale = Vector3.one;
                scrollImg.DOFade(1, 0.5f).SetDelay(1f);

                battleScrollHandler.GetScroll(scroll, () =>
                {
                    randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);
                }, true);
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = $"{hitedDamage}만큼 데미지를 입었다.";
                playerHealth.GetDamage(hitedDamage);
                break;
            case 2:
                showText = en_End_TextList[2];
                showImg = en_End_Image[2];
                en_End_Result = "아무일도 일어나지않았다";

                break;
            default:
                break;
        }
        ShowEndEncounter?.Invoke();
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
            case 2:
                OnExitEncounter?.Invoke(true);
                break;
            default:
                break;
        }
        choiceIdx = -1;
    }
}
