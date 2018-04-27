using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExtraTagsEditWindow : MsgboxWindow
{
    public Dropdown endDrop;
    public InputField endInput;
    public Toggle musicStopToggle;
    public InputField musicVolumeInput;
    public InputField musicPathInput;
    public InputField musicTimeInput;

    UnityAction exitAction;
    private int _index;

    void Awake()
    {
        //WindowManager.instance.OnSceneChanged += Close;
        exitBtn.enabled = true;
        exitAction += SaveTags;
        exitAction += Close;
        //exitBtn.onClick.AddListener(() => { WindowManager.instance.OnSceneChanged -= Close; });
        exitBtn.onClick.AddListener(exitAction);
    }

    public void Init(int index)
    {
        _index = index;
        Dialog dialog = DialogData.instance.dialogList[_index];

        if (!string.IsNullOrEmpty(dialog.endType))
        {
            int endtype = int.Parse(dialog.endType);
            endInput.text = dialog.endValue;
            switch (endtype)
            {
                case (int) END_TYPE.SCENE_END:
                    endDrop.value = 2;
                    break;

                case (int) END_TYPE.CHAPTER_END:
                    endDrop.value = 1;
                    break;

                case (int) END_TYPE.SENTENCE_JUMPTO:
                    endDrop.value = 3;
                    break;

                case (int) END_TYPE.GAME_END:
                    endDrop.value = 4;
                    break;
            }
        }
        else endDrop.value = 0;

        if (!string.IsNullOrEmpty(dialog.bgmPath))
        {
            musicPathInput.text = dialog.bgmPath;
            // todo : deal with situation of an empty "music" node

            musicTimeInput.text = dialog.bgmValue;
            musicVolumeInput.text = dialog.bgmVolume;
        }

    }



    public void SaveTags()
    {
        // todo : edge check

        Dialog dialog = DialogData.instance.dialogList[_index];

        if (endDrop.value == 0)
        {
            // clear endtype node at dialog
        }
        else
        {
            dialog.endValue = endInput.text;
            switch (endDrop.value)
            {
                case 1:
                    dialog.endType = ((int)END_TYPE.CHAPTER_END).ToString();
                    break;
                case 2:
                    dialog.endType = ((int)END_TYPE.SCENE_END).ToString();
                    break;
                case 3:
                    dialog.endType = ((int)END_TYPE.SENTENCE_JUMPTO).ToString();
                    break;
                case 4:
                    dialog.endType = ((int)END_TYPE.GAME_END).ToString();
                    break;
            }
        }

        if (musicPathInput.text == "" && musicStopToggle.isOn == false)
        {
            // clear music tag
        }
        else
        {
            if (musicPathInput.text == "")
            {
                // add empty music tag
            }
            else
            {
                dialog.bgmPath = musicPathInput.text;
                dialog.bgmValue = musicTimeInput.text;
                dialog.bgmVolume = musicVolumeInput.text;
            }
        }

        DialogData.instance.dialogList[_index] = dialog;
        RightPanel.instance.RefreshPanel(_index+1);
    }


}
