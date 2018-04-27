using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class WindowBase : MonoBehaviour
{
    protected Image maskImg;
    protected Text titleText;
    protected Button exitBtn;

    protected virtual void OnAwake()
    {
        if (transform.Find("Mask") != null)
            maskImg = transform.Find("Mask").GetComponent<Image>();
        if (transform.Find("WindowImage/HeadTxt") != null)
           titleText = transform.Find("WindowImage/HeadTxt").GetComponent<Text>();
        if (transform.Find("ExitBtn") != null)
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
        else if (transform.Find("WindowImage/ExitBtn") != null)
            exitBtn = transform.Find("WindowImage/ExitBtn").GetComponent<Button>();

        if (exitBtn != null)
            exitBtn.onClick.AddListener(Exit);
    }

    protected virtual void OnStart()
    {
    }


    protected virtual void Close()
    {
        Destroy(gameObject);
    }

    protected virtual void OnThisDestroy()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    void Awake()
    {
        OnAwake();
    }

    void Start()
    {
        OnStart();
    }

    void OnDestroy()
    {
        OnThisDestroy();
    }

    void Update()
    {
        OnUpdate();
    }

    void Exit()
    {
        Close();
    }

}
