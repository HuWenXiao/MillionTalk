using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour {

	public float fadeSpeed = 0.25f;
	public bool isFadeReverse = false;//该项为假时，该幕布实例化后执行从纯黑到透明的渐变；为真则为从透明到纯黑的遮盖

	CanvasGroup cg;

	void Start () {
		
		cg = gameObject.GetComponent<CanvasGroup> ();
		if (isFadeReverse)
			cg.alpha = 0.0f;

	}
		
	void FixedUpdate () {
		if (isFadeReverse) {
			if (cg.alpha < 1) {
				cg.alpha = cg.alpha + fadeSpeed * Time.deltaTime;
			} 
		}

		else {
			if (cg.alpha > float.Epsilon) {
				cg.alpha = cg.alpha - fadeSpeed * Time.deltaTime;
			}
			else {
				Destroy (gameObject);//幕布变为透明后即销毁自身。若isFadeReverse为真则需要以外部方式进行销毁
			}
		}
		
	}
}
