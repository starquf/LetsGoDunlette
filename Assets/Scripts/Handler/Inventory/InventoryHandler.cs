using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    // 현재 가지고 있는 모든 스킬들
    public List<SkillPiece> unusedSkills = new List<SkillPiece>();
    public List<SkillPiece> skills = new List<SkillPiece>();
    public List<SkillPiece> usedSkills = new List<SkillPiece>();

    [Header("기본으로 주는 스킬 프리팹")]
    public List<GameObject> defaultSkills = new List<GameObject>();

    public Transform usedTrans;

    public Text unusedCardCount;
    public Text usedCardCount;

    [Header("문양 이펙트 관련")]
    [SerializeField] private List<Sprite> effectSprites = new List<Sprite>();
    [SerializeField] private List<Gradient> effectGradients = new List<Gradient>();

    public Dictionary<PatternType, Sprite> effectSprDic;
    public Dictionary<PatternType, Gradient> effectGradDic;

    private void Awake()
    {
        GameManager.Instance.inventoryHandler = this;

        AddDefaultSkill();

        effectSprDic = new Dictionary<PatternType, Sprite>();
        effectGradDic = new Dictionary<PatternType, Gradient>();

        for (int i = 0; i < 5; i++)
        {
            effectSprDic.Add((PatternType)i, effectSprites[i]);
            effectGradDic.Add((PatternType)i, effectGradients[i]);
        }
    }

    public void SetCountUI()
    {
        unusedCardCount.text = unusedSkills.Count.ToString();
        usedCardCount.text = usedSkills.Count.ToString();
    }

    private void AddDefaultSkill()
    {
        for (int i = 0; i < defaultSkills.Count; i++)
        {
            SkillPiece skill = Instantiate(defaultSkills[i], transform).GetComponent<SkillPiece>();
            skill.gameObject.SetActive(false);

            AddSkill(skill);
        }
    }

    // 스킬 추가 할땐 이걸 호출
    public void CreateSkill(GameObject skillPrefab, Vector3 makePos)
    {
        SkillPiece skill = Instantiate(skillPrefab, transform).GetComponent<SkillPiece>();
        skill.transform.position = makePos;

        skill.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        skill.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), 0.5f)
            .SetDelay(0.5f);

        skill.transform.DOMove(transform.position, 0.5f)
            .SetDelay(0.5f)
            .OnComplete(() => {
                skill.gameObject.SetActive(false);
            });

        AddSkill(skill);
    }

    public void AddSkill(SkillPiece skill)
    {
        skill.transform.SetParent(transform);

        skills.Add(skill);
        unusedSkills.Add(skill);

        SetCountUI();
    }

    // 사용한 스킬은 이걸 호출
    public void SetUseSkill(SkillPiece skill)
    {
        usedSkills.Add(skill);

        skill.ResetPiece();

        for (int i = 0; i < 5; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[skill.patternType]);
            effect.SetColorGradient(effectGradDic[skill.patternType]);

            effect.transform.position = skill.skillImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(usedTrans.position, () =>
            {
                effect.Sr.DOFade(0f, 0.1f)
                .OnComplete(() =>
                {
                    effect.EndEffect();
                });
            }
            , BezierType.Quadratic, 0.5f);
        }

        skill.transform.SetParent(usedTrans);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

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
        result.gameObject.SetActive(true);

        unusedSkills.Remove(result);

        SetCountUI();

        return result;
    }

    private void CycleSkills()
    {
        for (int i = 0; i < usedSkills.Count; i++)
        {
            // 사용한거를 옮겨
            unusedSkills.Add(usedSkills[i]);
            usedSkills[i].transform.SetParent(transform);
            usedSkills[i].transform.localPosition = Vector3.zero;

            for (int j = 0; j < 5; j++)
            {
                EffectObj effect = PoolManager.GetItem<EffectObj>();
                effect.SetSprite(effectSprDic[usedSkills[i].patternType]);
                effect.SetColorGradient(effectGradDic[usedSkills[i].patternType]);

                effect.transform.position = usedTrans.position;

                effect.Play(transform.position, () =>
                {
                    effect.Sr.DOFade(0f, 0.1f)
                    .OnComplete(() =>
                    {
                        effect.EndEffect();
                    });

                }, BezierType.Quadratic, i * 0.05f);
            }
        }

        usedSkills.Clear();
    }
}
