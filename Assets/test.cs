using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
    WWW url;
	// Use this for initialization
	void Start () {
        string path = "file://" + Application.dataPath + "/splash.bmp";
        url = new WWW(path);
	}
	
	// Update is called once per frame
	void Update () {
        print(url.url);
	}
}
