using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EditorPanel : MonoBehaviour
{
    #region Singleton
    private static EditorPanel s_instance;
    public static EditorPanel instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new EditorPanel();
            return s_instance;
        }
    }

    public EditorPanel()
    {
        s_instance = this;
    }
    #endregion

    private string filePath = "\\Resources\\Scenarios\\story01.xml";
    public List<string> picCacheList; 

    void Awake()
    {
        picCacheList = new List<string>(); 
        picCacheList.Add("Empty"); // todo : 对"Empty"的多语言化？
        TopPanel.instance.OnChapterSceneChanged += LeftPanel.instance.ReSetSentencesPanel;
        LeftPanel.instance.OnCurrentSentenceChanged += MidPanel.instance.RefreshPanel;
        MidPanel.instance.OnSentenceNodeChanged += RightPanel.instance.RefreshPanel;

        DialogData.instance.LoadXMLFromPath(filePath);
        DialogData.instance.SetUpChapterScene(1,1);

    }

    void Start()
    {
        TopPanel.instance.InitDrop();

    }

    public void OnSave()
    {
        WindowManager.instance.CreateMsgBox("Save all the changes you made at sentences list? \nNote that this action can not be undone.",
            "Save", MSGBOX_TYPE.ENQUIRE, SaveDialogList);
    }

    private void SaveDialogList()
    {
        // todo : Format DialogList id

        int id = int.Parse((DialogData.instance.sceneNode as XmlElement).GetAttribute("id"));
        DialogData.instance.sceneNode.RemoveAll();
        (DialogData.instance.sceneNode as XmlElement).SetAttribute("id", id.ToString());
        XmlElement infoEle = DialogData.instance.document.CreateElement("info", null);
        infoEle.SetAttribute("bkgrdname", TopPanel.instance.backgroundText.text);
        DialogData.instance.sceneNode.AppendChild(infoEle);

        for (int i = 0; i < DialogData.instance.dialogList.Count; i++)
        {
            Dialog dialog = DialogData.instance.dialogList[i];
            XmlElement sentenceEle = DialogData.instance.document.CreateElement("sentence", null);
            sentenceEle.SetAttribute("id", dialog.id.ToString());

            if (!string.IsNullOrEmpty(dialog.speakerName))
            {
                sentenceEle.AppendChild(CreateChild("speaker", new[] { new AttributeInfo("name", dialog.speakerName) }));
            }

            if (!string.IsNullOrEmpty(dialog.leftPic))
            {
                sentenceEle.AppendChild(CreateChild("leftpic", new[] { new AttributeInfo("name", dialog.leftPic) }));
            }

            if (!string.IsNullOrEmpty(dialog.midPic))
            {
                sentenceEle.AppendChild(CreateChild("midpic", new[] { new AttributeInfo("name", dialog.midPic) }));
            }

            if (!string.IsNullOrEmpty(dialog.rightPic))
            {
                sentenceEle.AppendChild(CreateChild("rightpic", new[] { new AttributeInfo("name", dialog.rightPic) }));
            }

            if (!string.IsNullOrEmpty(dialog.picPosition))
            {
                sentenceEle.AppendChild(CreateChild("position", new[] { new AttributeInfo("value", dialog.picPosition) }));
            }

            if (!string.IsNullOrEmpty(dialog.content))
            {
                sentenceEle.AppendChild(CreateChild("content", new[] { new AttributeInfo("value", dialog.content) }));
            }

            if (!string.IsNullOrEmpty(dialog.endType))
            {
                sentenceEle.AppendChild(CreateChild("end", new[] {
                    new AttributeInfo("type", dialog.endType),
                    new AttributeInfo("value", dialog.endValue)
                }));
            }

            if (!string.IsNullOrEmpty(dialog.bgmPath))
            {
                sentenceEle.AppendChild(CreateChild("music", new[] {
                    new AttributeInfo("path", dialog.bgmPath),
                    new AttributeInfo("volume", dialog.bgmVolume),
                    new AttributeInfo("ttl", dialog.bgmValue)
                }));
            }

            DialogData.instance.sceneNode.AppendChild(sentenceEle);
        }

        // todo : format DialogList content

        DialogData.instance.document.Save(Application.dataPath + filePath);
    }

    /// <summary>
    /// Create and return a XmlElement
    /// </summary>
    /// <param name="childName">LocalName of this XmlElement</param>
    /// <param name="infos"> An array of AttributeInfo about this element's attributes</param>
    /// <returns></returns>
    private XmlElement CreateChild(string childName, AttributeInfo[] infos)
    {
        XmlElement ele = DialogData.instance.document.CreateElement(childName, null);
        for (int i = 0; i < infos.Length; i++)
        {
            ele.SetAttribute(infos[i].attributeName, infos[i].attributeValue);
        }
        return ele;
    }


    class AttributeInfo
    {
        public string attributeName;
        public string attributeValue;

        public AttributeInfo(string name, string value)
        {
            attributeName = name;
            attributeValue = value;
        }
    }
}
