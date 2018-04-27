using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSceneItem : MonoBehaviour
{
    public Text idText;
    public Toggle chooseToggle;
    public bool isChapter = false;
    public delegate void ToggleSelected();
    public ToggleSelected OnToggleSelected;


    void Awake()
    {
        chooseToggle.onValueChanged.AddListener(SetSceneNode);
    }

    void SetSceneNode(bool isOn)
    {
        if (isOn && isChapter)
        {
            int id = int.Parse(idText.text);
            DialogData.instance.SetSceneNode(id);
            OnToggleSelected();
        }
    }

    public void Init(int id)
    {
        idText.text = id.ToString();
    }


}
