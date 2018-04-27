using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour {

    public Image leftsidePic;
    public Image midsidePic;
    public Image rightsidePic;
    public Text speakerText;
    public Text contentText;
    public Image backgroundPic;
    public Image speakerBox;
    public Image contentBox;
    public Button nextDialogBtn;
    public Button menuBtn;
    public GameObject curtain;

    Color colorOnFocus = new Color(1, 1, 1, 1);
    Color colorOffFocus = new Color(0.5f, 0.5f, 0.5f, 1);

    //这一坨均是为了实现文字跳出不得已加入的全局变量，有待优化
    bool isTextPoping = false;
    StringBuilder textBuilder;
    string textBuffer;
    char[] contentChar;
    bool skipEnabled = false;
    float textPopSpeed = 0.07f;


    bool isAutoClickNext = true;
    float autoClickNextTime = 1.3f;

    int sentenceId = 1;

    void Awake()
    {
        nextDialogBtn.onClick.AddListener(OnClickNext);
        menuBtn.onClick.AddListener(OnClickMenu);
        speakerText.text = "";
        contentText.text = "";

        textBuilder = new StringBuilder();

        DialogData.instance.SetUpChapterScene(GlobalManager.instance.chapterId, GlobalManager.instance.sceneId);

        leftsidePic.enabled = false;
        midsidePic.enabled = false;
        rightsidePic.enabled = false;
        speakerBox.enabled = false;
        contentBox.enabled = false;
        menuBtn.enabled = false;

        CreateCurtain();
        SetBackground();
        if(DialogData.instance.bgmInfo.soundPath != "")
            AudioManager.instance.PlayBGM(DialogData.instance.bgmInfo);
        
    }

	void Start () {
	}
	
	void FixedUpdate () {
        //当没有对话文字输出时，使对话框渐隐；有文字输出时，显示对话框
        //if (speakContentTxt.text == "")
        //    dialogBox.GetComponent<CanvasGroup>().alpha = Max(dialogBox.GetComponent<CanvasGroup>().alpha - 0.9f * Time.deltaTime, 0);
        //else
        //    dialogBox.GetComponent<CanvasGroup>().alpha = Min(dialogBox.GetComponent<CanvasGroup>().alpha + 0.9f * Time.deltaTime, 1);

        //暂时关闭加速功能
        //if (Input.GetKey(KeyCode.LeftControl) && skipEnabled)
        //    OnClickNext();
	}

    void OnClickNext()
    {
        if (isTextPoping)
        {
            StopCoroutine("TextPop");
            contentText.text = textBuffer;
            isTextPoping = false;
        }
        else
        {
            Dialog dialog = DialogData.instance.dialogList[sentenceId-1];
            if (dialog.endType != null)
                EndSwitch(int.Parse(dialog.endType), int.Parse(dialog.endValue));
            else
            {
                //if (dialog.endType == "0") skipEnabled = true;
                //if (dialog.endType != "0") skipEnabled = false;
                OutputDialog(dialog);
                sentenceId++;
            }
        }
    }

    void OnClickMenu()
    {
        WindowManager.instance.CreateWindow<InGameMenu>();
    }

    //检测当前对话是否为该场景终点。如果是，则执行切换场景的必要操作
    void EndSwitch(int endType, int endValue)
    {
        switch (endType)
        {
            case (int)END_TYPE.SCENE_END:
                speakerText.text = "";
                contentText.text = "";
                leftsidePic.enabled = false;
                midsidePic.enabled = false;
                rightsidePic.enabled = false;
                leftsidePic.sprite = null;
                midsidePic.sprite = null;
                rightsidePic.sprite = null;
                speakerBox.enabled = false;
                contentBox.enabled = false;
                menuBtn.enabled = false;

                DialogData.instance.dialogList.Clear();
                DialogData.instance.SetSceneNode(endValue);
                DialogData.instance.LoadSentences();
                sentenceId = 1;

                StartCoroutine("CreateInterlude");
                break;

            case (int)END_TYPE.CHAPTER_END:
                DialogData.instance.SetChapterNode(endValue);
                DialogData.instance.SetSceneNode(1);
                DialogData.instance.LoadSentences();
                OutputDialog(DialogData.instance.dialogList[sentenceId - 1]);
                sentenceId = 1;
                break;

            case (int)END_TYPE.SENTENCE_JUMPTO:
                sentenceId = endValue;
                OnClickNext();
                break;

            case (int)END_TYPE.GAME_END:
                CreateCurtain(true, 0.3f);
                // send endValue to GlobalManager
                // load game_end scene
                break;
        }
    }

    //读取该dialog对象中的参数，并赋给画面上各实体以进行输出
    void OutputDialog(Dialog dialog)
    {
        speakerBox.enabled = false;
        contentBox.enabled = false;
        menuBtn.enabled = true;

        if (string.IsNullOrEmpty(dialog.speakerName))
        {
            speakerText.text = "";
        }
        else if (dialog.speakerName != "")
        {
            speakerText.text = dialog.speakerName;
            speakerBox.enabled = true;
        }

        if (dialog.content != null)
        {
            contentBox.enabled = true;
            textBuffer = dialog.content;
            char[] contentChar = dialog.content.ToCharArray();
            textBuilder = new StringBuilder();
            StartCoroutine("TextPop", contentChar);
        }
        else
        {
            dialog.content = "";
        }

        //ShowPic(dialog, leftsidePic);
        //ShowPic(dialog, midsidePic);
        //ShowPic(dialog, rightsidePic);

        Sprite sprite = new Sprite();

        if (dialog.leftPic == null)
            leftsidePic.enabled = false;
        else if (dialog.leftPic != "")
        {
            sprite = Resources.Load(dialog.leftPic, sprite.GetType()) as Sprite;
            leftsidePic.sprite = sprite;
            leftsidePic.enabled = true;
        }

        if (dialog.midPic == null)
            midsidePic.enabled = false;
        else if (dialog.midPic != "")
        {
            sprite = Resources.Load(dialog.midPic, sprite.GetType()) as Sprite;
            midsidePic.sprite = sprite;
            midsidePic.enabled = true;
        }

        if (dialog.rightPic == null)
            rightsidePic.enabled = false;
        else if (dialog.rightPic != "")
        {
            sprite = Resources.Load(dialog.rightPic, sprite.GetType()) as Sprite;
            rightsidePic.sprite = sprite;
            rightsidePic.enabled = true;
        }

        if(!string.IsNullOrEmpty(dialog.picPosition))
            switch(int.Parse(dialog.picPosition))
            {
                case 0:     //中
                        midsidePic.color = colorOnFocus;
                        leftsidePic.color = colorOffFocus;
                        rightsidePic.color = colorOffFocus;
                        break;
                case -1:    //左
                        midsidePic.color = colorOffFocus;
                        leftsidePic.color = colorOnFocus;
                        rightsidePic.color = colorOffFocus;
                        break;
                case 1:     //右
                        midsidePic.color = colorOffFocus;
                        leftsidePic.color = colorOffFocus;
                        rightsidePic.color = colorOnFocus;
                        break;
                default:
                        midsidePic.color = colorOffFocus;
                        leftsidePic.color = colorOffFocus;
                        rightsidePic.color = colorOffFocus;
                        break;
            }

        if(dialog.bgmPath != null)
        {
            if (dialog.bgmPath != "")
            {
                float volume = float.Parse(dialog.bgmVolume);
                float value = float.Parse(dialog.bgmValue);
                AudioManager.instance.PlayBGM(new SoundInfo(dialog.bgmPath, volume, value));
            }
            else AudioManager.instance.StopBGM();
        }

        if(!string.IsNullOrEmpty(dialog.soundPath))
        {
            float volume = float.Parse(dialog.soundVolume);
            AudioManager.instance.PlaySound(new SoundInfo(dialog.soundPath, volume));
        }
    }

    IEnumerator TextPop(char[] a)
    {
        isTextPoping = true;
        int lenth = a.Length;
        for (int i = 0; i != lenth; i++)
        {
            textBuilder.Append(a[i]);
            contentText.text = textBuilder.ToString();
            yield return new WaitForSeconds(textPopSpeed);
        }
        isTextPoping = false;

        yield return new WaitForSeconds(autoClickNextTime);
        if(isAutoClickNext)
            OnClickNext();
    }


    //创建一个幕布，用于实现从画面全黑到完全显示的效果
    GameObject CreateCurtain(bool isReverse = false, float speed = 0.25f)
    {
        GameObject curt = Instantiate(curtain) as GameObject;
        curt.GetComponent<Curtain>().isFadeReverse = isReverse;
        curt.GetComponent<Curtain>().fadeSpeed = speed;
        curt.transform.SetParent(GameObject.Find("Canvas").transform);
        Vector3 vec = GameObject.Find("Canvas").transform.position;
        Quaternion qua = new Quaternion(0, 0, 0, 0);
        curt.transform.SetPositionAndRotation(vec, qua);
        if (isReverse == false)
        {
            StartCoroutine("EnableSkip", 1 / speed);
        }
        
        return curt;
    }

    IEnumerator EnableSkip(float time)
    {
        yield return new WaitForSeconds(time);
        skipEnabled = true;
        OnClickNext();
    }

    //使用协程创建一个画面逐渐变黑，再逐渐显示的过场效果
    IEnumerator CreateInterlude()
    {
        GameObject cur = CreateCurtain(true, 0.3f);
        yield return new WaitForSeconds(4.0f);
        Destroy(cur);
        SetBackground();
        CreateCurtain(false, 0.3f);
    }

    //读取背景资源，设置当前场景的背景
    void SetBackground()
     {
        Sprite backgroundSprite = new Sprite();
        backgroundSprite = Resources.Load("Pictures/Background/" + DialogData.instance.GetBackgroundName(), backgroundSprite.GetType()) as Sprite;
        if (backgroundSprite == null)
            Debug.LogError("背景图片读取失败");
        backgroundPic.overrideSprite = backgroundSprite;
    }

}
