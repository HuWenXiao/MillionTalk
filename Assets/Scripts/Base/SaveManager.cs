using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Singleton
    private static SaveManager s_instance;
    public static SaveManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new SaveManager();
            return s_instance;
        }
    }

    public SaveManager()
    {
        s_instance = this;
    }
    #endregion

    public SaveFileInfo saveFileCache;

    void Awake()
    {
        saveFileCache = new SaveFileInfo();
        DontDestroyOnLoad(this);
    }

    public void Save(SaveFileInfo file)
    {
        saveFileCache = file;
    }

    public void Load(SaveFileInfo file)
    {
        saveFileCache = file;
    }

    public void AddFlag(int flag, int value)
    {
        saveFileCache.flagDict.Add(flag, value);
    }

    public void ClearSaveFileCache()
    {
        saveFileCache = new SaveFileInfo();
    }

    public void CreateSaveFileCahce()
    {
        

    }
}

public class SaveFileInfo
{
    public string playerName;
    public string bkgrdName;
    public string scenarioName;
    public string chapterId;
    public string sceneId;
    public string sentenceId;
    public Dictionary<int, int> flagDict = new Dictionary<int, int>(); 

    public SaveFileInfo() { }

    public SaveFileInfo(string player, string bkgrd, string scenario, string chapter, string scene, string sentence)
    {
        playerName = player;
        bkgrdName = bkgrd;
        scenarioName = scenario;
        chapterId = chapter;
        sceneId = scene;
        sentenceId = sentence;
    }

}
