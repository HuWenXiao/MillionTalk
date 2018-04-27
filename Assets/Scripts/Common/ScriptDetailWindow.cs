using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptDetailWindow : MsgboxWindow
{
    public Text nameText;
    public Text authorText;

    private ScriptIntro scriptIntro;

    void Awake()
    {
        WindowManager.instance.OnSceneChanged += Close;
        exitBtn.onClick.AddListener(() => { WindowManager.instance.OnSceneChanged -= Close; });
        exitBtn.onClick.AddListener(Close);
        confirmBtn.onClick.AddListener(Load);
    }

    public void Init(string name, string path)
    {
        DialogData.instance.LoadXMLFromFullPath(path);
        DialogData.instance.SetIntroNode();
        scriptIntro = DialogData.instance.GetScriptIntro();
        nameText.text = name;
        if (scriptIntro == null)
        {
            authorText.text = "Author Unknown";
            msgText.text = "No Introduction...";
        }
        else
        {
            authorText.text = scriptIntro.strAuthor + "\t\t\t" + scriptIntro.strDate;
            msgText.text = scriptIntro.strInfo;
        }
    }

    private void Load()
    {
        GlobalManager.instance.LoadScene("DialogScene");
    }
	
}
