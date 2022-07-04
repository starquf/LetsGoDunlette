using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PieceDicHandler : MonoBehaviour
{
    public GameObject pieceInfoObj;
    public Transform pieceHolder;
    public Transform pieceParent;

    public List<SkillPiece> skills = new List<SkillPiece>();

    [Header("컨트롤 버튼들")]
    public List<Button> elementalBtns = new List<Button>();
    public List<Toggle> toggles = new List<Toggle>();

    private List<SkillPiece> showedPieceList = new List<SkillPiece>();

    private Dictionary<ElementalType, List<SkillPiece>> pieceListDic;

    public PieceDesUIHandler desUI;
    public PlayerInventory player;

    private GradeRange gradeRange = GradeRange.All;
    private ElementalType currentType = ElementalType.None;

    private void Awake()
    {
        PoolManager.CreatePool<PieceInfoUI>(pieceInfoObj, pieceHolder, 10);

        InitDic();
    }

    private void Start()
    {
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

        for (int i = 0; i < (int)ElementalType.Monster; i++)
        {
            pieceListDic.Add((ElementalType)i, new List<SkillPiece>());

            ElementalType type = (ElementalType)i;

            elementalBtns[i].onClick.AddListener(() =>
            {
                SelectElemental(type);
            });
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

            PieceInfoUI pieceInfoUI = PoolManager.GetItem<PieceInfoUI>();

            //print(sp.cardBG + " " + sp.PieceName);

            pieceInfoUI.SetSkillIcon(sp.cardBG, sp.skillStroke);
            pieceInfoUI.transform.SetParent(pieceHolder);
            pieceInfoUI.transform.SetAsLastSibling();

            pieceInfoUI.button.onClick.AddListener(() =>
            {
                desUI.ShowDescription(sp);
            });
        }
    }
}
