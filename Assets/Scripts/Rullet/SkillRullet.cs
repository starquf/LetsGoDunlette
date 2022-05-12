using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class SkillRullet : Rullet
{
    protected override void Start()
    {
        base.Start();
    }

    public override void AddPiece(RulletPiece piece)
    {
        SkillPiece sp = piece as SkillPiece;

        sp.IsInRullet = true;
        sp.OnRullet();

        ShowPieceEffect(sp);

        if (pieces.Count >= 6)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == null) // 빈칸이라면
                {
                    piece.pieceIdx = i;
                    pieces[i] = piece;

                    break;
                }
            }
        }
        else
        {
            piece.pieceIdx = pieces.Count;
            pieces.Add(piece);
        }

        SetRulletSmooth();
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

        changePiece.IsInRullet = true;

        changePiece.OnRullet();
        changePiece.pieceIdx = changeIdx;

        ShowPieceEffect(changePiece);

        pieces[changeIdx] = changePiece;

        SetRulletSmooth();
    }

    private void ShowPieceEffect(SkillPiece sp)
    {
        sp.transform.SetParent(transform);

        sp.gameObject.SetActive(false);

        InventoryHandler inven = GameManager.Instance.inventoryHandler;
        Sprite effectSpr = inven.effectSprDic[sp.currentType];
        Gradient effectGrad = inven.effectGradDic[sp.currentType];

        for (int i = 0; i < 2; i++)
        {
            int a = i;

            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSpr);
            effect.SetColorGradient(effectGrad);

            effect.transform.position = sp.owner.indicator.transform.position;

            effect.Play(transform.position, () =>
            {
                if (a == 1)
                {
                    inven.CreateSkillEffect(sp, transform.position);

                    sp.transform.localPosition = Vector3.zero;
                    sp.transform.localScale = Vector3.one;

                    sp.gameObject.SetActive(true);
                    sp.HighlightColor(0.3f);

                    GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.4f, 0.1f);
                }

                effect.EndEffect();
            }
            , BezierType.Quadratic, playSpeed: 2f);
        }
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
