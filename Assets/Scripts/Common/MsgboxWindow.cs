using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MsgboxWindow : MonoBehaviour
{
    public Text headText;
    public Text msgText;
    public Button confirmBtn;
    public Button okBtn;
    public Button cancelBtn;
    public Button exitBtn;

    public object[] windowParams;

    void Awake()
    {
        WindowManager.instance.OnSceneChanged += Close;
        exitBtn.enabled = false;
    }

    public virtual void Init()
    {
        
    }

    public void Init(string msg, string head = null, MSGBOX_TYPE type = MSGBOX_TYPE.CONFIRM,
        UnityAction positiveAction = null, UnityAction cancelAction = null)
    {
        msgText.text = msg;
        if (head != null)
            headText.text = head;
        if (type == MSGBOX_TYPE.CONFIRM)
        {
            confirmBtn.gameObject.SetActive(true);
            okBtn.gameObject.SetActive(false);
            cancelBtn.gameObject.SetActive(false);

            positiveAction += Close;
            confirmBtn.onClick.AddListener(positiveAction);
        }
        if (type == MSGBOX_TYPE.ENQUIRE)
        {
            confirmBtn.gameObject.SetActive(false);
            okBtn.gameObject.SetActive(true);
            cancelBtn.gameObject.SetActive(true);

            positiveAction += Close;
            okBtn.onClick.AddListener(positiveAction);

            cancelAction += Close;
            cancelBtn.onClick.AddListener(cancelAction);

        }
    }

    public void Close()
    {
        WindowManager.instance.OnSceneChanged -= Close;
        Destroy(gameObject);
    }

}
