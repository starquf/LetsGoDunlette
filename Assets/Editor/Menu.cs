using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [MenuItem("Scene/EnemySpanwer")]
    static void EnemySpanwerScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/EnemySpanwer.unity");
    }

    [MenuItem("Scene/Seunghwan")]
    static void SeunghwanScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/SeungHwanScene.unity");
    }

    [MenuItem("Scene/Seunghyeok")]
    static void SeunghyeokScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/Seunghyeok.unity");
    }

    [MenuItem("Scene/Kyeounghyeok")]
    static void KyeounghyeokScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/KyeounghyeokScene.unity");
    }

    [MenuItem("Scene/SunHo")]
    static void SunHoScene()
    {
        if (EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }
        EditorSceneManager.OpenScene("Assets/Scenes/SunHo.unity");
    }

    [MenuItem("Scene/CopySeunghwan")]
    static void CopySeunghwan()
    {
        string path = Application.dataPath;
        string fileName = path + "/Scenes/SeungHwanScene.unity";
        string destFileName = path + "/Scenes/Seunghyeok.unity";


        System.IO.File.Copy(fileName, destFileName, true);
    }
}
