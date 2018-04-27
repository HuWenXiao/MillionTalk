using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class BkgrdBrowseItem : MonoBehaviour
{
    public Button goBtn;
    public Image bkgrdImage;
    public Text nameTxt;
    public delegate void SetBkgrdItem(string name);
    public SetBkgrdItem OnSetBkgrdItem;

	void Awake () {
		goBtn.onClick.AddListener(OnClickBtn);
	}
	

    public void Init(string name, string path = null)
    {
        nameTxt.text = name;
        if (path != null)
        {
            var nameArray = name.Split('.');
            nameArray[nameArray.Length-1] = "";
            StringBuilder strb = new StringBuilder();
            foreach (string str in nameArray)
            {
                strb.Append(str);
            }

            Sprite sprite = new Sprite();
            sprite = Resources.Load<Sprite>("Pictures/Background/" + strb);
            nameTxt.text = strb.ToString();
            if (sprite != null)
                bkgrdImage.overrideSprite = sprite;
        }
    }

    void OnClickBtn()
    {
        OnSetBkgrdItem(nameTxt.text);
    }
}
