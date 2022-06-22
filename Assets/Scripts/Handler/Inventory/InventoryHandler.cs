using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryHandler : MonoBehaviour
{
    public List<SkillPiece> skills = new List<SkillPiece>();

    // ���� ������ �ִ� ��� ��ų �κ��丮��
    public List<Inventory> inventorys = new List<Inventory>();
    public List<SkillPiece> graveyard = new List<SkillPiece>();

    public InventoryIndicator graveyardIndicator;

    [Header("ī�� ����")]
    [SerializeField] private List<Sprite> effectSprites = new List<Sprite>();
    [SerializeField] private List<Gradient> effectGradients = new List<Gradient>();
    [SerializeField] private List<Sprite> pieceBGSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> pieceBGStrokeSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> targetBGSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> targetIconSprites = new List<Sprite>();


    public Dictionary<ElementalType, Sprite> effectSprDic;
    public Dictionary<ElementalType, Gradient> effectGradDic;
    public Dictionary<ElementalType, Sprite> pieceBGSprDic;
    public Dictionary<ElementalType, Sprite> pieceBGStrokeSprDic;
    public Dictionary<ElementalType, Sprite> targetBGSprDic;
    public Dictionary<SkillRange, Sprite> targetIconSprDic;

    public event Action onUpdateInfo;

    private void Awake()
    {
        GameManager.Instance.inventoryHandler = this;

        //AddDefaultSkill();

        effectSprDic = new Dictionary<ElementalType, Sprite>();
        effectGradDic = new Dictionary<ElementalType, Gradient>();
        pieceBGSprDic = new Dictionary<ElementalType, Sprite>();
        pieceBGStrokeSprDic = new Dictionary<ElementalType, Sprite>();
        targetBGSprDic = new Dictionary<ElementalType, Sprite>();
        targetIconSprDic = new Dictionary<SkillRange, Sprite>();

        for (int i = 0; i < 6; i++)
        {
            effectSprDic.Add((ElementalType)i, effectSprites[i]);
            effectGradDic.Add((ElementalType)i, effectGradients[i]);
            pieceBGSprDic.Add((ElementalType)i, pieceBGSprites[i]);
            pieceBGStrokeSprDic.Add((ElementalType)i, pieceBGStrokeSprites[i]);
            targetBGSprDic.Add((ElementalType)i, targetBGSprites[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            targetIconSprDic.Add((SkillRange)i, targetIconSprites[i]);
        }
    }

    public void SetCountUI()
    {
        //unusedCardCount.text = unusedSkills.Count.ToString();
        graveyardIndicator.SetText(graveyard.Count);
        onUpdateInfo?.Invoke();
    }

    // ��ų �߰� �Ҷ� �̰� ȣ��
    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory owner)
    {
        SkillPiece skill = Instantiate(skillPrefab, transform).GetComponent<SkillPiece>(); //���� �� ������?
        skill.gameObject.SetActive(false);

        AddSkill(skill, owner);

        return skill;
    }

    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory Owner, Vector3 makePos)
    {
        SkillPiece skill = CreateSkill(skillPrefab, Owner);

        CreateSkillEffect(skill, makePos);

        return skill;
    }

    public void AddSkill(SkillPiece skill, Inventory owner)
    {
        skill.Owner = owner;
        owner.skills.Add(skill);
        skills.Add(skill);

        owner.indicator.SetText(owner.skills.Count);
        owner.indicator.ShowEffect();

        SetCountUI();
    }

    public void AddInventory(Inventory inven)
    {
        inventorys.Add(inven);
    }

    public void RemoveInventory(Inventory inven)
    {
        inventorys.Remove(inven);
    }

    public Inventory GetPlayerInventory()
    {
        for (int i = 0; i < inventorys.Count; i++)
        {
            if (inventorys[i].isPlayerInven)
            {
                return inventorys[i];
            }
        }

        return null;
    }

    // ����� ��ų�� �̰� ȣ��
    public void SetSkillToGraveyard(SkillPiece skill)
    {
        if (skill == null)
        {
            Debug.LogError("null �߻�!!");
            return;
        }

        skill.ResetPiece();

        if (skill.isDisposable)
        {
            RemovePiece(skill);
            SetCountUI();
            return;
        }

        skill.IsInRullet = false;
        graveyard.Add(skill);

        for (int i = 0; i < 2; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[skill.currentType]);
            effect.SetColorGradient(effectGradDic[skill.currentType]);

            effect.transform.position = skill.skillIconImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(graveyardIndicator.transform.position, () =>
            {
                graveyardIndicator.ShowEffect();

                effect.EndEffect();
            }
            , BezierType.Quadratic, 0.4f);
        }

        skill.transform.SetParent(transform);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

        graveyardIndicator.SetText(graveyard.Count);

        SetCountUI();
    }

    public void SetSkillToInventory(SkillPiece skill)
    {
        skill.IsInRullet = false;

        Inventory Owner = skill.Owner;

        Owner.skills.Add(skill);

        skill.ResetPiece();

        Owner.indicator.SetText(Owner.skills.Count);
        Owner.indicator.ShowEffect();

        for (int i = 0; i < 2; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[skill.currentType]);
            effect.SetColorGradient(effectGradDic[skill.currentType]);

            effect.transform.position = skill.skillIconImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(Owner.indicator.transform.position, () =>
            {
                Owner.indicator.ShowEffect();

                effect.EndEffect();
            }
            , BezierType.Quadratic, 0.4f);
        }

        skill.transform.SetParent(transform);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

        SetCountUI();
    }

    // ���� unusedSkill�� usedSkill�� ��ȸ�ؼ� ã�� ������ �������� �� �Լ��� ����ؾߵ�
    public void GetSkillFromInventoryOrGraveyard(SkillPiece piece)
    {
        piece.gameObject.SetActive(true);
        piece.ResetPiece();

        piece.Owner.skills.Remove(piece);
        graveyard.Remove(piece);

        Inventory Owner = piece.Owner;

        Owner.indicator.SetText(Owner.skills.Count);
        Owner.indicator.ShowEffect();

        SetCountUI();
    }

    // ������ �� �� ��ų �ϳ� ��������
    public SkillPiece GetRandomUnusedSkill()
    {
        // ���������
        if (CheckAllInventoryEmpty())
        {
            CycleSkills();
        }

        List<Inventory> filterdInven = new List<Inventory>();

        for (int i = 0; i < inventorys.Count; i++)
        {
            if (inventorys[i].skills.Count > 0)
            {
                filterdInven.Add(inventorys[i]);
            }
        }

        int randIdx = Random.Range(0, filterdInven.Count);
        Inventory inven = filterdInven[randIdx];

        randIdx = Random.Range(0, inven.skills.Count);
        SkillPiece result = inven.skills[randIdx];

        if (result == null)
        {
            Debug.LogError("null ���� !!");
        }

        result.ResetPiece();

        result.gameObject.SetActive(true);

        inven.skills.Remove(result);

        inven.indicator.SetText(inven.skills.Count);
        inven.indicator.ShowEffect();

        SetCountUI();

        return result;
    }

    public SkillPiece GetRandomPlayerOrEnemySkill(bool isPlayer)
    {
        // ���������
        if (CheckAllInventoryEmpty())
        {
            CycleSkills();
        }

        // ���̳� �÷��̾� ��ų�� ���ٸ�
        if (!CheckPlayerOrEnemySkill(isPlayer))
        {
            // �׳� ��ü���� �ϳ� �������� �ش�
            return GetRandomUnusedSkill();
        }

        List<Inventory> filterdInven = new List<Inventory>();

        for (int i = 0; i < inventorys.Count; i++)
        {
            if (inventorys[i].isPlayerInven == isPlayer && inventorys[i].skills.Count > 0)
            {
                filterdInven.Add(inventorys[i]);
            }
        }

        if (filterdInven.Count <= 0)
        {
            return GetRandomUnusedSkill();
        }

        int randIdx = Random.Range(0, filterdInven.Count);
        Inventory inven = filterdInven[randIdx];

        randIdx = Random.Range(0, inven.skills.Count);
        SkillPiece result = inven.skills[randIdx];

        result.ResetPiece();

        result.gameObject.SetActive(true);

        inven.skills.Remove(result);

        inven.indicator.SetText(inven.skills.Count);
        inven.indicator.ShowEffect();

        SetCountUI();

        return result;
    }

    public bool CheckAllInventoryEmpty()
    {
        for (int i = 0; i < inventorys.Count; i++)
        {
            if (inventorys[i].skills.Count > 0)
            {
                return false;
            }
        }

        return true;
    }

    public void CycleSkills()
    {
        for (int i = 0; i < graveyard.Count; i++)
        {
            if (graveyard[i] == null)
            {
                continue;
            }

            // ����ѰŸ� �Ű�
            graveyard[i].Owner.skills.Add(graveyard[i]);

            graveyard[i].transform.SetParent(transform);
            graveyard[i].transform.localPosition = Vector3.zero;

            Inventory Owner = graveyard[i].Owner;

            Owner.indicator.SetText(Owner.skills.Count);
            Owner.indicator.ShowEffect();

            for (int j = 0; j < 2; j++)
            {
                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.SetSprite(effectSprDic[graveyard[i].currentType]);
                effect.SetColorGradient(effectGradDic[graveyard[i].currentType]);

                effect.transform.position = graveyardIndicator.transform.position;

                effect.Play(graveyard[i].Owner.indicator.transform.position, () =>
                {
                    graveyardIndicator.ShowEffect();

                    effect.EndEffect();

                }, BezierType.Quadratic, i * 0.05f);
            }
        }

        graveyard.Clear();

        SetCountUI();
    }

    public void RemoveAllEnemyPiece() //��� ����ų�� ����
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            SkillPiece piece = skills[i];

            if (!piece.isPlayerSkill)
            {
                // �귿 �ȿ� �ִ� �Ÿ�
                if (piece.IsInRullet)
                {
                    CreateSkillEffect(piece, piece.skillIconImg.transform.position);
                    RemovePiece(piece);
                }
                else
                {
                    RemovePiece(piece);
                }
            }
        }
    }

    public void RemoveAllOwnerPiece(Inventory Owner)
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            SkillPiece piece = skills[i];

            if (piece.Owner == Owner)
            {
                // �귿 �ȿ� �ִ� �Ÿ�
                if (piece.IsInRullet)
                {
                    CreateSkillEffect(piece, piece.skillIconImg.transform.position);
                    RemovePiece(piece);
                }
                else
                {
                    RemovePiece(piece);
                }
            }
        }

        Owner.skills.Clear();
    }

    public void RemoveAllDisposable(Inventory Owner)
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            SkillPiece piece = skills[i];

            if (piece.Owner == Owner)
            {
                // �귿 �ȿ� �ִ� �Ÿ�
                if (!piece.IsInRullet)
                {
                    if (piece.isDisposable)
                    {
                        RemovePiece(piece);
                        SetCountUI();
                    }
                }
            }
        }
    }

    public void CreateSkillEffect(SkillPiece piece, Vector3 pos)
    {
        //print("����Ʈ ������!!!");

        for (int i = 0; i < 3; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[piece.currentType]);
            effect.SetColorGradient(effectGradDic[piece.currentType]);

            pos.z = 0f;

            effect.transform.position = pos;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative()
                .OnComplete(() => effect.EndEffect());
        }
    }

    public void RemovePiece(SkillPiece piece)
    {
        if (piece == null)
        {
            return;
        }

        piece.ResetPiece();

        CreateSkillEffect(piece, piece.skillIconImg.transform.position);

        graveyard.Remove(piece);
        piece.Owner.skills.Remove(piece);
        skills.Remove(piece);

        piece.Owner.indicator.SetText(piece.Owner.skills.Count);
        graveyardIndicator.SetText(graveyard.Count);

        Destroy(piece.gameObject);

        SetCountUI();
    }

    // �÷��̾�/�� ��ų ����
    public bool CheckPlayerOrEnemySkill(bool isPlayer)
    {
        for (int i = 0; i < inventorys.Count; i++)
        {
            if (inventorys[i].isPlayerInven == isPlayer)
            {
                if (inventorys[i].skills.Count > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
