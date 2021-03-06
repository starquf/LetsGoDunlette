using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Encounter_007 : RandomEncounter
{
    private SkillPiece skill = null;
    private InventoryInfoHandler invenInfoHandler = null;
    private InventoryHandler invenHandler = null;
    private Transform frontPanelTrm = null;
    private RandomEncounterUIHandler randomEncounterUIHandler;

    public override void Init()
    {
        base.Init();
        invenInfoHandler = GameManager.Instance.invenInfoHandler;
        invenHandler = GameManager.Instance.inventoryHandler;
        frontPanelTrm = invenInfoHandler.transform.parent;
        randomEncounterUIHandler = encounterInfoHandler.GetComponent<RandomEncounterUIHandler>();
    }
    public override void ResultSet(int resultIdx)
    {
        choiceIdx = resultIdx;
        switch (resultIdx)
        {
            case 0:
                showText = en_End_TextList[0];
                showImg = en_End_Image[0];
                en_End_Result = "무작위 룰렛으로 교환";

                invenInfoHandler.closeBtn.interactable = false;
                randomEncounterUIHandler.exitBtn.gameObject.SetActive(false);


                //invenInfoHandler.transform.SetParent(encounterInfoHandler.transform);
                //invenInfoHandler.desPanel.transform.SetParent(encounterInfoHandler.transform);

                invenInfoHandler.ShowInventoryInfo("교체할 조각을 선택하세요", ShowInfoRange.Inventory, sp =>
                {
                    invenInfoHandler.desPanel.ShowDescription(sp);

                    invenInfoHandler.desPanel.ShowConfirmBtn(() =>
                    {
                        invenInfoHandler.desPanel.ShowPanel(false);

                        invenInfoHandler.CloseInventoryInfo();

                        GameManager.Instance.bottomUIHandler.ShowBottomPanel(false);

                        invenHandler.GetSkillFromInventoryOrGraveyard(sp);

                        sp.transform.SetParent(encounterInfoHandler.transform);

                        DOTween.Sequence()
                        .Append(sp.transform.DOMove(randomEncounterUIHandler.encounterImg.transform.position, 0.5f))
                        .Join(sp.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                        .Join(sp.GetComponent<Image>().DOFade(0f, 0.5f))
                        .OnComplete(() =>
                        {
                            invenHandler.RemovePiece(sp);
                            Destroy(sp);

                            SkillPiece rulletPieces = encounterInfoHandler.GetRandomSkillRewards(1)[0].GetComponent<SkillPiece>();

                            MakeSkill(rulletPieces, out skill);
                            randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);


                            ShowEndEncounter?.Invoke();
                        });
                    });
                }, () =>
                {

                }, stopTime: false, closePanel: false);

                break;
            case 1:
                showText = en_End_TextList[1];
                showImg = en_End_Image[1];
                en_End_Result = "지나간다.";
                ShowEndEncounter?.Invoke();
                break;
            default:
                break;
        }
    }

    public override void Result()
    {
        switch (choiceIdx)
        {
            case 0:
                GetSkillInRandomEncounterAnim(skill, () =>
                {
                    invenInfoHandler.transform.SetParent(frontPanelTrm.transform);
                    invenInfoHandler.desPanel.transform.SetParent(frontPanelTrm.transform);
                    invenInfoHandler.closeBtn.interactable = true;
                    randomEncounterUIHandler.exitBtn.gameObject.SetActive(true);

                    OnExitEncounter?.Invoke(true);
                });
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
