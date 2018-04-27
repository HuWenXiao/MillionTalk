using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : WindowBase
{
    public Button returnGameBtn;
    public Button fastSaveBtn;
    public Button fileManageBtn;
    public Button settingBtn;
    public Button exitGameBtn;

    protected override void OnAwake()
    {
        base.OnAwake();

        Time.timeScale = 0;
        if (AudioManager.instance.bgmPlayer.isPlaying)
        {
            AudioManager.instance.bgmPlayer.Pause();
            returnGameBtn.onClick.AddListener(() =>
            {
                AudioManager.instance.bgmPlayer.UnPause();
            });
        }
		
        returnGameBtn.onClick.AddListener(OnClickReturn);
        fastSaveBtn.onClick.AddListener(OnClickFastSave);
        fileManageBtn.onClick.AddListener(OnClickFileManage);
        settingBtn.onClick.AddListener(OnClickSetting);
        exitGameBtn.onClick.AddListener(OnClickExit);
	}

    void OnClickReturn()
    {
        Time.timeScale = 1;
        Close();
    }

    void OnClickFastSave()
    {
        SaveManager.instance.CreateSaveFileCahce();
    }

    void OnClickFileManage()
    {
        WindowManager.instance.CreateWindow<FileManageWindow>();
    }

    void OnClickSetting()
    {
        WindowManager.instance.CreateWindow<SettingWindow>();
    }

    void OnClickExit()
    {
        WindowManager.instance.CreateMsgBox("Do you really want to exit? All the unsaved progress will be lost", "Return To Main Menu", MSGBOX_TYPE.ENQUIRE,
            () =>
            {
                Time.timeScale = 1;
                AudioManager.instance.bgmPlayer.clip = null;
                Close();
                GlobalManager.instance.LoadScene("MainScene");
            });
    }
}
