using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{

    public Text chapterInput;
    public Text sceneInput;

    private int _chapterId;
    private int _sceneId;

	void Awake ()
	{
	}

    public void OnClickScriptBrowse()
    {
        //WindowManager.instance.CreateScriptBrowseWindow();
        SetInput();
        GlobalManager.instance.LoadScene("DialogScene");
    }

    public void OnClickFile()
    {
        //    SetInput();
        //    GlobalManager.LoadScene("DialogScene");
        WindowManager.instance.CreateWindow<FileManageWindow>();
    }

    public void OnClickEdit()
    {
        SetInput();
        GlobalManager.instance.LoadScene("EditScene");
    }

    public void OnClickSetting()
    {
        WindowManager.instance.CreateWindow<SettingWindow>();
    }

    public void OnClickExit()
    {
        WindowManager.instance.CreateMsgBox("Do you really want to exit?", "Notice", MSGBOX_TYPE.ENQUIRE,
        () =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }

    private void SetInput()
    {
        if (string.IsNullOrEmpty(chapterInput.text)) _chapterId = 1;
        else _chapterId = int.Parse(chapterInput.text);

        if (string.IsNullOrEmpty(sceneInput.text)) _sceneId = 1;
        else _sceneId = int.Parse(sceneInput.text);

        if (_chapterId < 1) _chapterId = 1;
        if (_sceneId < 1) _sceneId = 1;

        GlobalManager.instance.chapterId = this._chapterId;
        GlobalManager.instance.sceneId = this._sceneId;
    }
}
