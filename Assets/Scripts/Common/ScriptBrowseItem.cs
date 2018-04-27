using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptBrowseItem : MonoBehaviour
{
    public Button detailBtn;
    public Text nameText;
    public Text dateText;
    public string scriptPath;

    void Awake()
    {
        detailBtn.onClick.AddListener(OnClickDetailBtn);
    }

    public void Init()
    {
        nameText.text = "";
        dateText.text = "";
    }

    public void Init(string name, string date, string path)
    {
        nameText.text = name;
        dateText.text = date;
        scriptPath = path;
    }

    public void OnClickDetailBtn()
    {
        WindowManager.instance.CreateScriptDetailWindow(nameText.text, scriptPath);
    }
}
