using System;
using System.Collections.Generic;
using UnityEngine;

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

    public abstract void ResultSet(int resultIdx);
    public abstract void Result();
}
