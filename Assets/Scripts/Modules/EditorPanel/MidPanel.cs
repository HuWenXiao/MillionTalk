using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MidPanel : MonoBehaviour
{
    #region Singleton
    private static MidPanel s_instance;
    public static MidPanel instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new MidPanel();
            return s_instance;
        }
    }

    public MidPanel()
    {
        s_instance = this;
    }
    #endregion

    public delegate void SentenceNodeChanged(int i);
    public SentenceNodeChanged OnSentenceNodeChanged;
    public Button pictureButton;
    public Button prevButton;
    public Button nextButton;
    public Text idText;
    public Text leftpicText;
    public Text midpicText;
    public Text rightpicText;
    public InputField speakerInput;
    public Dropdown positionDrop;
    public InputField contentInput;

    /// <summary>
    /// start at 0
    /// </summary>
    public int _listIndex;

    void Awake()
    {
        pictureButton.onClick.AddListener(OnSetPic);
        prevButton.onClick.AddListener(OnSetPrev);
        nextButton.onClick.AddListener(OnSetNext);
        positionDrop.onValueChanged.AddListener(OnPositionDropChanged);
    }

    void Update()
    {
        //if (!string.IsNullOrEmpty(DialogData.instance.dialogList[_listIndex].speakerName))
        if (speakerInput.text != DialogData.instance.dialogList[_listIndex].speakerName)
            DialogData.instance.dialogList[_listIndex].speakerName = speakerInput.text;

        //if (!string.IsNullOrEmpty(DialogData.instance.dialogList[_listIndex].content))
        if (contentInput.text != DialogData.instance.dialogList[_listIndex].content)
            DialogData.instance.dialogList[_listIndex].content = contentInput.text;
    }

    void OnPositionDropChanged(int value)
    {
        switch (value)
        {
            case 0:
                DialogData.instance.dialogList[_listIndex].picPosition = 2.ToString();
                break;
            case 1:
                DialogData.instance.dialogList[_listIndex].picPosition = 0.ToString();
                break;
            case 2:
                DialogData.instance.dialogList[_listIndex].picPosition = (-1).ToString();
                break;
            case 3:
                DialogData.instance.dialogList[_listIndex].picPosition = 1.ToString();
                break;
            case 4:
                DialogData.instance.dialogList[_listIndex].picPosition = null;
                break;
        }
    }

    public void RefreshPanel(int i) //  i starts at 1
    {
        _listIndex = i - 1;
        Dialog dialog = DialogData.instance.dialogList[_listIndex];
        speakerInput.text = dialog.speakerName;
        contentInput.text = dialog.content;
        if (!string.IsNullOrEmpty(dialog.picPosition))
        {
            int position = int.Parse(dialog.picPosition);
            switch (position)
            {
                case 0:
                    positionDrop.value = 1; break;
                case -1:
                    positionDrop.value = 2; break;
                case 1:
                    positionDrop.value = 3; break;
                default:
                    positionDrop.value = 0; break;
            }
        }
        else positionDrop.value = 4;

        idText.text = "No. " + dialog.id + ": ";
        leftpicText.text = "Left:\t\t" + dialog.leftPic;
        midpicText.text = "Mid:\t\t"+dialog.midPic;
        rightpicText.text = "Right:\t"+ dialog.rightPic;

        OnSentenceNodeChanged(_listIndex+1);
    }

    private void OnSetPic()
    {
        WindowManager.instance.CreateWindow<PictureEditWindow>();
    }

    private void OnSetPrev()
    {
        if (_listIndex < 1)
            _listIndex = 1;
        LeftPanel.instance.SelectedSentenceById(_listIndex);
        LeftPanel.instance.contentTransform.transform.Translate(0, -0.458f, 0);
    }

    private void OnSetNext()
    {
        if (_listIndex >= DialogData.instance.dialogList.Count - 1)
            _listIndex = DialogData.instance.dialogList.Count - 2;

        LeftPanel.instance.SelectedSentenceById(_listIndex + 2);
        LeftPanel.instance.contentTransform.transform.Translate(0, 0.458f, 0);
    }
}
