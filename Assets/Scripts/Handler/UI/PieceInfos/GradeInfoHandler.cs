using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeInfoHandler : MonoBehaviour
{
    private List<Image> starImgs = new List<Image>();

    [Header("?? ?̹???")]
    public List<Sprite> starIcon = new List<Sprite>();

    private void Awake()
    {
        GetComponentsInChildren(starImgs);
    }

    public void SetGrade(GradeInfo grade)
    {
        for (int i = 0; i < starImgs.Count; i++)
        {
            starImgs[i].gameObject.SetActive(false);
            starImgs[i].sprite = grade == GradeInfo.Legend ? starIcon[0] : starIcon[1];
        }

        for (int i = 0; i < (int)grade; i++)
        {
            starImgs[i].gameObject.SetActive(true);
        }
    }
}
