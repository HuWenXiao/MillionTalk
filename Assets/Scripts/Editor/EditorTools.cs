using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class EditorTools {

	[MenuItem("Excel's Tools/Run _F5", false, 001)]
    public static void RunGame()
    {
        GeneralTools.instance.RunGame();
    }

    [MenuItem("Excel's Tools/Pause _F6", false, 002)]
    public static void PauseGame()
    {
        GeneralTools.instance.PauseGame();
    }

    [MenuItem("Excel's Tools/Load Scene/Main &1", false, 100)]
    public static void LoadMainScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainScene.unity");
    }

    [MenuItem("Excel's Tools/Load Scene/Dialog &2", false, 101)]
    public static void LoadDialogScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/DialogScene.unity");
    }

    [MenuItem("Excel's Tools/Load Scene/Edit &3", false, 102)]
    public static void LoadEditScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/EditScene.unity");
    }

    [MenuItem("Excel's Tools/Load Scene/Start &4", false, 103)]
    public static void LoadStartScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
    }
}
