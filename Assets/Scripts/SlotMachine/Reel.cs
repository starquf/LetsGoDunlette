using System.Collections;
using UnityEngine;

public class Reel : MonoBehaviour
{
    [Header("Option")]
    [SerializeField][Range(1, 10)] private float speed = 1;

    [Header("Reference")]
    [SerializeField] private RectTransform contentRectTrm;
    private int contentChildCount;
    private float contentChildHeight;

    private int tempCount;

    private void Start()
    {
        CacheData();
        StartCoroutine(RollReel(10));
    }

    private void CacheData()
    {

        contentChildCount = contentRectTrm.childCount;
        if (contentChildCount > 0)
        {
            contentChildHeight = contentRectTrm.GetChild(0).GetComponent<RectTransform>().rect.height;
        }

    }

    private IEnumerator RollReel(int count) // 릴을 돌립니다.
    {
        tempCount = 0;
        while (true)
        {
            contentRectTrm.anchoredPosition -= new Vector2(0, -500) * Time.deltaTime * speed;

            if (contentRectTrm.anchoredPosition.y >= ((contentChildCount - 1) * contentChildHeight))
            {
                contentRectTrm.anchoredPosition = new Vector2(0, 0);
                tempCount++;
            }

            if (tempCount >= count)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

}
