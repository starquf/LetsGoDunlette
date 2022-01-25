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
    }

    // 해당 인덱스의 조각을 인벤토리에 넣고 바꾸는 함수
    public void ChangePiece(int changeIdx, SkillPiece changePiece)
    {
        changePiece.isInRullet = true;

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetUseSkill((SkillPiece)pieces[changeIdx]);

        changePiece.transform.SetParent(transform);
        changePiece.transform.DOLocalMove(Vector3.zero, 0.35f);
        changePiece.transform.DOScale(Vector3.one, 0.35f);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();

        CreateChain();
    }

    // 해당 인덱스의 조각을 바꾸는 함수
    public void SetPiece(int changeIdx, SkillPiece changePiece)
    {
        changePiece.isInRullet = true;

        changePiece.transform.SetParent(transform);
        changePiece.transform.DOLocalMove(Vector3.zero, 0.35f);
        changePiece.transform.DOScale(Vector3.one, 0.35f);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();

        CreateChain();
    }

    // 체인 거는 함수
    private void CreateChain()
    {
        SkillPiece currentPiece = null;
        SkillPiece prevPiece = null;

        for (int i = 0; i < pieces.Count; i++)
        {
            currentPiece = pieces[i] as SkillPiece;

            if (i == 0)
                prevPiece = pieces[pieces.Count - 1] as SkillPiece;

            if (prevPiece != null && currentPiece != null)
            {
                // 만약 현재 조각이 전 조각의 속성과 같다면
                if (currentPiece.patternType == prevPiece.patternType)
                {
                    print($"체인된 속성 : {currentPiece.patternType}");
                    currentPiece.isChained = true;
                    prevPiece.isChained = true;
                }
            }

            prevPiece = currentPiece;
        }

        CheckChain();
    }

    // 체인 체크 함수
    private void CheckChain()
    {
        SkillPiece currentPiece = null;
        SkillPiece prevPiece = null;

        for (int i = 0; i < pieces.Count; i++)
        {
            currentPiece = pieces[i] as SkillPiece;

            if (i == 0)
                prevPiece = pieces[pieces.Count - 1] as SkillPiece;

            if (prevPiece != null)
            {
                // 만약 현재 조각이 전 조각의 속성과 같다면
                if (currentPiece.PieceType == prevPiece.PieceType)
                {
                    currentPiece.isChained = true;
                    prevPiece.isChained = true;
                }
            }

            prevPiece = currentPiece;
        }
    }

    // 해당 인덱스의 조각을 인벤토리로 넣는 함수
    public void PutRulletPieceToGraveYard(int changeIdx) //현제 룰렛에 index 에 있는 조각을 무덤에 넣는다
    {
        if (pieces[changeIdx] == null) return;

        InventoryHandler inventory = GameManager.Instance.inventoryHandler;

        inventory.SetUseSkill((SkillPiece)pieces[changeIdx]);

        pieces[changeIdx] = null;

        SetRulletSmooth();
    }

    public void PutRulletPieceToInventory(int changeIdx) //현제 룰렛에 index 에 있는 조각을 인벤에 넣는다
    {
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
    public void SetEmpty(int changeIdx)
    {
        pieces[changeIdx] = null;
    }

    protected override void CastDefault()
    {
        result = null;
    }
}
