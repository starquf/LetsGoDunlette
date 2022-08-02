using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PieceDicHandler : MonoBehaviour
{
    public GameObject cardInfoObj;
    public Transform pieceHolder;
    public Transform pieceParent;

    public List<SkillPiece> skills = new List<SkillPiece>();

    [Header("컨트롤 버튼들")]
    public List<Button> elementalBtns = new List<Button>();
    public List<Toggle> toggles = new List<Toggle>();

    private List<SkillPiece> showedPieceList = new List<SkillPiece>();

    private Dictionary<ElementalType, List<SkillPiece>> pieceListDic;

    public PieceDesUIHandler desUI;

    private PlayerInventory player;

    private GradeRange gradeRange = GradeRange.All;
    private ElementalType currentType = ElementalType.None;


    [SerializeField] private List<Sprite> pieceBGStrokeSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> targetBGSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> targetIconSprites = new List<Sprite>();

    public Dictionary<ElementalType, Sprite> pieceBGStrokeSprDic;
    public Dictionary<ElementalType, Sprite> targetBGSprDic;
    public Dictionary<SkillRange, Sprite> targetIconSprDic;

    private bool isInit = false;

    private void Awake()
    {
        PoolManager.CreatePool<CardInfo_SC>(cardInfoObj, pieceHolder, 10);
    }

    private void Start()
    {
        InitDic();

        SelectElemental(ElementalType.None);

        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null;

        for (int i = 0; i < toggles.Count; i++)
        {
            int a = i;

            toggles[a].onValueChanged.AddListener(enable =>
            {
                GradeRange grade = (GradeRange)(1 << a);

                if (enable)
                {
                    gradeRange |= grade;
                }
                else
                {
                    gradeRange &= ~grade;
                }

                SelectElemental(currentType);
            });
        }
    }

    private void InitDic()
    {
        pieceListDic = new Dictionary<ElementalType, List<SkillPiece>>();

        pieceBGStrokeSprDic = new Dictionary<ElementalType, Sprite>();
        targetBGSprDic = new Dictionary<ElementalType, Sprite>();
        targetIconSprDic = new Dictionary<SkillRange, Sprite>();

        for (int i = 0; i < (int)ElementalType.Monster; i++)
        {
            pieceListDic.Add((ElementalType)i, new List<SkillPiece>());
            pieceBGStrokeSprDic.Add((ElementalType)i, pieceBGStrokeSprites[i]);
            targetBGSprDic.Add((ElementalType)i, targetBGSprites[i]);

            ElementalType type = (ElementalType)i;

            elementalBtns[i].onClick.AddListener(() =>
            {
                SelectElemental(type);
            });
        }

        for (int i = 0; i < 3; i++)
        {
            targetIconSprDic.Add((SkillRange)i, targetIconSprites[i]);
        }

        for (int i = 0; i < skills.Count; i++)
        {
            SkillPiece sp = Instantiate(skills[i], pieceParent);
            sp.Owner = player;

            sp.gameObject.SetActive(false);

            pieceListDic[skills[i].patternType].Add(sp);
        }

        for (int i = 0; i < (int)ElementalType.Monster; i++)
        {
            ElementalType type = (ElementalType)i;

            pieceListDic[type] = pieceListDic[type].OrderBy(sp => (int)sp.skillGrade).ThenBy(sp => sp.PieceName).ToList();
        }

        isInit = true;
    }

    public void SelectElemental(ElementalType elemental)
    {
        currentType = elemental;

        for (int i = 0; i < pieceHolder.childCount; i++)
        {
            pieceHolder.GetChild(i).gameObject.SetActive(false);
        }

        showedPieceList = pieceListDic[elemental];

        for (int i = 0; i < showedPieceList.Count; i++)
        {
            bool isShow = true;

            SkillPiece sp = showedPieceList[i];

            switch (sp.skillGrade)
            {
                case GradeInfo.Normal:

                    if (!gradeRange.HasFlag(GradeRange.Normal))
                    {
                        isShow = false;
                    }
                    break;

                case GradeInfo.Epic:

                    if (!gradeRange.HasFlag(GradeRange.Epic))
                    {
                        isShow = false;
                    }

                    break;

                case GradeInfo.Legend:

                    if (!gradeRange.HasFlag(GradeRange.Legend))
                    {
                        isShow = false;
                    }

                    break;
            }

            if (!isShow)
            {
                continue;
            }

            CardInfo_SC cardInfoUI = PoolManager.GetItem<CardInfo_SC>();

            //print(sp.cardBG + " " + sp.PieceName);

            cardInfoUI.SetInfo(sp.cardBG, pieceBGStrokeSprDic[sp.patternType], sp.skillGrade, targetIconSprDic[sp.skillRange], 
                targetBGSprDic[sp.patternType], sp.PieceName);

            cardInfoUI.transform.SetParent(pieceHolder);
            cardInfoUI.transform.SetAsLastSibling();

            cardInfoUI.btn.onClick.AddListener(() =>
            {
                desUI.ShowDescription(sp);
            });
        }
    }

    public void ChangePlayer(PlayerInventory player)
    {
        this.player = player;

        if (!isInit)
            return;

        foreach (var elemental in pieceListDic.Keys)
        {
            for (int i = 0; i < pieceListDic[elemental].Count; i++)
            {
                pieceListDic[elemental][i].Owner = player;
            }
        }
    }
}
