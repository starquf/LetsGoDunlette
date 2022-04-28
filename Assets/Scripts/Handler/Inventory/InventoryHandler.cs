using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryHandler : MonoBehaviour
{
    // 현재 가지고 있는 모든 스킬들
    public List<SkillPiece> unusedSkills = new List<SkillPiece>();
    public List<SkillPiece> skills = new List<SkillPiece>();
    public List<SkillPiece> usedSkills = new List<SkillPiece>();

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
        unusedCardCount.text = unusedSkills.Count.ToString();
        usedCardCount.text = usedSkills.Count.ToString();

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

    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory owner, Vector3 makePos, Action onEndCreate = null)
    {
        SkillPiece skill = Instantiate(skillPrefab, transform).GetComponent<SkillPiece>();
        skill.transform.position = makePos;

        skill.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        skill.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), 0.5f)
            .SetDelay(0.5f);

        skill.transform.DOMove(transform.position, 0.5f)
            .SetDelay(0.24f)
            .OnComplete(() =>
            {
                skill.gameObject.SetActive(false);
                onEndCreate?.Invoke();
            });

        AddSkill(skill, owner);

        return skill;
    }

    public void AddSkill(SkillPiece skill, Inventory owner)
    {
        skill.gameObject.SetActive(false);

        skill.transform.SetParent(transform);
        skill.owner = owner;
        owner.skills.Add(skill);

        skills.Add(skill);
        unusedSkills.Add(skill);

        SetCountUI();
    }

    // 사용한 스킬은 이걸 호출
    public void SetUseSkill(SkillPiece skill)
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
        usedSkills.Add(skill);

        skill.ResetPiece();

        for (int i = 0; i < 5; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[skill.currentType]);
            effect.SetColorGradient(effectGradDic[skill.currentType]);

            effect.transform.position = skill.skillImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(usedTrans.position, () =>
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

        skill.transform.SetParent(usedTrans);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

        SetCountUI();
    }

    public void SetUnUseSkill(SkillPiece skill)
    {
        skill.isInRullet = false;
        unusedSkills.Add(skill);

        skill.ResetPiece();

        for (int i = 0; i < 5; i++)
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

        unusedSkills.Remove(piece);
        usedSkills.Remove(piece);

        SetCountUI();
    }

    // 랜덤한 안 쓴 스킬 하나 가져오기
    public SkillPiece GetRandomUnusedSkill()
    {
        // 비어있으면
        if (unusedSkills.Count <= 0)
        {
            CycleSkills();
        }

        int randIdx = Random.Range(0, unusedSkills.Count);

        SkillPiece result = unusedSkills[randIdx];

        if (result == null)
        {
            Debug.LogError("null 있음 !!");
        }

        result.ResetPiece();

        result.gameObject.SetActive(true);

        unusedSkills.Remove(result);

        SetCountUI();

        return result;
    }

    public SkillPiece GetRandomPlayerOrEnemySkill(bool isPlayer)
    {
        // 비어있으면
        if (unusedSkills.Count <= 0)
        {
            CycleSkills();
        }

        List<SkillPiece> filterdSkill = new List<SkillPiece>();

        for (int i = 0; i < unusedSkills.Count; i++)
        {
            if (unusedSkills[i].isPlayerSkill == isPlayer)
            {
                filterdSkill.Add(unusedSkills[i]);
            }
        }

        // 적이나 플레이어 스킬이 없다면
        if (filterdSkill.Count == 0)
        {
            // 그냥 전체에서 하나 랜덤으로 준다
            return GetRandomUnusedSkill();
        }

        int randIdx = Random.Range(0, filterdSkill.Count);

        SkillPiece result = filterdSkill[randIdx];
        result.ResetPiece();

        result.gameObject.SetActive(true);

        unusedSkills.Remove(result);

        SetCountUI();

        return result;
    }

    public void CycleSkills()
    {
        for (int i = 0; i < usedSkills.Count; i++)
        {
            if (usedSkills[i] == null)
            {
                continue;
            }

            // 사용한거를 옮겨
            unusedSkills.Add(usedSkills[i]);
            usedSkills[i].transform.SetParent(transform);
            usedSkills[i].transform.localPosition = Vector3.zero;

            for (int j = 0; j < 5; j++)
            {
                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.SetSprite(effectSprDic[usedSkills[i].currentType]);
                effect.SetColorGradient(effectGradDic[usedSkills[i].currentType]);

                effect.transform.position = usedTrans.position;

                effect.Play(transform.position, () =>
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

        usedSkills.Clear();
    }

    public void RemoveAllEnemyPiece() //모든 적스킬을 삭제
    {
        for (int i = skills.Count - 1; i >= 0; i--)
        {
            if (!skills[i].isPlayerSkill)
            {
                RemovePiece(skills[i]);
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
                CreateRemoveEffect(piece);
                RemovePiece(piece);
            }
            else
            {
                RemovePiece(piece);
            }
        }

        owner.skills.Clear();
    }

    private void CreateRemoveEffect(SkillPiece piece)
    {
        //print("이펙트 생성중!!!");

        for (int i = 0; i < 5; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[piece.currentType]);
            effect.SetColorGradient(effectGradDic[piece.currentType]);

            Vector3 pos = piece.skillImg.transform.position;
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

        skills.Remove(piece);
        usedSkills.Remove(piece);
        unusedSkills.Remove(piece);
        piece.owner.skills.Remove(piece);

        Destroy(piece.gameObject);
        SetCountUI();
    }

    public void RemoveNull()
    {
        unusedSkills.Clear();

        for (int i = 0; i < skills.Count; i++)
        {
            unusedSkills.Add(skills[i]);
        }

        SetCountUI();
    }

    // 플레이어/적 스킬 유무
    public bool CheckPlayerOrEnemyInUnUsedInven(bool isPlayer)
    {
        for (int i = 0; i < unusedSkills.Count; i++)
        {
            if (unusedSkills[i].isPlayerSkill == isPlayer)
            {
                return true;
            }
        }

        return false;
    }
}
