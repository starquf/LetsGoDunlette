using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [MenuItem("Scene/EnemySpanwer")]
    private static void EnemySpanwerScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/EnemySpanwer.unity");
    }

    [MenuItem("Scene/Seunghwan")]
    private static void SeunghwanScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/SeungHwanScene.unity");
    }

    [MenuItem("Scene/Seunghyeok")]
    private static void SeunghyeokScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/Seunghyeok.unity");
    }

    [MenuItem("Scene/Kyeounghyeok")]
    private static void KyeounghyeokScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/KyeounghyeokScene.unity");
    }

    [MenuItem("Scene/SunHo")]
    private static void SunHoScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/SunHo.unity");
    }

    [MenuItem("Scene/CopySeunghwan")]
    private static void CopySeunghwan()
    {
        string path = Application.dataPath;
        string fileName = path + "/Scenes/SeungHwanScene.unity";
        string destFileName = path + "/Scenes/Seunghyeok.unity";


        System.IO.File.Copy(fileName, destFileName, true);
    }
}
