using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class RandomEncounter : MonoBehaviour
{
    [HideInInspector] public EncounterInfoHandler encounterInfoHandler;
    [HideInInspector] public Action<bool> OnExitEncounter;
    [HideInInspector] public Action ShowEndEncounter;

    [Header("선택전")]
    public Sprite en_Start_Image;
    public string en_Name, en_Start_Text;
    public List<string> en_ChoiceList;
    public int en_Choice_Count;

    [Header("선택후")]
    public List<Sprite> en_End_Image;
    public List<string> en_End_TextList;
    [HideInInspector] public string en_End_Result, showText;
    [HideInInspector] public Sprite showImg;
    [HideInInspector] public bool isEffectEnd;
    protected int choiceIdx;

    protected BattleHandler bh;
    public virtual void Init()
    {
        bh = GameManager.Instance.battleHandler;
    }

    public void MakeScroll(ScrollType scrollType, out Scroll scroll)
    {
        scroll = PoolManager.GetScroll(scrollType);
        scroll.transform.position = Vector2.zero;
        Image scrollImg = scroll.GetComponent<Image>();
        scrollImg.color = new Color(1, 1, 1, 0);
        scroll.transform.SetParent(encounterInfoHandler.transform);
        scroll.GetComponent<RectTransform>().sizeDelta = Vector2.one * 400f;
        scroll.transform.localScale = Vector3.one;
    }

    public void MakeSkill(SkillPiece piece, out SkillPiece skill)
    {
        skill = Instantiate(piece).GetComponent<SkillPiece>();
        skill.transform.position = Vector2.zero;
        skill.transform.rotation = Quaternion.Euler(0, 0, 30f);
        Image skillImg = skill.GetComponent<Image>();
        skillImg.color = new Color(1, 1, 1, 0);
        skill.transform.SetParent(encounterInfoHandler.transform);
        skill.transform.localScale = Vector3.one;
        //skillImg.DOFade(1, 0.5f).SetDelay(1f);
    }

    public void GetSkillInRandomEncounterAnim(SkillPiece skill, Action OnComplete = null, bool fadeInSkip = false)
    {
        GameManager.Instance.getPieceHandler.GetPiecePlayer(skill, OnComplete, () =>
        {
            Inventory owner = bh.player.GetComponent<Inventory>();
            Transform unusedInventoryTrm = owner.indicator.transform;
            Image skillImg = skill.GetComponent<Image>();
            if (fadeInSkip)
            {
                DOTween.Sequence()
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skillImg.DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    skill.GetComponent<Image>().color = Color.white;
                    skill.gameObject.SetActive(false);

                    OnComplete?.Invoke();
                });
            }
            else
            {
                DOTween.Sequence()
                .Append(skillImg.DOFade(1, 0.5f))
                .AppendInterval(0.1f)
                .Append(skill.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                .Join(skill.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                .Join(skillImg.DOFade(0f, 0.5f))
                .OnComplete(() =>
                {
                    skill.GetComponent<Image>().color = Color.white;
                //skill.transform.SetParent
                skill.gameObject.SetActive(false);

                    OnComplete?.Invoke();
                });
            }
        });
    }

    public abstract void ResultSet(int resultIdx);
    public abstract void Result();
}
