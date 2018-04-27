using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanel : MonoBehaviour
{
    #region Singleton
    private static LeftPanel s_instance;
    public static LeftPanel instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new LeftPanel();
            return s_instance;
        }
    }

    public LeftPanel()
    {
        s_instance = this;
    }
    #endregion

    public ScrollRect sentenceRect;
    public RectTransform contentTransform;
    public Button addButton;
    public Button deleteButton;
    public GameObject sentenceListCell;
    public delegate void SentencesPanelChanged(int index);
    public delegate void SelectSentence(int index);
    public SentencesPanelChanged OnCurrentSentenceChanged;

    private int _sentenceId;
    private int _prevId = -1;
    private List<SentenceListItem> _itemList; 

    void Awake ()
    {
        addButton.onClick.AddListener(AddSentence);
        deleteButton.onClick.AddListener(DeleteSentence);
        _itemList = new List<SentenceListItem>();
    }

    public void ReSetSentencesPanel()
    {
        _prevId = -1;
        DialogData.instance.LoadSentencesInEditor();
        RefreshSentencesPanel();
        SelectedSentenceById(1);
    }

    /// <summary>
    /// 按Id选择对应句子节点，Id的初始值为1
    /// </summary>
    /// <param name="id">句子Id，初始值为1</param>
    public void SelectedSentenceById(int id) // start at 1
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            _itemList[i].InitByDialog(DialogData.instance.dialogList[i]);
        }

        _sentenceId = id;
        OnCurrentSentenceChanged(_sentenceId);
        SetFocus();
    }

    public void AddSentence()
    {
        DialogData.instance.dialogList.Insert(_sentenceId, new Dialog());
        DialogData.instance.FormatListId();
        RefreshSentencesPanel();
        SelectedSentenceById(_sentenceId + 1);

    }

    public void DeleteSentence()
    {
        DialogData.instance.dialogList.RemoveAt(_sentenceId - 1);
        DialogData.instance.FormatListId();
        RefreshSentencesPanel();
        if (_sentenceId == DialogData.instance.dialogList.Count + 1)
        {
            _sentenceId--;
            _prevId--;
        }
        SelectedSentenceById(_sentenceId);
    }

    private void RefreshSentencesPanel()
    {
        if (contentTransform.childCount > 0)
        {
            _itemList.Clear();
            foreach (var go in contentTransform.GetComponentsInChildren<Transform>())
            {
                if (go.gameObject != contentTransform.gameObject)
                    Destroy(go.gameObject);
            }
        }

        for (int i = 0; i < DialogData.instance.dialogList.Count; i++)
        {
            GameObject ob = Instantiate(sentenceListCell);
            ob.transform.SetParent(contentTransform);
            ob.transform.localPosition = Vector3.zero;
            ob.transform.localScale = Vector3.one;
            ob.GetComponent<SentenceListItem>().InitByDialog(DialogData.instance.dialogList[i]);
            ob.GetComponent<SentenceListItem>().OnSelectSentence += SelectedSentenceById;
            _itemList.Add(ob.GetComponent<SentenceListItem>());
        }

        RefreshPirCache();
    }

    private void RefreshPirCache()
    {
        for (int i = 0; i < DialogData.instance.dialogList.Count; i++)
        {
            Dialog dialog = DialogData.instance.dialogList[i];
            if (!string.IsNullOrEmpty(dialog.leftPic))
            {
                string name = dialog.leftPic;
                if(!EditorPanel.instance.picCacheList.Contains(name))
                    EditorPanel.instance.picCacheList.Add(name);
            }
            if (!string.IsNullOrEmpty(dialog.midPic))
            {
                string name = dialog.midPic;
                if (!EditorPanel.instance.picCacheList.Contains(name))
                    EditorPanel.instance.picCacheList.Add(name);
            }
            if (!string.IsNullOrEmpty(dialog.rightPic))
            {
                string name = dialog.rightPic;
                if (!EditorPanel.instance.picCacheList.Contains(name))
                    EditorPanel.instance.picCacheList.Add(name);
            }
        }
    }

    private void SetFocus()
    {
        _itemList[_sentenceId - 1].OnFocus();
        if (_prevId != -1 && _prevId != _sentenceId)
        {
            _itemList[_prevId - 1].OffFocus();
        }
        _prevId = _sentenceId;
    }

}
