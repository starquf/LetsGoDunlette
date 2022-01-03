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
        GetComponentsInChildren(pieces);

        SetRullet();

        int sizeSum = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            sizeSum += pieces[i].Size;
        }

        int addNomalCnt = (maxSize - sizeSum) / 6;

        //AddNormalAttackPiece(addNomalCnt);
    }

    // 해당 인덱스의 조각을 인벤토리에 넣고 바꾸는 함수
    public void ChangePiece(int changeIdx, RulletPiece changePiece)
    {
        //pieces[changeIdx].state = PieceState.USED;
        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetUseSkill((SkillPiece)pieces[changeIdx]);

        changePiece.transform.SetParent(transform);
        changePiece.transform.DOLocalMove(Vector3.zero, 0.35f);
        changePiece.transform.DOScale(Vector3.one, 0.35f);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();
    }

    // 해당 인덱스의 조각을 바꾸는 함수
    public void SetPiece(int changeIdx, RulletPiece changePiece)
    {
        //pieces[changeIdx].state = PieceState.USED;
        changePiece.transform.SetParent(transform);
        changePiece.transform.DOLocalMove(Vector3.zero, 0.35f);
        changePiece.transform.DOScale(Vector3.one, 0.35f);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();
    }

    // 해당 인덱스의 조각을 인벤토리로 넣는 함수
    public void PutRulletPieceToGraveYard(int changeIdx) //현제 룰렛에 index 에 있는 조각을 무덤에 넣는다
    {
        //pieces[changeIdx].state = PieceState.USED;
        if (pieces[changeIdx] == null) return;

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetUseSkill((SkillPiece)pieces[changeIdx]);

        pieces[changeIdx] = null;

        SetRulletSmooth();
    }

    public void PutRulletPieceToInventory(int changeIdx) //현제 룰렛에 index 에 있는 조각을 인벤에 넣는다
    {
        //pieces[changeIdx].state = PieceState.USED;
        if (pieces[changeIdx] == null) return;

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetUnUseSkill((SkillPiece)pieces[changeIdx]);

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
    public void SetExeptPiece(int changeIdx)
    {
        pieces[changeIdx] = null;
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
