using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Bookmark : PlayerSkill_Cooldown
{
    private InventoryInfoHandler infoHandler;
    private InventoryHandler ih;

    private Inventory playerInven;

    protected override void Start()
    {
        base.Start();

        infoHandler = GameManager.Instance.invenInfoHandler;
        ih = GameManager.Instance.inventoryHandler;

        playerInven = bh.player.GetComponent<Inventory>();
    }

    public override bool CanUseSkill()
    {
        if (playerInven == null)
        {
            playerInven = bh.player.GetComponent<Inventory>();
        }

        // 쿨타임 체크
        if (base.CanUseSkill())
        {
            // 하나라도 있다면
            if (playerInven.skills.Count > 0)
            {
                return true;
            }
            else 
            {
                ui.ShowMessege("인벤토리가 비어있습니다!");
            }
        }

        return false;
    }

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        bh.mainRullet.PauseRullet();

        infoHandler.ShowHighlight("사용할 조각을 선택하세요");

        infoHandler.ShowInventoryInfo("인벤토리에서 조각을 선택하세요", ShowInfoRange.Inventory, sp =>
        {
            infoHandler.desPanel.ShowDescription(sp);

            infoHandler.desPanel.ShowConfirmBtn(() =>
            {
                infoHandler.desPanel.ShowPanel(false);

                infoHandler.onCloseBtn = null;
                infoHandler.CloseInventoryInfo();

                bh.playerSkillHandler.isCasting = false;

                ih.GetSkillFromInventoryOrGraveyard(sp);

                print(sp.IsInRullet);

                OnEndSkill();

                bh.CastPiece(sp);
            });
        }, onCancelSkill, true);
    }
}
