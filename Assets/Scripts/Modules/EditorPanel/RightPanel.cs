using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightPanel : MonoBehaviour
{
    #region Singleton
    private static RightPanel s_instance;
    public static RightPanel instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new RightPanel();
            return s_instance;
        }
    }

    public RightPanel()
    {
        s_instance = this;
    }
    #endregion

    public ScrollRect extraRect;
    public RectTransform extraTransform;
    public Button editButton;
    public GameObject extraListCell;
    public Image backgroundPreview;
    public Image leftpicPreview;
    public Image midpicPreview;
    public Image rightpicPreview;

    Color colorOnFocus = new Color(1, 1, 1, 1);
    Color colorOffFocus = new Color(0.5f, 0.5f, 0.5f, 1);
    private int _index;

    void Awake ()
    {
		editButton.onClick.AddListener(OnClickEdit);
	}

    public void RefreshPanel(int index) // start at 1
    {
        _index = index - 1;
        ClearList();
        Dialog dialog = DialogData.instance.dialogList[_index];

        Sprite backgroundSprite = new Sprite();
        backgroundSprite = Resources.Load("Pictures/Background/" + DialogData.instance.GetBackgroundName(), backgroundSprite.GetType()) as Sprite;
        if (backgroundSprite == null)
            Debug.LogError("背景图片读取失败");
        backgroundPreview.overrideSprite = backgroundSprite;

        Sprite sprite = new Sprite();

        if (dialog.leftPic == null)
            leftpicPreview.enabled = false;
        else if (dialog.leftPic != "")
        {
            sprite = Resources.Load(dialog.leftPic, sprite.GetType()) as Sprite;
            leftpicPreview.sprite = sprite;
            leftpicPreview.enabled = true;
        }

        if (dialog.midPic == null)
            midpicPreview.enabled = false;
        else if (dialog.midPic != "")
        {
            sprite = Resources.Load(dialog.midPic, sprite.GetType()) as Sprite;
            midpicPreview.sprite = sprite;
            midpicPreview.enabled = true;
        }

        if (dialog.rightPic == null)
            rightpicPreview.enabled = false;
        else if (dialog.rightPic != "")
        {
            sprite = Resources.Load(dialog.rightPic, sprite.GetType()) as Sprite;
            rightpicPreview.sprite = sprite;
            rightpicPreview.enabled = true;
        }
        if(!string.IsNullOrEmpty(dialog.picPosition)) 
        switch (int.Parse(dialog.picPosition))
        {
            case 0:     //中
                midpicPreview.color = colorOnFocus;
                leftpicPreview.color = colorOffFocus;
                rightpicPreview.color = colorOffFocus;
                break;
            case -1:    //左
                midpicPreview.color = colorOffFocus;
                leftpicPreview.color = colorOnFocus;
                rightpicPreview.color = colorOffFocus;
                break;
            case 1:     //右
                midpicPreview.color = colorOffFocus;
                leftpicPreview.color = colorOffFocus;
                rightpicPreview.color = colorOnFocus;
                break;
            default:
                midpicPreview.color = colorOffFocus;
                leftpicPreview.color = colorOffFocus;
                rightpicPreview.color = colorOffFocus;
                break;
        }

        if (!string.IsNullOrEmpty(dialog.endType))
        {
            GameObject ob = Instantiate(extraListCell);
            ob.transform.SetParent(extraTransform);
            ob.transform.localPosition = Vector3.zero;
            ob.transform.localScale = Vector3.one;
            string str;
            int endtype = int.Parse(dialog.endType);
            switch (endtype)
            {
                case (int)END_TYPE.SCENE_END:
                    str = string.Format("Scene End, go to No." + dialog.endValue);
                    break;

                case (int)END_TYPE.CHAPTER_END:
                    str = string.Format("Chapter End, go to No." + dialog.endValue);
                    break;

                case (int)END_TYPE.SENTENCE_JUMPTO:
                    str = string.Format("Jump to Sentence id=" + dialog.endValue);
                    break;

                case (int)END_TYPE.GAME_END:
                    str = string.Format("End of the game, go to Ending " + dialog.endValue);
                    break;

                default:
                    str = string.Format("End\tType = " + dialog.endType + ", Value = " + dialog.endValue);
                    break;
            }
            ob.GetComponentInChildren<Text>().text = str;
        }

        if (!string.IsNullOrEmpty(dialog.bgmPath))
        {
            GameObject ob = Instantiate(extraListCell);
            ob.transform.SetParent(extraTransform);
            ob.transform.localPosition = Vector3.zero;
            ob.transform.localScale = Vector3.one;
            string str = string.Format("Play BGM, volume {0}, {1} sec", dialog.bgmVolume, dialog.bgmValue);

            ob.GetComponentInChildren<Text>().text = str;
        }

    }


    public void ClearList()
    {
        if (extraTransform.childCount <= 0) return;

        foreach (var go in extraTransform.GetComponentsInChildren<Transform>())
        {
            if (go.gameObject != extraTransform.gameObject)
                Destroy(go.gameObject);
        }
    }

    public void OnClickEdit()
    {
        WindowManager.instance.CreateExtraTagsWindow(_index);
    }


}
