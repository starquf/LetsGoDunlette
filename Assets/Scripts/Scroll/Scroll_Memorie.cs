using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Memorie : Scroll
{
    private BattleHandler bh;
    private InventoryHandler invenHandler;
    private InventoryInfoHandler invenInfoHandler;

    private readonly string msg_1 = "무덤에서 꺼낼 조각 선택";
    private readonly string msg_2 = "교체할 조각을 선택하세요";

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;
        invenInfoHandler = GameManager.Instance.invenInfoHandler;
        invenHandler = GameManager.Instance.inventoryHandler;

        scrollType = ScrollType.Memorie;
    }

    public override void Use(Action onEndUse, Action onCancelUse)
    {
        if (invenHandler.usedSkills.Count <= 0 || bh.mainRullet.IsStop)
        {
            onCancelUse?.Invoke();
            return;
        }

        bh.mainRullet.PauseRullet();

        invenInfoHandler.ShowInventoryInfo(msg_1, ShowInfoRange.Graveyard, sp =>
        {
            invenInfoHandler.desPanel.ShowDescription(sp);

            invenInfoHandler.desPanel.ShowConfirmBtn(() =>
            {
                invenInfoHandler.desPanel.ShowPanel(false);

                invenInfoHandler.onCloseBtn = null;
                invenInfoHandler.CloseInventoryInfo();

                invenHandler.GetSkillFromInventory(sp);

                bh.battleUtil.ChangeRulletPiece(UnityEngine.Random.Range(0, 6), sp);
                bh.battleUtil.SetTimer(0.5f, () => onEndUse?.Invoke());
            });
        }, onCancelUse);

        invenInfoHandler.ShowHighlight(msg_2);
    }
}
