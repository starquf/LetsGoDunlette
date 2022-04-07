using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_017 : RandomEncounter
{
    private SkillPiece skill;
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        PlayerHealth playerHealth = GameManager.Instance.GetPlayer();
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "전기를 통하게 하자 문이 열렸다. 문 안에는 돈과 여러가지 진귀한 물품들이 있었다.";
                RandomEncounterUIHandler randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
                InventoryInfoHandler invenInfoHandler = GameManager.Instance.invenInfoHandler;
                InventoryHandler invenHandler = GameManager.Instance.inventoryHandler;

                invenInfoHandler.closeBtn.interactable = false;
                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);

                GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);
                Transform frontPanelTrm = invenInfoHandler.transform.parent;

                //invenInfoHandler.transform.SetParent(encounterInfoHandler.transform);
                //invenInfoHandler.desPanel.transform.SetParent(encounterInfoHandler.transform);

                invenInfoHandler.ShowInventoryInfo("사용될 전기 조각을 선택하세요", ShowInfoRange.Inventory, sp =>
                {
                    invenInfoHandler.desPanel.ShowDescription(sp);

                    invenInfoHandler.desPanel.ShowConfirmBtn(() =>
                    {
                        invenInfoHandler.desPanel.ShowPanel(false);

                        if(sp.currentType == PatternType.Diamonds)
                        {
                            invenInfoHandler.onCloseBtn = null;
                            invenInfoHandler.CloseInventoryInfo();

                            invenHandler.GetSkillFromInventory(sp);

                            GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);

                            sp.transform.SetParent(encounterInfoHandler.transform);

                            DOTween.Sequence()
                            .Append(sp.transform.DOMove(randomEncounterUIHandler.encounterImg.transform.position, 0.5f))
                            .Join(sp.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                            .Join(sp.GetComponent<Image>().DOFade(0f, 0.5f))
                            .OnComplete(() =>
                            {
                                Destroy(sp);
                                invenInfoHandler.closeBtn.interactable = true;
                                randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);


                                GameManager.Instance.Gold += 10;
                                Debug.LogWarning("일단 유물 없음");
                            });
                        }
                        else
                        {
                            Debug.Log("tlqkf");
                            Anim_TextUp textAnim = PoolManager.GetItem<Anim_TextUp>();
                            textAnim.SetScale(1f);
                            textAnim.Play("전기속성 조각을 선택해 주세요!!");
                        }
                    });
                }/*, onCancelUse*/);
                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "무시했다.";
                break;
            default:
                break;
        }
    }

    public override void Result()
    {
        BattleHandler battleHandler = GameManager.Instance.battleHandler;
        Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
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
