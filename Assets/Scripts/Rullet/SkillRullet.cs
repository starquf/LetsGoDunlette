using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SkillRullet : Rullet
{
    protected override void Start()
    {
        base.Start();
    }

    public override void AddPiece(RulletPiece piece)
    {
        base.AddPiece(piece);

        (piece as SkillPiece).isInRullet = true;
    }

    // 해당 인덱스의 조각을 인벤토리에 넣고 바꾸는 함수
    public void ChangePiece(int changeIdx, SkillPiece changePiece)
    {
        if (changePiece == null)
        {
            Debug.LogError("null 있음 !!");
        }

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        if (pieces[changeIdx] != null)
        {
            inventory.SetSkillToGraveyard((SkillPiece)pieces[changeIdx]);
        }

        SetPiece(changeIdx, changePiece);
    }

    // 해당 인덱스의 조각을 바꾸는 함수
    public void SetPiece(int changeIdx, SkillPiece changePiece)
    {
        if (changePiece == null)
        {
            Debug.LogError("null 발생!!");
        }

        changePiece.isInRullet = true;

        changePiece.OnRullet();
        changePiece.pieceIdx = changeIdx;
        changePiece.transform.SetParent(transform);
        changePiece.transform.DOLocalMove(Vector3.zero, 0.35f);
        changePiece.transform.DOScale(Vector3.one, 0.35f);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();
    }

    // 해당 인덱스의 조각을 인벤토리로 넣는 함수
    public void PutRulletPieceToGraveYard(int changeIdx) //현제 룰렛에 index 에 있는 조각을 무덤에 넣는다
    {
        if (pieces[changeIdx] == null) return;

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetSkillToGraveyard((SkillPiece)pieces[changeIdx]);

        pieces[changeIdx] = null;

        SetRulletSmooth();
    }

    public void PutRulletPieceToInventory(int changeIdx) //현제 룰렛에 index 에 있는 조각을 인벤에 넣는다
    {
        if (pieces[changeIdx] == null) return;

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetSkillToInventory((SkillPiece)pieces[changeIdx]);

        pieces[changeIdx] = null;

        SetRulletSmooth();
    }

    public void PutAllRulletPieceToInventory()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            PutRulletPieceToInventory(i);
        }
    }


    // 해당 인덱스의 조각을 감지하지 못하게 함수
    public void SetEmpty(int changeIdx)
    {
        pieces[changeIdx] = null;
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
