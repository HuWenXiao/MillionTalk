using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class BkgrdBrowseWindow : WindowBase
{
    public GameObject bkgrdBrowseCell;
    public RectTransform contentTrans;

    protected override void OnAwake()
    {
        base.OnAwake();
        exitBtn.enabled = true;
        exitBtn.onClick.AddListener(Close);
        string path = "Resources/Pictures/Background";

        if (Directory.Exists(Application.dataPath + "/" + path))
        {
            DirectoryInfo direction = new DirectoryInfo(Application.dataPath + "/" + path);
            FileInfo[] files = direction.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".png") || files[i].Name.EndsWith(".jpg") || files[i].Name.EndsWith(".jpeg"))
                {
                    GameObject go = Instantiate(bkgrdBrowseCell);
                    go.transform.SetParent(contentTrans);
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localScale = Vector3.one;
                    go.GetComponent<BkgrdBrowseItem>().Init(files[i].Name, files[i].FullName);
                    go.GetComponent<BkgrdBrowseItem>().OnSetBkgrdItem += SetBkgrd;
                }
            }
        }

        titleText.text = "Choose A Background";
    }


    void SetBkgrd(string bkgrdName)
    {
        XmlElement infoEle = (XmlElement)DialogData.instance.sceneNode.ChildNodes[0];
        TopPanel.instance.backgroundText.text = bkgrdName;
        infoEle.SetAttribute("bkgrdname", bkgrdName);
        TopPanel.instance.OnChapterSceneChanged();

        Close();
    }
}
