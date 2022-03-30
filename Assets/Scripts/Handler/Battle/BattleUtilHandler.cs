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
                //print("빈칸 발생");
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

    public IEnumerator DrawRulletPieces()
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();
        List<int> changeIdxList = new List<int>();

        for (int i = 0; i < pieces.Count; i++)
        {
            // 비어있는곳이라면
            if (pieces[i] == null)
            {
                changeIdxList.Add(i);
            }
        }

        for (int i = 0; i < changeIdxList.Count; i++)
        {
            SkillPiece skill = inventory.GetRandomUnusedSkill();
            mainRullet.SetPiece(changeIdxList[i], skill);

            if (i != changeIdxList.Count - 1)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.35f);

        yield break;
    }

    public IEnumerator ResetRulletPiecesWithCondition(Action<Vector3> onResetPiecePosition = null, bool hasWait = false)
    {
        // 인벤토리에서 랜덤한 6개의 스킬을 뽑아 룰렛에 적용한다. 단, 최소한 적의 스킬 1개와 내 스킬 2개가 보장된다.
        // true : 플레이어    false : 적

        List<bool> condition = new List<bool>() { true, true, false };
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < condition.Count; i++)
        {
            if (pieces.Count > i)
            {
                onResetPiecePosition?.Invoke(pieces[i].skillImg.transform.position);
            }

            yield return null;

            SkillPiece skill = GetRandomPlayerOrEnemySkill(condition[i]);

            if (pieces.Count <= i)
            {
                mainRullet.AddPiece(skill);
            }
            else
            {
                ChangeRulletPiece(i, skill);
            }

            if(hasWait)
                yield return new WaitForSeconds(0.15f);
        }

        for (int i = condition.Count; i < 6; i++)
        {
            if (pieces.Count > i)
            {
                onResetPiecePosition?.Invoke(pieces[i].skillImg.transform.position);
            }

            yield return null;

            SkillPiece skill = GetRandomSkill();

            if (pieces.Count <= i)
            {
                mainRullet.AddPiece(skill);
            }
            else
            {
                ChangeRulletPiece(i, skill);
            }

            if (i != 5)
            {
                if (hasWait)
                    yield return new WaitForSeconds(0.15f);
            }
        }

        yield return new WaitForSeconds(0.35f);

        yield break;
    }

    public SkillPiece GetRandomPlayerOrEnemySkill(bool isPlayer) //true 면 플레이어
    {
        SkillPiece skill = inventory.GetRandomPlayerOrEnemySkill(isPlayer);

        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        return skill;
    }

    public SkillPiece GetRandomSkill()
    {
        SkillPiece skill = inventory.GetRandomUnusedSkill();
        skill.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        return skill;
    }

    public IEnumerator ResetRullet(Action onEndReset = null)
    {
        List<RulletPiece> pieces = mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            ChangeRulletPiece(i);

            if (i != 5)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.35f);

        onEndReset?.Invoke();

        yield break;
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

    public List<T> DeepCopyList<T>(List<T> targetList)
    {
        List<T> list = new List<T>();

        for (int i = 0; i < targetList.Count; i++)
        {
            list.Add(targetList[i]);
        }

        return list;
    }
}
