using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private bool isEquip = false;
    public bool IsEquip => isEquip;
    public Button OpenBtn;
    public CanvasGroup InventoryCvsGroup;
    public GameObject skillPiecePrefab;
    private bool isInventoryOpened = false;

    [HideInInspector]
    public RulletPiece result;
    [HideInInspector]
    public int resultIdx;

    private void Start()
    {
        GameManager.Instance.inventoryHandler = this;
        OpenBtn.onClick.AddListener(() =>
        {
            OpenInventory();
        });
    }
    public IEnumerator CheckEquipRullet(SkillRullet skillRullet, RulletInventorySlot equipSkillPieceSlot, Action<int> deleteItem)
    {
        EquipRullet();
        skillRullet.StopRulletToChangePiece();
        yield return new WaitForSeconds(0.1f);
        // 전부 돌릴 때까지
        yield return new WaitUntil(()=> !skillRullet.IsRoll);
        yield return new WaitForSeconds(0.5f);

        Transform rulletTrans = skillRullet.transform;

        GameObject changeSkillPiece = Instantiate(skillPiecePrefab, equipSkillPieceSlot.transform.position, Quaternion.identity, rulletTrans);

        changeSkillPiece.transform.localPosition = new Vector3(changeSkillPiece.transform.localPosition.x, changeSkillPiece.transform.localPosition.y, 0f);
        changeSkillPiece.transform.localScale = new Vector3(1f, 1f, 1f);
        changeSkillPiece.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        changeSkillPiece.GetComponent<Image>().sprite = equipSkillPieceSlot.sprite;

        SkillPiece equipSkillPiece = equipSkillPieceSlot.rulletPiece;
        SkillPiece skillPieceItem = changeSkillPiece.GetComponent<SkillPiece>();
        skillPieceItem.ChangeSize(equipSkillPiece.Size);
        skillPieceItem.ChangePieceName(equipSkillPiece.PieceName);
        skillPieceItem.ChangeValue(equipSkillPiece.Value);
        skillPieceItem.skillImg.sprite = equipSkillPieceSlot.iconSprite;

        deleteItem(equipSkillPieceSlot.slotIdx);

        Sequence itemSeq = DOTween.Sequence()
            .Append(changeSkillPiece.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                skillRullet.ChangePiece(resultIdx, changeSkillPiece.GetComponent<RulletPiece>());
            })
            .Append(changeSkillPiece.transform.DOLocalMove(Vector3.zero, 1f))
            .AppendCallback(() =>
            {
                EndEquipRullet();
                skillRullet.RollRullet();
            });


        //ResetRullets();

        //if (enemyHealth.IsDie)
        //{
        //    yield return new WaitUntil(() => !enemyHealth.enemyReward.IsReward);

        //    GoNextRoom();

        //    yield return new WaitUntil(() => !enemyHealth.IsDie);

        //    (turnRullet as TurnRullet).InitTurn();
        //}

        //yield return new WaitForSeconds(0.5f);
        //RollAllRullet();

        //yield return new WaitForSeconds(0.1f);

        //tapGroup.alpha = 1f;
        //tapGroup.interactable = true;
        //tapGroup.blocksRaycasts = true;

        //GameObject item = Instantiate(skillPiecePrefab, transform.position, Quaternion.identity, rulletTrans);

        //item.transform.localPosition = new Vector3(item.transform.localPosition.x, item.transform.localPosition.y, 0f);
        //item.transform.localScale = new Vector3(1f, 1f, 1f);
        //item.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        //item.GetComponent<Image>().sprite = slots[slotIdx].sprite;

        //SkillPiece skillPieceItem = item.GetComponent<SkillPiece>();
        //skillPieceItem.ChangeSize(slots[slotIdx].rulletPiece.Size);
        //skillPieceItem.ChangePieceName(slots[slotIdx].rulletPiece.PieceName);
        //skillPieceItem.ChangeValue(slots[slotIdx].rulletPiece.Value);
        //skillPieceItem.skillImg.sprite = slots[slotIdx].iconSprite;

        //GameManager.Instance.inventoryHandler.EquipRullet();

        //DeleteItem(slotIdx);

        //Sequence itemSeq = DOTween.Sequence()
        //    .Append(item.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
        //    .AppendCallback(() =>
        //    {
        //        rulletTrans.GetComponent<Rullet>().AddPiece(item.GetComponent<RulletPiece>());
        //    })
        //    .Append(item.transform.DOLocalMove(Vector3.zero, 1f))
        //    .AppendCallback(() =>
        //    {
        //        GameManager.Instance.inventoryHandler.EndEquipRullet();
        //    });
    }

    public void EquipRullet()
    {
        isEquip = true;
        InventoryCvsGroup.alpha = 0f;
    }
    public void EndEquipRullet()
    {
        isEquip = false;
        InventoryCvsGroup.alpha = 1f;
    }

    public void OpenInventory()
    {
        if (isEquip)
            return;
        isInventoryOpened = !isInventoryOpened;
        InventoryCvsGroup.alpha = isInventoryOpened ? 1f : 0f;
        InventoryCvsGroup.interactable = isInventoryOpened;
        InventoryCvsGroup.blocksRaycasts = isInventoryOpened;
    }
}
