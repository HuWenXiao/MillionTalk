using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class FileManageWindow : WindowBase
{
    public Button saveBtn;
    public Button loadBtn;
    public Button deleteBtn;
    public RectTransform contentTrans;
    public ToggleGroup toggleGroup;
    public GameObject fileManageCell;

    private FileManageItem newSaveItem;
    private FileManageItem fastSaveItem;
    private string savePath;
    private List<FileManageItem> fileItemList = new List<FileManageItem>();
    private int toggleIndex;

    protected override void OnAwake()
    {
        base.OnAwake();
        newSaveItem = transform.Find("WindowImage/FilesList/Viewport/Content/NewSaveItem").GetComponent<FileManageItem>();
        fastSaveItem = transform.Find("WindowImage/FilesList/Viewport/Content/FastSaveItem").GetComponent<FileManageItem>();

        saveBtn.onClick.AddListener(OnClickSave);
        loadBtn.onClick.AddListener(OnClickLoad);
        deleteBtn.onClick.AddListener(OnClickDelete);
        newSaveItem.OnToggleSelected += OnToggleChanged;
        fastSaveItem.OnToggleSelected += OnToggleChanged;
        fastSaveItem.gameObject.SetActive(false);
        savePath = Application.dataPath + "/Saves";
        RefreshWindow();
    }

    void RefreshWindow()
    {
        if (!Directory.Exists(savePath)) return;

        //fileItemList.Add(newSaveItem);

        DirectoryInfo direction = new DirectoryInfo(savePath);
        FileInfo[] files = direction.GetFiles();
        for (int i = 0; i < files.Length; i++)
        {
            if (!files[i].Name.EndsWith(".txt")) continue;
            if (files[i].Name.StartsWith("_FastSave"))
            {
                fastSaveItem.gameObject.SetActive(true);
                fastSaveItem.Init(files[i]);
                fileItemList.Add(fastSaveItem);
            }
            else
            {
                GameObject go = Instantiate(fileManageCell);
                go.transform.SetParent(contentTrans);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.GetComponent<Toggle>().group = toggleGroup;
                go.GetComponent<FileManageItem>().Init(files[i]);
                go.GetComponent<FileManageItem>().OnToggleSelected += OnToggleChanged;
            }
        }
        int activeToggleCount = fastSaveItem.isActiveAndEnabled ? contentTrans.childCount : contentTrans.childCount - 1;
        for (int k = 0; k < activeToggleCount; k++)
        {
            var item = contentTrans.GetComponentsInChildren<FileManageItem>()[k];
            if (!item.enabled) continue;
            item.id = k;
            if(item.numberTxt != null)
                item.numberTxt.text = "#" + k.ToString();
            fileItemList.Add(item);
        }
    }

    void OnToggleChanged(FileManageItem item)
    {
        if (item.itemType == FileManageItem.ItemType.NEW_SAVE)
        {
            loadBtn.enabled = false;
            deleteBtn.enabled = false;
        }
        else
        {
            loadBtn.enabled = true;
            deleteBtn.enabled = true;
        }
        toggleIndex = item.id;
    }

    void OnClickSave()
    {
        WindowManager.instance.CreateMsgBox("Do you really want to SAVE HERE?\nThis file will be overwritten.",
            "Save File", MSGBOX_TYPE.ENQUIRE, SaveFile);
    }

    void OnClickLoad()
    {
        WindowManager.instance.CreateMsgBox("Do you really want to LOAD FROM IT?\nCurrent unsaved progress will be lost.",
            "Load File", MSGBOX_TYPE.ENQUIRE, LoadFile);
    }

    void OnClickDelete()
    {
        WindowManager.instance.CreateMsgBox("Do you really want to DELETE IT?\nThis action can not be undone.",
            "Delete File", MSGBOX_TYPE.ENQUIRE, DeleteFile);
    }

    void SaveFile()
    {
        if (fileItemList[toggleIndex] != null)
            SaveManager.instance.Save(fileItemList[toggleIndex].saveFileInfo);
    }

    void LoadFile()
    {
        if (fileItemList[toggleIndex] != null)
            SaveManager.instance.Load(fileItemList[toggleIndex].saveFileInfo);
    }

    void DeleteFile()
    {

    }
}
