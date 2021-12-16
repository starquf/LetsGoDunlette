using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    // �귿�� ������ ����� �ִ� ���ΰ�?
    private bool isEquip = false;
    public bool IsEquip => isEquip;

    // �κ��丮�� ���� ��ư
    public Button OpenBtn;

    // ��� �κ��丮 ���� �ִ� ĵ���� �׷�
    public CanvasGroup InventoryCvsGroup;
    // �ƹ��͵� ���� ��ų ����
    public GameObject skillPiecePrefab;

    // �κ��丮�� �����ִ°�?
    private bool isInventoryOpened = false;

    // ������ ���� ���
    [HideInInspector]
    public RulletPiece result;

    // ������ ���� ����� �ε���
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

    public void CheckEquitRulletStart(SkillRullet skillRullet, RulletInventorySlot equipSkillPieceSlot, Action<int> deleteItem)
    {
        StartCoroutine(CheckEquipRullet(skillRullet, equipSkillPieceSlot, deleteItem));
    }

    // �귿 �����ϴ� ���� ����            ����� �귿               ������ �귿������ ����                     ������ ���� �� �޴� �׼�
    public IEnumerator CheckEquipRullet(SkillRullet skillRullet, RulletInventorySlot equipSkillPieceSlot, Action<int> deleteItem)
    {
        // ���� ����
        EquipRullet();

        // �귿�� �����
        skillRullet.StopRullet((result, pieceIdx) =>
        {
            this.result = result;
            resultIdx = pieceIdx;
        });

        // ���� ���� ������ ��ٸ���
        yield return new WaitUntil(() => !skillRullet.IsRoll);
        yield return new WaitForSeconds(0.5f);

        // ������ �귿�� Ʈ������
        Transform rulletTrans = skillRullet.transform;

        // ������ �귿������ �ν��Ͻ�
        GameObject changeSkillPiece = Instantiate(skillPiecePrefab, equipSkillPieceSlot.transform.position, Quaternion.identity, rulletTrans);
        Image changeSkillPieceImg = changeSkillPiece.GetComponent<Image>();

        changeSkillPiece.transform.localPosition = new Vector3(changeSkillPiece.transform.localPosition.x, changeSkillPiece.transform.localPosition.y, 0f);
        changeSkillPiece.transform.localScale = new Vector3(1f, 1f, 1f);

        changeSkillPieceImg.color = new Color(1f, 1f, 1f, 0f);
        changeSkillPieceImg.sprite = equipSkillPieceSlot.sprite;

        // ������ ��ų ���� ��ũ��Ʈ
        SkillPiece equipSkillPiece = equipSkillPieceSlot.rulletPiece;
        // ������ ������ ��ų ���� ��ũ��Ʈ
        SkillPiece skillPieceItem = changeSkillPiece.GetComponent<SkillPiece>();

        // ������ ������ ������ ������ ��ų ������ ������ ����
        skillPieceItem.ChangeSize(equipSkillPiece.Size);
        skillPieceItem.ChangePieceName(equipSkillPiece.PieceName);
        skillPieceItem.ChangeValue(equipSkillPiece.Value);

        // ���⿡ Instantiate�� ���� ������ Cast�� ������ ������ Cast�� �ٲ�ߵȴ�
        // ������ Cast�� ���õ� Action�� ����� Action���ٰ� ������ ������ Action���� �ٲ��ְ�

        skillPieceItem.skillImg.sprite = equipSkillPieceSlot.iconSprite;

        // ������ ���� �� �޴� �׼� ȣ��
        deleteItem(equipSkillPieceSlot.slotIdx);

        // ����
        Sequence itemSeq = DOTween.Sequence()
            .Append(changeSkillPiece.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                skillRullet.ChangePiece(resultIdx, changeSkillPiece.GetComponent<RulletPiece>());
            })
            .Append(changeSkillPiece.transform.DOLocalMove(Vector3.zero, 1f))
            .AppendCallback(() =>
            {
                // ������ ���� �� ȣ��
                EndEquipRullet();
                // �ٽ� ������
                skillRullet.RollRullet();
            });
    }

    // �귿�� ������ ������ �� �Ҹ��� �Լ�
    public void EquipRullet()
    {
        isEquip = true;
        InventoryCvsGroup.alpha = 0f;
    }

    // �귿�� ���� ������ ���� �� �Ҹ��� �Լ�
    public void EndEquipRullet()
    {
        isEquip = false;
        InventoryCvsGroup.alpha = 1f;
    }

    // �κ��丮�� ���� �ݴ� �Լ�
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
