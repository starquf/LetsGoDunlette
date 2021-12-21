using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
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

    public GameObject effectObj;

    public List<Sprite> effectSprites = new List<Sprite>();
    private Dictionary<EComboType, Sprite> effectDic;

    private void Awake()
    {
        GameManager.Instance.inventorySystem = this;
        AddDefaultSkill();

        effectDic = new Dictionary<EComboType, Sprite>();

        effectDic.Add(EComboType.None, effectSprites[0]);
        effectDic.Add(EComboType.Clover, effectSprites[1]);
        effectDic.Add(EComboType.Diamonds, effectSprites[2]);
        effectDic.Add(EComboType.Heart, effectSprites[3]);
        effectDic.Add(EComboType.Spade, effectSprites[4]);
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

    public void AddSkill(SkillPiece skill)
    {
        skill.transform.SetParent(transform);

        skills.Add(skill);
        unusedSkills.Add(skill);
    }

    public void SetUseSkill(SkillPiece skill)
    {
        usedSkills.Add(skill);

        for (int i = 0; i < 5; i++)
        {
            EffectObj effect = Instantiate(effectObj, null).GetComponent<EffectObj>();
            effect.SetSprite(effectDic[skill.comboType]);

            effect.transform.position = skill.skillImg.transform.position;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative();

            effect.Play(usedTrans.position, () =>
            {
                effect.GetComponent<SpriteRenderer>().DOFade(0f, 0.1f);
            }
            , BezierType.Quadratic, 0.5f);
        }

        skill.transform.SetParent(usedTrans);
        skill.transform.localPosition = Vector3.zero;
        skill.gameObject.SetActive(false);

        SetCountUI();
    }

    public SkillPiece GetRandomUnusedSkill()
    {
        // 비어있으면
        if (unusedSkills.Count <= 0)
        {
            for (int i = 0; i < usedSkills.Count; i++)
            {
                // 사용한거를 옮겨
                unusedSkills.Add(usedSkills[i]);
                usedSkills[i].transform.SetParent(transform);
                usedSkills[i].transform.localPosition = Vector3.zero;

                for (int j = 0; j < 5; j++)
                {
                    EffectObj effect = Instantiate(effectObj, null).GetComponent<EffectObj>();
                    effect.SetSprite(effectDic[usedSkills[i].comboType]);

                    effect.transform.position = usedTrans.position;

                    effect.Play(transform.position, () =>
                    {
                        effect.GetComponent<SpriteRenderer>().DOFade(0f, 0.1f);
                    }, BezierType.Quadratic);
                }
            }

            usedSkills.Clear();
        }

        int randIdx = Random.Range(0, unusedSkills.Count);

        SkillPiece result = unusedSkills[randIdx];
        result.gameObject.SetActive(true);

        unusedSkills.Remove(result);
        SetCountUI();

        return result;
    }
}
