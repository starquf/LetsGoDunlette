using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UILevelUPPopUp : MonoBehaviour
{
    public Image expFillImage;
    public Image AdditiveExpFillImage;

    public Text levelText;
    public Text expText;

    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        PopUp(1, 90, 10, 50, 100);
    }

    public void PopUp(int originLevel, int originExp, int nowLevel, int nowExp, int maxExp, bool isFirst = true)
    {
        StartCoroutine(StartPopUp(originLevel, originExp, nowLevel, nowExp, maxExp, isFirst));
    }

    public IEnumerator StartPopUp(int originLevel, int originExp, int nowLevel, int nowExp, int maxExp, bool isFirst = true) //exp가 고정이 아니게 되면 변경해야함
    {
        float time = 0.7f;
        expFillImage.color = Color.white;
        expText.color = Color.white;
        levelText.text = $"{originLevel}";
        expText.text = $"{originExp}/{maxExp}";

        float tempRatio = originExp / (float)maxExp;

        expFillImage.fillAmount = 0;
        AdditiveExpFillImage.fillAmount = 0;
        if (isFirst)
        {
            canvasGroup.DOFade(1f, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, tempRatio, time);
            yield return new WaitForSeconds(time);
        }

        if (nowLevel > originLevel)
        {
            DOTween.To(() => AdditiveExpFillImage.fillAmount, x => AdditiveExpFillImage.fillAmount = x, 1, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, 1, time);
            expText.DOText($"{maxExp}/{maxExp}", time);
            yield return new WaitForSeconds(time);
            expFillImage.DOColor(new Color(0, 0.6787322f, 1), time/2);
            expText.DOColor(new Color(0, 0.6787322f, 1), time / 2);
            yield return new WaitForSeconds(time);
            PopUp(originLevel + 1, 0, nowLevel, nowExp, maxExp, false);
        }
        else
        {
            tempRatio = nowExp / (float)maxExp;

            DOTween.To(() => AdditiveExpFillImage.fillAmount, x => AdditiveExpFillImage.fillAmount = x, tempRatio, time);
            yield return new WaitForSeconds(time);
            DOTween.To(() => expFillImage.fillAmount, x => expFillImage.fillAmount = x, tempRatio, time);
            expText.DOText($"{nowExp}/{maxExp}", time);
        }

        yield return null;
    }
}
