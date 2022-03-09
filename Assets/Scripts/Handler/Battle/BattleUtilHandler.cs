using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUtilHandler : MonoBehaviour
{
    private InventoryHandler inventory;
    private SkillRullet mainRullet;

    public void Init(InventoryHandler inven, SkillRullet rullet)
    {
        inventory = inven;
        mainRullet = rullet;
    }

    public bool CheckRulletPenalty(bool isPlayerPiece)
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] != null)
            {
                if ((pieces[i] as SkillPiece).isPlayerSkill != isPlayerPiece)
                {
                    return false;
                }
            }
            else
            {
                print("빈칸 발생");
                return false;
            }
        }

        return true;
    }

    public void ChangeRulletPiece(int pieceIdx)
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();

        mainRullet.GetComponent<SkillRullet>().ChangePiece(pieceIdx, skill);
    }

    public void ChangeRulletPiece(int pieceIdx, SkillPiece piece)
    {
        mainRullet.GetComponent<SkillRullet>().ChangePiece(pieceIdx, piece);
    }

    public void DrawRulletPieces()
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            // 비어있는곳이라면
            if (pieces[i] == null)
            {
                SkillPiece skill = inventory.GetRandomUnusedSkill();
                mainRullet.SetPiece(i, skill);
            }
        }
    }

    public void ResetRullet()
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            ChangeRulletPiece(i);
        }
    }

    public void SetPieceToGraveyard(int pieceIdx)
    {
        mainRullet.PutRulletPieceToGraveYard(pieceIdx);
    }

    public void SetPieceToGraveyard(SkillPiece piece)
    {
        if (piece == null) return;

        GameManager.Instance.inventoryHandler.SetUseSkill(piece);
    }

    public void SetPieceToInventory(SkillPiece piece)
    {
        GameManager.Instance.inventoryHandler.SetUnUseSkill(piece);
    }

    public void SetRulletEmpty(int pieceIdx)
    {
        mainRullet.SetEmpty(pieceIdx);
    }

    public void SetTimer(float time, Action onEndWait)
    {
        StartCoroutine(WaitTimer(time, onEndWait));
    }

    private IEnumerator WaitTimer(float time, Action onEndWait)
    {
        yield return new WaitForSeconds(time);

        onEndWait?.Invoke();
    }

    public bool CheckDie(LivingEntity player)
    {
        return player.IsDie;
    }

    public bool CheckEnemyDie(List<EnemyHealth> enemys)
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                return false;
            }
        }

        return true;
    }

    public List<EnemyHealth> CheckLivingEnemy(List<EnemyHealth> enemys)
    {
        List<EnemyHealth> livingEnemys = new List<EnemyHealth>();

        for (int i = 0; i < enemys.Count; i++)
        {
            if (!enemys[i].IsDie)
            {
                livingEnemys.Add(enemys[i]);
            }
        }

        return livingEnemys;
    }
}
