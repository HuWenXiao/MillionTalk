using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    #region Singleton
    private static GlobalManager s_instance;
    public static GlobalManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new GlobalManager();
            return s_instance;
        }
    }

    public GlobalManager()
    {
        s_instance = this;
    }
    #endregion

    public int chapterId = 1;
    public int sceneId = 1;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string path)
    {
        if (WindowManager.instance.OnSceneChanged != null)
            WindowManager.instance.OnSceneChanged();
        SceneManager.LoadScene(path);
    }

}
