using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class GeneralTools {

    private static GeneralTools s_instance;
    public static GeneralTools instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new GeneralTools();
            return s_instance;
        }
    }

    public GeneralTools()
    {
        s_instance = this;
    }

    public void RunGame()
    {
        if (!EditorApplication.isPlaying)
            EditorSceneManager.OpenScene("Assets/Scenes/StartScene.unity");
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    public void PauseGame()
    {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }
}
