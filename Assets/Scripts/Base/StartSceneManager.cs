using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GlobalManager.instance.LoadScene("MainScene");
	}
	
}
