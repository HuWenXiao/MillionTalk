using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScriptBrowseWindow : WindowBase
{
    public RectTransform rectContent;
    public GameObject scriptCell;


    protected override void OnAwake()
    {
        base.OnAwake();

        WindowManager.instance.OnSceneChanged += Close;
        exitBtn.onClick.AddListener(()=> { WindowManager.instance.OnSceneChanged -= Close; });
        exitBtn.onClick.AddListener(Close);
        string path = "Resources/Scenarios";

        if (Directory.Exists(Application.dataPath +"/"+ path))
        {
            DirectoryInfo direction = new DirectoryInfo(Application.dataPath + "/" + path);
            FileInfo[] files = direction.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                    continue;

                string scriptName = Path.GetFileNameWithoutExtension(files[i].Name);

                GameObject ob = Instantiate(scriptCell);
                ob.transform.SetParent(rectContent);
                ob.transform.localPosition = Vector3.zero;
                ob.transform.localScale = Vector3.one;
                ob.GetComponent<ScriptBrowseItem>().Init(scriptName, files[i].LastWriteTime.ToString(), files[i].FullName);
            }
        }


        titleText.text = "Choose A Scenario";
    }
}