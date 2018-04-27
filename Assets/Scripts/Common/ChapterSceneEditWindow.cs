using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSceneEditWindow : WindowBase
{
    public Button chapterAddBtn;
    public Button chapterDeleteBtn;
    public Button sceneAddBtn;
    public Button sceneDeleteBtn;
    public RectTransform chapterTrans;
    public RectTransform sceneTrans;
    public GameObject chaptersceneCell;

    protected override void OnAwake()
    {
        base.OnAwake();

	    exitBtn.enabled = true;
		chapterAddBtn.onClick.AddListener(OnChapterAdd);
        chapterDeleteBtn.onClick.AddListener(OnChapterDelete);
        sceneAddBtn.onClick.AddListener(OnSceneAdd);
        sceneDeleteBtn.onClick.AddListener(OnSceneDelete);
        exitBtn.onClick.AddListener(OnClickClose);

        if (DialogData.instance.chapterNode == null)
            DialogData.instance.SetChapterNode(1);

        XmlNodeList list = DialogData.instance.document.GetElementsByTagName("chapter");
        foreach (var node in list)
        {
            GameObject ob = Instantiate(chaptersceneCell);
            ob.transform.SetParent(chapterTrans);
            ob.transform.localPosition = Vector3.zero;
            ob.transform.localScale = Vector3.one;
            string strId = ((XmlElement)node).GetAttribute("id");
            if (string.IsNullOrEmpty(strId))
                strId = "1";
            ob.GetComponent<ChapterSceneItem>().Init(int.Parse(strId));
            ob.GetComponent<ChapterSceneItem>().isChapter = true;
            ob.GetComponent<ChapterSceneItem>().OnToggleSelected += RefreshSceneRect;
            ob.GetComponentInChildren<Toggle>().group = chapterTrans.GetComponent<ToggleGroup>();
        }

        RefreshSceneRect();
	}

    new void OnClickClose()
    {
        Close();
    }

    void OnChapterAdd()
    {
        
    }

    void OnChapterDelete()
    {
        WindowManager.instance.CreateMsgBox("Are you sure you want to delete? \nAll the progresses will be lost.", "Notice", MSGBOX_TYPE.ENQUIRE);
    }

    void DeleteChapter()
    {
        
    }

    void OnSceneAdd()
    {
        
    }

    void OnSceneDelete()
    {
        WindowManager.instance.CreateMsgBox("Are you sure you want to delete? \nAll the progresses will be lost.", "Notice", MSGBOX_TYPE.ENQUIRE);
    }

    void RefreshSceneRect()
    {
        if(DialogData.instance.sceneNode == null)
            DialogData.instance.SetSceneNode(1);

        XmlNodeList list = DialogData.instance.chapterNode.ChildNodes;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject ob = Instantiate(chaptersceneCell);
            ob.transform.SetParent(sceneTrans);
            ob.transform.localPosition = Vector3.zero;
            ob.transform.localScale = Vector3.one;
            string strId = ((XmlElement)list[i]).GetAttribute("id");
            if (string.IsNullOrEmpty(strId))
                strId = "1";
            ob.GetComponent<ChapterSceneItem>().Init(int.Parse(strId));
            ob.GetComponentInChildren<Toggle>().group = sceneTrans.GetComponent<ToggleGroup>();
        }
    }
}
