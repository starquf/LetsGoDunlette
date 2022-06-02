using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPieceHandler : MonoBehaviour
{
    private InventoryHandler invenHandler;
    private InventoryInfoHandler invenInfo;
    private BattleHandler battleHandler;

    private void Awake()
    {
        GameManager.Instance.getPieceHandler = this;
    }

    void Start()
    {
        invenHandler = GameManager.Instance.inventoryHandler;
        battleHandler = GameManager.Instance.battleHandler;
        invenInfo = GameManager.Instance.invenInfoHandler;
    }

    public void GetPiecePlayer(SkillPiece skillPiece,Action onCanceled , Action onEnd = null)
    {
        if (CheckCapacity())
        {
            invenInfo.ShowInventoryInfo("교체할 조각을 선택하세요", ShowInfoRange.Inventory,
                selected =>
                {
                    invenInfo.desPanel.ShowDescription(selected);

                    invenInfo.desPanel.ShowConfirmBtn(() =>
                    {
                        invenInfo.desPanel.ShowPanel(false);

                        invenInfo.onCloseBtn = null;
                        invenInfo.CloseInventoryInfo();

                        invenHandler.RemovePiece(selected);

                        GetReward(skillPiece, onEnd);
                    });
                }, 
                onCanceled);
        }
        else
        {
            GetReward(skillPiece, onEnd);
        }
    }

    private void GetReward(SkillPiece selected, Action onEnd = null)
    {
        GetPiece(selected);

        onEnd?.Invoke();
    }

    private void GetPiece(SkillPiece skill, Action onEndCreate = null)
    {
        invenHandler.AddSkill(
            skill,
            battleHandler.player.GetComponent<Inventory>());
    }

    private bool CheckCapacity()
    {
        PlayerInventory inven = battleHandler.player.GetComponent<PlayerInventory>();

        return inven.IsInventoryFull();
    }
}
