using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Memorie : Scroll
{
    private BattleHandler bh;

    private void Start()
    {
        bh = GameManager.Instance.battleHandler;

        scrollType = ScrollType.Memorie;
    }

    public override void Use(Action onEndUse)
    {
        GameManager.Instance.invenInfoHandler.ShowInventoryInfo("���� ����", ShowInfoRange.Graveyard, sp => print(sp.PieceName));
    }
}
