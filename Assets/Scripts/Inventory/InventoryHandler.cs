using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    // 룰렛에 조각을 끼우고 있는 중인가?
    private bool isEquip = false;
    public bool IsEquip => isEquip;

    // 인벤토리를 여는 버튼
    public Button OpenBtn;

    // 모든 인벤토리 위에 있는 캔버스 그룹
    public CanvasGroup InventoryCvsGroup;
    // 아무것도 없는 스킬 조각
    public GameObject skillPiecePrefab;

    // 인벤토리가 열려있는가?
    private bool isInventoryOpened = false;

    // 돌려서 나온 결과
    [HideInInspector]
    public RulletPiece result;

    // 돌려서 나온 결과의 인덱스
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

    // 룰렛 착용하는 메인 로직            착용될 룰렛               착용할 룰렛조각의 슬롯                     슬롯을 없엘 때 받는 액션
    public IEnumerator CheckEquipRullet(SkillRullet skillRullet, RulletInventorySlot equipSkillPieceSlot, Action<int> deleteItem)
    {
        // 착용 시작
        EquipRullet();

        // 룰렛을 멈춘다
        skillRullet.StopRullet((result, pieceIdx) =>
        {
            this.result = result;
            resultIdx = pieceIdx;
        });

        // 전부 멈출 때까지 기다린다
        yield return new WaitUntil(() => !skillRullet.IsRoll);
        yield return new WaitForSeconds(0.5f);

        // 착용할 룰렛의 트렌스폼
        Transform rulletTrans = skillRullet.transform;

        // 변경할 룰렛조각의 인스턴스
        GameObject changeSkillPiece = Instantiate(skillPiecePrefab, equipSkillPieceSlot.transform.position, Quaternion.identity, rulletTrans);
        Image changeSkillPieceImg = changeSkillPiece.GetComponent<Image>();

        changeSkillPiece.transform.localPosition = new Vector3(changeSkillPiece.transform.localPosition.x, changeSkillPiece.transform.localPosition.y, 0f);
        changeSkillPiece.transform.localScale = new Vector3(1f, 1f, 1f);

        changeSkillPieceImg.color = new Color(1f, 1f, 1f, 0f);
        changeSkillPieceImg.sprite = equipSkillPieceSlot.sprite;

        // 슬롯의 스킬 조각 스크립트
        SkillPiece equipSkillPiece = equipSkillPieceSlot.rulletPiece;
        // 변경할 조각의 스킬 조각 스크립트
        SkillPiece skillPieceItem = changeSkillPiece.GetComponent<SkillPiece>();

        // 변경할 조각의 정보를 슬롯의 스킬 조각의 정보로 수정
        skillPieceItem.ChangeSize(equipSkillPiece.Size);
        skillPieceItem.ChangePieceName(equipSkillPiece.PieceName);
        skillPieceItem.ChangeValue(equipSkillPiece.Value);

        // 여기에 Instantiate로 만든 조각의 Cast를 선택한 조각의 Cast로 바꿔야된다
        // 조각에 Cast와 관련된 Action을 만들고 Action에다가 선택한 조각의 Action으로 바꿔주고

        skillPieceItem.skillImg.sprite = equipSkillPieceSlot.iconSprite;

        // 슬롯을 없엘 때 받는 액션 호출
        deleteItem(equipSkillPieceSlot.slotIdx);

        // 연출
        Sequence itemSeq = DOTween.Sequence()
            .Append(changeSkillPiece.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                skillRullet.ChangePiece(resultIdx, changeSkillPiece.GetComponent<RulletPiece>());
            })
            .Append(changeSkillPiece.transform.DOLocalMove(Vector3.zero, 1f))
            .AppendCallback(() =>
            {
                // 연출이 끝난 후 호출
                EndEquipRullet();
                // 다시 돌린다
                skillRullet.RollRullet();
            });
    }

    // 룰렛에 조각을 착용할 때 불리는 함수
    public void EquipRullet()
    {
        isEquip = true;
        InventoryCvsGroup.alpha = 0f;
    }

    // 룰렛에 조각 착용이 끝날 때 불리는 함수
    public void EndEquipRullet()
    {
        isEquip = false;
        InventoryCvsGroup.alpha = 1f;
    }

    // 인벤토리를 열고 닫는 함수
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
