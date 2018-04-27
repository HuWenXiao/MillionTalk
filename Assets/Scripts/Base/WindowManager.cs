using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 通知窗口的类型
/// </summary>
public enum MSGBOX_TYPE
{
    /// <summary>
    /// 确认窗口，只有一个 “确认” 按钮
    /// </summary>
    CONFIRM = 0,
    /// <summary>
    /// 询问窗口，有 “确定” 和 “取消” 两个按钮
    /// </summary>
    ENQUIRE = 1,
}

public class WindowManager : MonoBehaviour
{
    #region Singleton

    private static WindowManager s_instance;

    public static WindowManager instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new WindowManager();
            return s_instance;
        }
    }

    public WindowManager()
    {
        s_instance = this;
    }

    #endregion

    public delegate void SceneChanged();

    public SceneChanged OnSceneChanged;

    private Dictionary<string, string> windowDict = new Dictionary<string, string>
    {
        {"MsgboxWindow",            "Prefabs/MsgboxWindow"},
        {"ExtraTagsEditWindow",     "Prefabs/ExtraTagsWindow"},
        {"PictureEditWindow",       "Prefabs/PictureEditWindow"},
        {"ScriptBrowseWindow",      "Prefabs/ScriptBrowseWindow"},
        {"ScriptDetailWindow",      "Prefabs/ScriptDetailWindow"},
        {"ChapterSceneEditWindow",  "Prefabs/ChapterSceneEditWindow"},
        {"BkgrdBrowseWindow",       "Prefabs/BkgrdBrowseWindow"},
        {"InGameMenu",              "Prefabs/InGameMenu"},
        {"SettingWindow",           "Prefabs/SettingWindow"},
        {"FileManageWindow",        "Prefabs/FileManageWindow" }
    };

void Awake () {
		DontDestroyOnLoad(this);
	}

    public void CreateMsgBox(string msg, string head = null, MSGBOX_TYPE type = MSGBOX_TYPE.CONFIRM, UnityAction positiveAction = null, UnityAction cancelAction = null)
    {
        GameObject prefab = Resources.Load("Prefabs/MsgboxWindow") as GameObject;
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform.Find("WindowCanvas"));
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<MsgboxWindow>().Init(msg, head, type, positiveAction, cancelAction);
    }

    public void CreateWindow<T>() where T : WindowBase
    {
        GameObject prefab = Resources.Load(windowDict[typeof(T).ToString()]) as GameObject;
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform.Find("WindowCanvas"));
        go.transform.localPosition = Vector3.zero;
        if (go.GetComponent<T>() == null)
        {
            go.AddComponent<T>();
        }
    }

    public void CreateExtraTagsWindow(int index)
    {
        GameObject prefab = Resources.Load("Prefabs/ExtraTagsWindow") as GameObject;
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform.Find("WindowCanvas"));
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<ExtraTagsEditWindow>().Init(index);
    }

    public void CreateScriptDetailWindow(string name, string path)
    {
        GameObject prefab = Resources.Load("Prefabs/ScriptDetailWindow") as GameObject;
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform.Find("WindowCanvas"));
        go.transform.localPosition = Vector3.zero;
        go.GetComponent<ScriptDetailWindow>().Init(name, path);
    }

}
