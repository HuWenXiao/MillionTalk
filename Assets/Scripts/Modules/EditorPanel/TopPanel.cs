using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    #region Singleton
    private static TopPanel s_instance;
    public static TopPanel instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new TopPanel();
            return s_instance;
        }
    }

    public TopPanel()
    {
        s_instance = this;
    }
    #endregion

    public Dropdown chapterDrop;
    public Dropdown sceneDrop;
    public Button nodeBtn;
    public Button backgroundBtn;
    public Button saveButton;
    public Button exitButton;
    public Text backgroundText;
    public delegate void ChapterSceneChanged();
    public ChapterSceneChanged OnChapterSceneChanged;

    private void Awake()
    {
        chapterDrop.onValueChanged.AddListener( x => { SetChapterDrop(); });
        sceneDrop.onValueChanged.AddListener( x=> { SetSceneDrop(); });
        backgroundBtn.onClick.AddListener(OnSetBackground);
        saveButton.onClick.AddListener(EditorPanel.instance.OnSave);
        exitButton.onClick.AddListener(OnClickExit);
        nodeBtn.onClick.AddListener(OnClickNode);
    }

    public void InitDrop()
    {
        XmlNodeList nodeList = DialogData.instance.document.GetElementsByTagName("chapter");
        List<Dropdown.OptionData> dropList = new List<Dropdown.OptionData>();
        if (nodeList.Count < 1)
        {
            Debug.LogError("Init ChapterNode Error!");
            return;
        }

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            string str = ele.GetAttribute("id");
            if (str != "")
            {
                dropList.Add(new Dropdown.OptionData(str));
                chapterDrop.options = dropList;
            }
        }
        DialogData.instance.SetSceneNode(1);
        InitSceneDrop();
    }

    public void SetChapterDrop()
    {
        DialogData.instance.SetSceneNode(chapterDrop.value + 1);

        InitSceneDrop();
    }

    public void InitSceneDrop()
    {
        XmlNodeList nodeList = DialogData.instance.chapterNode.ChildNodes;
        List<Dropdown.OptionData> dropList = new List<Dropdown.OptionData>();
        if (nodeList.Count < 1)
        {
            Debug.LogError("Init SceneNode Error!");
            return;
        }

        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            string str = ele.GetAttribute("id");
            if (str != "")
            {
                dropList.Add(new Dropdown.OptionData(str));
                sceneDrop.options = dropList;
            }
        }

        SetSceneDrop();
    }

    public void SetSceneDrop()
    {
        DialogData.instance.SetSceneNode(sceneDrop.value+1);

        OnChapterSceneChanged();
        backgroundText.text = DialogData.instance.backgroundName;
    }

    public void OnSetBackground()
    {
        WindowManager.instance.CreateWindow<BkgrdBrowseWindow>();

    }

    public void OnClickExit()
    {
        EditorPanel.instance.picCacheList.Clear();
        // todo : check save
        GlobalManager.instance.LoadScene("MainScene");
    }

    public void OnClickNode()
    {
        WindowManager.instance.CreateWindow<ChapterSceneEditWindow>();
    }
}
