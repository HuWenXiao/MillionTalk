using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileManageItem : MonoBehaviour
{
    public enum ItemType
    {
        COMMON_SAVE,
        NEW_SAVE,
        FAST_SAVE
    }

    public ItemType itemType = ItemType.COMMON_SAVE;
    public Text titleTxt;
    public Image bkgrdImage;
    public Text dateTxt;
    public Text timeTxt;
    public Text numberTxt;
    public delegate void ToggleSelected(FileManageItem item);
    public ToggleSelected OnToggleSelected;
    public int id;
    public SaveFileInfo saveFileInfo;

    void Awake()
    {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(OnToggleValueChanged);

    }

    public void Init(FileInfo fileInfo)
    {
        Debug.Log("name is " + fileInfo.Name);
        Debug.Log("path is " + fileInfo.LastWriteTime);
        titleTxt.text = fileInfo.Name;
        dateTxt.text = fileInfo.LastWriteTime.Date.ToString("yyyy / MM / dd");
        timeTxt.text = fileInfo.LastWriteTime.TimeOfDay.ToString().Split('.')[0];

    }

    void OnToggleValueChanged(bool value)
    {
        if(value)
            OnToggleSelected(this);
    }
}
