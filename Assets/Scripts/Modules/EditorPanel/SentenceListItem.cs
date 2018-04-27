using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SentenceListItem : MonoBehaviour
{
    public LeftPanel.SelectSentence OnSelectSentence;
    private Button _backgroundBtn;
    private Image _headImage;
    private Text _idText;
    private Text _speakerText;
    private int _dialogId;

    void Awake()
    {
        _backgroundBtn = transform.Find("Background").GetComponent<Button>();
        _headImage = transform.Find("head").GetComponent<Image>();
        _headImage.enabled = false;
        _idText = transform.Find("idText").GetComponent<Text>();
        _speakerText = transform.Find("speakerText").GetComponent<Text>();
        _backgroundBtn.onClick.AddListener(() => { OnSelectSentence(_dialogId); });
    }

    public void InitByDialog(Dialog dialog)
    {
        if (dialog == null)
        {
            _speakerText.text = "Error";
            return;
        }

        if(dialog.id != 0)
            _idText.text = dialog.id.ToString();

        if (string.IsNullOrEmpty(dialog.endType))
            _speakerText.text = dialog.speakerName;
        else
            _speakerText.text = "End";

        _dialogId = dialog.id;
    }

    public void OnFocus()
    {
        _backgroundBtn.GetComponent<Image>().color = new Color(0.65f, 0.65f, 0.65f);
    }

    public void OffFocus()
    {
        _backgroundBtn.GetComponent<Image>().color = new Color(1, 1, 1);
    }

}
