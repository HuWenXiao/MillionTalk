using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum END_TYPE
{
    SCENE_END = 1,
    CHAPTER_END = 2,
    SENTENCE_JUMPTO = 3,
    GAME_END = 100,
}

public class ScriptIntro
{
    public string strAuthor;
    public string strDate;
    public string strInfo;

    public ScriptIntro(string author, string date, string info)
    {
        strAuthor = author;
        strDate = date;
        strInfo = info;
    }
}

public class DialogData
{
    #region Singleton
    private static DialogData s_instance;
    public static DialogData instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new DialogData();
            return s_instance;
        }
    }
    #endregion

    public List<Dialog> dialogList = new List<Dialog>();
    public XmlDocument document = new XmlDocument();
    public XmlNode chapterNode;
    public XmlNode sceneNode;
    public XmlNode introNode;
    public string backgroundName;
    public SoundInfo bgmInfo;

    public DialogData()
    {
        s_instance = this;
        document = new XmlDocument();
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;//读取XML文件时忽视注释
        document.Load(XmlReader.Create(Application.dataPath + "\\Resources\\Scenarios\\story01.xml", settings));

        if (document == null)
            Debug.LogError("加载XML文件失败");

        dialogList = new List<Dialog>();
        bgmInfo = new SoundInfo();
    }

    public DialogData(string path)
    {
        s_instance = this;
        document = new XmlDocument();
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;//读取XML文件时忽视注释
        document.Load(XmlReader.Create(path, settings));

        if (document == null)
            Debug.LogError("加载XML文件失败");

        dialogList = new List<Dialog>();
    }

    public void Init()
    {
        LoadXMLFromPath("/data.xml");
        if (document == null) Debug.LogError("Load Error");
    }

    public void LoadXMLFromPath(string path)
    {
        XmlReaderSettings readerSetting = new XmlReaderSettings();
        readerSetting.IgnoreComments = true;
        document.Load(XmlReader.Create(Application.dataPath + path, readerSetting));
    }

    public void LoadSentences()
    {
        dialogList.Clear();

        XmlNode sentenceNode = sceneNode.FirstChild;
        backgroundName = LoadAttribute(sceneNode, "info", "bkgrdname");

        if ( LoadAttribute(sceneNode, "info", "musicpath") != null)
        {
            bgmInfo.soundPath = LoadAttribute(sceneNode, "info", "musicpath");

            if (LoadAttribute(sceneNode, "info", "musicvolume") != null && LoadAttribute(sceneNode, "info", "musicvolume") != "")
                bgmInfo.volume = float.Parse(LoadAttribute(sceneNode, "info", "musicvolume")); 
            else
                bgmInfo.volume = 0.5f;

            if (LoadAttribute(sceneNode, "info", "musicttl") != null && LoadAttribute(sceneNode, "info", "musicttl") != "")
                bgmInfo.ttl = float.Parse(LoadAttribute(sceneNode, "info", "musicttl")); 
            else
                bgmInfo.ttl = 0;
        }

        for (int i = 1; i < sceneNode.ChildNodes.Count; i++)
        {
            Dialog dialog = new Dialog();
            sentenceNode = sentenceNode.NextSibling;
            string str = (sentenceNode as XmlElement).GetAttribute("id");
            if (str != "") dialog.id = int.Parse(str);

            dialog.speakerName   = LoadAttribute(sentenceNode, "speaker",   "name");
            dialog.picPosition   = LoadAttribute(sentenceNode, "position",  "value");
            dialog.leftPic       = LoadAttribute(sentenceNode, "leftpic",   "name");
            dialog.midPic        = LoadAttribute(sentenceNode, "midpic",    "name");
            dialog.rightPic      = LoadAttribute(sentenceNode, "rightpic",  "name");
            dialog.content       = LoadAttribute(sentenceNode, "content",   "value");
            dialog.endType       = LoadAttribute(sentenceNode, "end",       "type");
            dialog.endValue      = LoadAttribute(sentenceNode, "end",       "value");
            dialog.bgmPath       = LoadAttribute(sentenceNode, "music",     "path");
            dialog.bgmVolume     = LoadAttribute(sentenceNode, "music",     "volume");
            dialog.bgmValue      = LoadAttribute(sentenceNode, "music",     "ttl");
            dialog.soundPath     = LoadAttribute(sentenceNode, "sound",     "path");
            dialog.soundVolume   = LoadAttribute(sentenceNode, "sound",     "volume");

            dialogList.Add(dialog);
        }
    }

    string LoadAttribute(XmlNode node, string nodeName, string attrName)
    {
        node = node.SelectSingleNode(nodeName);
        if (node == null)
            return null;

        string str = (node as XmlElement).GetAttribute(attrName);
        if (str != null)
            return str;

        return "";
    }

    public void SetUpChapterScene(int chapterId, int sceneId)
    {
        SetChapterNode(chapterId);
        SetSceneNode(sceneId);
        LoadSentences();
    }

    public void SetChapterNode(int num)
    {
        XmlNodeList nodeList = document.GetElementsByTagName("chapter");
        chapterNode = nodeList.Item(num - 1);
    }

    public void SetSceneNode(int num)
    {
        sceneNode = chapterNode.ChildNodes.Item(num - 1);
    }

    public int GetChapterId()
    {
        return int.Parse((chapterNode as XmlElement).GetAttribute("id"));
    }

    public int GetSceneId()
    {
        return int.Parse((sceneNode as XmlElement).GetAttribute("id"));
    }

    public string GetBackgroundName()
    {
        return backgroundName;
    }

    public void LoadSentencesInEditor()
    {
        LoadSentences();
        FormatListId();
        for (int i = 1; i < dialogList.Count; i++)
        {
            Dialog dialog = dialogList[i];
            Dialog prevDialog = dialogList[i - 1];

            if (dialog.id <= 0) continue;

            if (dialog.speakerName != null && prevDialog.speakerName != null && dialog.speakerName == "" && prevDialog.speakerName != "")
                dialog.speakerName = prevDialog.speakerName;

            if (dialog.content == "" && prevDialog.content != "")
                dialog.content = prevDialog.content;

            if (dialog.picPosition == "" && prevDialog.picPosition != "")
                dialog.picPosition = prevDialog.picPosition;

            if (dialog.leftPic == "" && prevDialog.leftPic != "")
                dialog.leftPic = prevDialog.leftPic;

            if (dialog.midPic == "" && prevDialog.midPic != "")
                dialog.midPic = prevDialog.midPic;

            if (dialog.rightPic == "" && prevDialog.rightPic != "")
                dialog.rightPic = prevDialog.rightPic;
        }
    }

    public void FormatListId()
    {
        for (int i = 0; i < dialogList.Count; i++)
        {
            dialogList[i].id = i + 1;
        }
    }

    public void SetIntroNode()
    {
        introNode = document.GetElementsByTagName("intro").Item(0);
    }

    public ScriptIntro GetScriptIntro()
    {
        if (introNode == null) return null;

        XmlElement ele = (XmlElement)introNode;
        ScriptIntro intro = new ScriptIntro
        (
            ele.GetAttribute("author"),
            ele.GetAttribute("date"),
            ele.GetAttribute("info")
        );

        return intro;
    }

    public void LoadXMLFromFullPath(string path)
    {
        XmlReaderSettings readerSetting = new XmlReaderSettings();
        readerSetting.IgnoreComments = true;
        document.Load(XmlReader.Create(path, readerSetting));

        if (document == null) Debug.LogError("Load Error");
    }
}