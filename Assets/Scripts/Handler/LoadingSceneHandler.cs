using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneHandler : MonoBehaviour
{
    private static string nextScene;

    public Image progressBar;

    public static void LoadScene(string sceneName)
    {
        DG.Tweening.DOTween.CompleteAll();
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    public void Start()
    {
        StartCoroutine(LoadScene());
    }


    public IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount > 0.99f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
        }
        progressBar.fillAmount = 1f;
    }
}