using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryHandler : MonoBehaviour
{
    public List<SkillPiece> skills = new List<SkillPiece>();

    // 현재 가지고 있는 모든 스킬 인벤토리들
    public List<Inventory> inventorys = new List<Inventory>();
    public List<SkillPiece> graveyard = new List<SkillPiece>();

    public Transform usedTrans;

    public Text unusedCardCount;
    public Text usedCardCount;

    [Header("문양 이펙트 관련")]
    [SerializeField] private List<Sprite> effectSprites = new List<Sprite>();
    [SerializeField] private List<Gradient> effectGradients = new List<Gradient>();
    [SerializeField] private List<Sprite> bookmarkSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> pieceBGSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> pieceBGStrokeSprites = new List<Sprite>();

    public Dictionary<ElementalType, Sprite> effectSprDic;
    public Dictionary<ElementalType, Gradient> effectGradDic;
    public Dictionary<ElementalType, Sprite> bookmarkSprDic;
    public Dictionary<ElementalType, Sprite> pieceBGSprDic;
    public Dictionary<ElementalType, Sprite> pieceBGStrokeSprDic;

    private Tween unusedOpenTween;
    private Tween usedOpenTween;

    public event Action onUpdateInfo;

    private void Awake()
    {
        GameManager.Instance.inventoryHandler = this;

        //AddDefaultSkill();

        effectSprDic = new Dictionary<ElementalType, Sprite>();
        effectGradDic = new Dictionary<ElementalType, Gradient>();
        bookmarkSprDic = new Dictionary<ElementalType, Sprite>();
        pieceBGSprDic = new Dictionary<ElementalType, Sprite>();
        pieceBGStrokeSprDic = new Dictionary<ElementalType, Sprite>();

        for (int i = 0; i < 6; i++)
        {
            effectSprDic.Add((ElementalType)i, effectSprites[i]);
            effectGradDic.Add((ElementalType)i, effectGradients[i]);
            bookmarkSprDic.Add((ElementalType)i, bookmarkSprites[i]);
            pieceBGSprDic.Add((ElementalType)i, pieceBGSprites[i]);
            pieceBGStrokeSprDic.Add((ElementalType)i, pieceBGStrokeSprites[i]);
        }
    }

    public void SetCountUI()
    {
        //unusedCardCount.text = unusedSkills.Count.ToString();
        usedCardCount.text = graveyard.Count.ToString();

        onUpdateInfo?.Invoke();
    }

    // 스킬 추가 할땐 이걸 호출
    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory owner)
    {
        SkillPiece skill = Instantiate(skillPrefab, transform).GetComponent<SkillPiece>();
        skill.gameObject.SetActive(false);

        AddSkill(skill, owner);

        return skill;
    }

    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory owner, Vector3 makePos)
    {
        SkillPiece skill = CreateSkill(skillPrefab, owner);

        CreateSkillEffect(skill, makePos);

        return skill;
    }

    public void AddSkill(SkillPiece skill, Inventory owner)
    {
        skill.owner = owner;
        owner.skills.Add(skill);
        skills.Add(skill);

        //skills.Add(skill);
        //unusedSkills.Add(skill);

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
                return inventorys[i];
        }

        return null;
    }

    // 사용한 스킬은 이걸 호출
    public void SetSkillToGraveyard(SkillPiece skill)
    {
        if (skill == null)
        {
            Debug.LogError("null 발생!!");
            return;
        }

        if (skill.isDisposable)
        {
            RemovePiece(skill);
            SetCountUI();
            return;
        }

        skill.isInRullet = false;
        graveyard.Add(skill);

        skill.ResetPiece();

        for (int i = 0; i < 2; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[skill.currentType]);
            effect.SetColorGradient(effectGradDic[skill.currentType]);

            effect.transform.position = skill.skillImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(usedTrans.transform.position, () =>
            {
                usedOpenTween.Kill();
                usedOpenTween = usedTrans.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.15f)
                .OnComplete(() =>
                {
                    usedTrans.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
                });

                effect.EndEffect();
            }
            , BezierType.Quadratic, 0.4f);
        }

        skill.transform.SetParent(transform);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

        SetCountUI();
    }

    public void SetSkillToInventory(SkillPiece skill)
    {
        skill.isInRullet = false;

        skill.owner.skills.Add(skill);

        skill.ResetPiece();

        for (int i = 0; i < 2; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[skill.currentType]);
            effect.SetColorGradient(effectGradDic[skill.currentType]);

            effect.transform.position = skill.skillImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(transform.position, () =>
            {
                usedOpenTween.Kill();
                usedOpenTween = transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.15f)
                .OnComplete(() =>
                {
                    transform.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
                });

                effect.EndEffect();
            }
            , BezierType.Quadratic, 0.4f);
        }

        skill.transform.SetParent(transform);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

        SetCountUI();
    }

    // 직접 unusedSkill나 usedSkill를 순회해서 찾은 조각을 꺼내려면 이 함수를 사용해야됨
    public void GetSkillFromInventory(SkillPiece piece)
    {
        piece.gameObject.SetActive(true);
        piece.ResetPiece();

        for (int i = 0; i < inventorys.Count; i++)
        {
            inventorys[i].skills.Remove(piece);
        }

        graveyard.Remove(piece);

        SetCountUI();
    }

    // 랜덤한 안 쓴 스킬 하나 가져오기
    public SkillPiece GetRandomUnusedSkill()
    {
        // 비어있으면
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
            Debug.LogError("null 있음 !!");
        }

        result.ResetPiece();

        result.gameObject.SetActive(true);

        inven.skills.Remove(result);

        SetCountUI();

        return result;
    }

    public SkillPiece GetRandomPlayerOrEnemySkill(bool isPlayer)
    {
        // 비어있으면
        if (CheckAllInventoryEmpty())
        {
            CycleSkills();
        }

        // 적이나 플레이어 스킬이 없다면
        if (!CheckPlayerOrEnemySkill(isPlayer))
        {
            // 그냥 전체에서 하나 랜덤으로 준다
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

        SetCountUI();

        return result;
    }

    public bool CheckAllInventoryEmpty()
    {
        for(int i = 0; i < inventorys.Count; i++)
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

            // 사용한거를 옮겨
            graveyard[i].owner.skills.Add(graveyard[i]);

            graveyard[i].transform.SetParent(transform);
            graveyard[i].transform.localPosition = Vector3.zero;

            for (int j = 0; j < 2; j++)
            {
                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.SetSprite(effectSprDic[graveyard[i].currentType]);
                effect.SetColorGradient(effectGradDic[graveyard[i].currentType]);

                effect.transform.position = usedTrans.position;

                effect.Play(graveyard[i].owner.transform.position, () =>
                {
                    unusedOpenTween.Kill();
                    unusedOpenTween = transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.15f)
                    .OnComplete(() =>
                    {
                        transform.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
                    });

                    effect.EndEffect();

                }, BezierType.Quadratic, i * 0.05f);
            }
        }

        graveyard.Clear();
    }

    public void RemoveAllEnemyPiece() //모든 적스킬을 삭제
    {
        for (int i = inventorys.Count - 1; i >= 0; i--)
        {
            if (inventorys[i].isPlayerInven == false)
            {
                RemoveAllOwnerPiece(inventorys[i]);
            }
        }
    }

    public void RemoveAllOwnerPiece(Inventory owner)
    {
        for (int i = owner.skills.Count - 1; i >= 0; i--)
        {
            SkillPiece piece = owner.skills[i];

            // 룰렛 안에 있는 거면
            if (piece.isInRullet)
            {
                CreateSkillEffect(piece, piece.skillImg.transform.position);
                RemovePiece(piece);
            }
            else
            {
                RemovePiece(piece);
            }
        }

        owner.skills.Clear();
    }

    public void CreateSkillEffect(SkillPiece piece, Vector3 pos)
    {
        //print("이펙트 생성중!!!");

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
        if (piece == null) return;

        piece.KillTween();

        graveyard.Remove(piece);
        piece.owner.skills.Remove(piece);
        skills.Remove(piece);

        Destroy(piece.gameObject);

        SetCountUI();
    }

    // 플레이어/적 스킬 유무
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
