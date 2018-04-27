using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//WARNING : This file is full of bad designs. Need to be rewrite.

public class PictureEditWindow : WindowBase
{
    public Dropdown leftpicDrop;
    public Dropdown midpicDrop;
    public Dropdown rightpicDrop;
    public InputField leftpicInput;
    public InputField midpicInput;
    public InputField rightpicInput;
    public Button browseButton1;
    public Button browseButton2;
    public Button browseButton3;
    public Button copyButton1;
    public Button copyButton2;
    public Button copyButton3;

    private UnityAction onCloseAction;

    protected override void OnAwake()
    {
        base.OnAwake();

        //WindowManager.instance.OnSceneChanged += Close;
        //onCloseAction += ()=> { WindowManager.instance.OnSceneChanged -= Close; };
        onCloseAction += Save;
        onCloseAction += Close;
		exitBtn.onClick.AddListener(onCloseAction);

        copyButton1.onClick.AddListener(CopyLeftPicFromPrev);
        copyButton2.onClick.AddListener(CopyMidPicFromPrev);
	    copyButton3.onClick.AddListener(CopyRightPicFromPrev);

        titleText.text = "Picture Edit Window";
        Dialog dialog = DialogData.instance.dialogList[MidPanel.instance._listIndex];
        Debug.Log(MidPanel.instance._listIndex);
        if (!string.IsNullOrEmpty(dialog.leftPic))
        {
            //leftpicDrop.AddOptions(new List<string> { dialog.leftPic });
            leftpicInput.text = dialog.leftPic;
        }

        if (!string.IsNullOrEmpty(dialog.midPic))
        {
            midpicInput.text = dialog.midPic;
        }

        if (!string.IsNullOrEmpty(dialog.rightPic))
        {
            rightpicInput.text = dialog.rightPic;
        }

        leftpicDrop.ClearOptions();
        midpicDrop.ClearOptions();
        rightpicDrop.ClearOptions();

        leftpicDrop.AddOptions(EditorPanel.instance.picCacheList);
        midpicDrop.AddOptions(EditorPanel.instance.picCacheList);
        rightpicDrop.AddOptions(EditorPanel.instance.picCacheList);

        leftpicDrop.onValueChanged.AddListener(OnLeftDropChanged);
        midpicDrop.onValueChanged.AddListener(OnMidDropChanged);
        rightpicDrop.onValueChanged.AddListener(OnRightDropChanged);
    }

    private void Save()
    {
        if (leftpicInput.text == "" || leftpicInput.text == leftpicDrop.options[0].text)
            DialogData.instance.dialogList[MidPanel.instance._listIndex].leftPic = null;
        else
            DialogData.instance.dialogList[MidPanel.instance._listIndex].leftPic = leftpicInput.text;

        if (midpicInput.text == "" || midpicInput.text == midpicDrop.options[0].text)
            DialogData.instance.dialogList[MidPanel.instance._listIndex].midPic = null;
        else
            DialogData.instance.dialogList[MidPanel.instance._listIndex].midPic = midpicInput.text;

        if (rightpicInput.text == "" || rightpicInput.text == rightpicDrop.options[0].text)
            DialogData.instance.dialogList[MidPanel.instance._listIndex].rightPic = null;
        else
            DialogData.instance.dialogList[MidPanel.instance._listIndex].rightPic = rightpicInput.text;

        if (leftpicInput.text != "" && !EditorPanel.instance.picCacheList.Contains(leftpicInput.text))
            EditorPanel.instance.picCacheList.Add(leftpicInput.text);

        if (midpicInput.text != "" && !EditorPanel.instance.picCacheList.Contains(midpicInput.text))
            EditorPanel.instance.picCacheList.Add(midpicInput.text);

        if (rightpicInput.text != "" && !EditorPanel.instance.picCacheList.Contains(rightpicInput.text))
            EditorPanel.instance.picCacheList.Add(rightpicInput.text);

        MidPanel.instance.RefreshPanel(MidPanel.instance._listIndex+1);
    }

    void OnLeftDropChanged(int id)
    {
        leftpicInput.text = leftpicDrop.options[id].text;
    }

    void OnMidDropChanged(int id)
    {
        midpicInput.text = midpicDrop.options[id].text;
    }

    void OnRightDropChanged(int id)
    {
        rightpicInput.text = rightpicDrop.options[id].text;
    }

    private void BrowsePicture()
    {
        
    }

    private void CopyLeftPicFromPrev()
    {
        if (null != DialogData.instance.dialogList[MidPanel.instance._listIndex - 1].leftPic)
        {
            leftpicInput.text = DialogData.instance.dialogList[MidPanel.instance._listIndex - 1].leftPic;
        }
    }

    private void CopyMidPicFromPrev()
    {
        if (null != DialogData.instance.dialogList[MidPanel.instance._listIndex - 1].midPic)
        {
            midpicInput.text = DialogData.instance.dialogList[MidPanel.instance._listIndex - 1].midPic;
        }
    }

    private void CopyRightPicFromPrev()
    {
        if (null != DialogData.instance.dialogList[MidPanel.instance._listIndex - 1].rightPic)
        {
            rightpicInput.text = DialogData.instance.dialogList[MidPanel.instance._listIndex - 1].rightPic;
        }
    }

}
