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

        // ��Ÿ�� üũ
        if (base.CanUseSkill())
        {
            // �ϳ��� �ִٸ�
            if (playerInven.skills.Count > 0)
            {
                return true;
            }
            else 
            {
                ui.ShowMessege("�κ��丮�� ����ֽ��ϴ�!");
            }
        }

        return false;
    }

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        bh.mainRullet.PauseRullet();

        infoHandler.ShowHighlight("����� ������ �����ϼ���");

        infoHandler.ShowInventoryInfo("�κ��丮���� ������ �����ϼ���", ShowInfoRange.Inventory, sp =>
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
