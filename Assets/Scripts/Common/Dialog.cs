using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog {

	public int id;
	public string speakerName;
    public string picPosition; //标示该对话的说话者的立绘位置，默认为0表示中间位置,-1表示在左侧，1表示在右侧
	public string leftPic;
    public string rightPic;
    public string midPic;
	public string content;
    public string endType; //标示该节点为场景或章节终点，Type值表示跳转类型，1表示场景终点，2表示章节终点
    public string endValue; //Value值表示跳转的目标章节或场景的Id 
    public string bgmPath;
    public string bgmVolume;
    public string bgmValue;
    public string soundPath;
    public string soundVolume;

	public Dialog()
	{
	}

}
