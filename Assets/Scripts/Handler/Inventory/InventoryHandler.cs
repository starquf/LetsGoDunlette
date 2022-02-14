using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    // ���� ������ �ִ� ��� ��ų��
    public List<SkillPiece> unusedSkills = new List<SkillPiece>();
    public List<SkillPiece> skills = new List<SkillPiece>();
    public List<SkillPiece> usedSkills = new List<SkillPiece>();

    public Transform usedTrans;

    public Text unusedCardCount;
    public Text usedCardCount;

    [Header("���� ����Ʈ ����")]
    [SerializeField] private List<Sprite> effectSprites = new List<Sprite>();
    [SerializeField] private List<Gradient> effectGradients = new List<Gradient>();
    [SerializeField] private List<Sprite> bookmarkSprites = new List<Sprite>();

    public Dictionary<PatternType, Sprite> effectSprDic;
    public Dictionary<PatternType, Gradient> effectGradDic;
    public Dictionary<PatternType, Sprite> bookmarkSprDic;

    private Tween unusedOpenTween;
    private Tween usedOpenTween;

    private void Awake()
    {
        GameManager.Instance.inventoryHandler = this;

        //AddDefaultSkill();

        effectSprDic = new Dictionary<PatternType, Sprite>();
        effectGradDic = new Dictionary<PatternType, Gradient>();
        bookmarkSprDic = new Dictionary<PatternType, Sprite>();

        for (int i = 0; i < 6; i++)
        {
            effectSprDic.Add((PatternType)i, effectSprites[i]);
            effectGradDic.Add((PatternType)i, effectGradients[i]);
            bookmarkSprDic.Add((PatternType)i, bookmarkSprites[i]);
        }
    }

    public void SetCountUI()
    {
        unusedCardCount.text = unusedSkills.Count.ToString();
        usedCardCount.text = usedSkills.Count.ToString();
    }

    // ��ų �߰� �Ҷ� �̰� ȣ��
    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory owner)
    {
        SkillPiece skill = Instantiate(skillPrefab, transform).GetComponent<SkillPiece>();
        skill.gameObject.SetActive(false);
        skill.owner = owner;

        AddSkill(skill);

        return skill;
    }

    public SkillPiece CreateSkill(GameObject skillPrefab, Inventory owner, Vector3 makePos)
    {
        SkillPiece skill = Instantiate(skillPrefab, transform).GetComponent<SkillPiece>();
        skill.transform.position = makePos;
        skill.owner = owner;

        skill.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        skill.transform.DOScale(new Vector3(0.2f, 0.2f, 1f), 0.5f)
            .SetDelay(0.5f);

        skill.transform.DOMove(transform.position, 0.5f)
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                skill.gameObject.SetActive(false);
            });

        AddSkill(skill);

        return skill;
    }

    public void AddSkill(SkillPiece skill)
    {
        skill.transform.SetParent(transform);

        skills.Add(skill);
        unusedSkills.Add(skill);

        SetCountUI();
    }

    // ����� ��ų�� �̰� ȣ��
    public void SetUseSkill(SkillPiece skill)
    {
        skill.isInRullet = false;
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
            effect.SetSprite(effectSprDic[skill.patternType]);
            effect.SetColorGradient(effectGradDic[skill.patternType]);

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

    // ���� unusedSkill�� usedSkill�� ��ȸ�ؼ� ã�� ������ �������� �� �Լ��� ����ؾߵ�
    public void GetSkillFromInventory(SkillPiece piece)
    {
        piece.gameObject.SetActive(true);

        unusedSkills.Remove(piece);
        usedSkills.Remove(piece);

        SetCountUI();
    }

    // ������ �� �� ��ų �ϳ� ��������
    public SkillPiece GetRandomUnusedSkill()
    {
        // ���������
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
        // ���������
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

        // ���̳� �÷��̾� ��ų�� ���ٸ�
        if (filterdSkill.Count == 0)
        {
            // �׳� ��ü���� �ϳ� �������� �ش�
            return GetRandomUnusedSkill();
        }

        int randIdx = Random.Range(0, filterdSkill.Count);

        SkillPiece result = filterdSkill[randIdx];
        result.gameObject.SetActive(true);

        unusedSkills.Remove(result);

        SetCountUI();

        return result;
    }

    public void CycleSkills()
    {
        for (int i = 0; i < usedSkills.Count; i++)
        {
            // ����ѰŸ� �Ű�
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

    public void RemoveAllEnemyPiece() //��� ����ų�� ����
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
        for (int i = 0; i < owner.skills.Count; i++)
        {
            SkillPiece piece = owner.skills[i];

            // �귿 �ȿ� �ִ� �Ÿ�
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
    }

    private void CreateRemoveEffect(SkillPiece piece)
    {
        print("����Ʈ ������!!!");

        for (int i = 0; i < 5; i++)
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.SetSprite(effectSprDic[piece.patternType]);
            effect.SetColorGradient(effectGradDic[piece.patternType]);

            Vector3 pos = piece.skillImg.transform.position;
            pos.z = 0f;

            effect.transform.position = pos;

            effect.transform.DOMove(Random.insideUnitCircle * 1.5f, 0.4f)
                .SetRelative()
                .OnComplete(() => effect.EndEffect());
        }
    }

    private void RemovePiece(SkillPiece piece)
    {
        skills.Remove(piece);
        usedSkills.Remove(piece);
        unusedSkills.Remove(piece);

        Destroy(piece.gameObject);
        SetCountUI();
    }
}
