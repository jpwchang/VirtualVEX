using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Update : MonoBehaviour {
    private WWW www;
    private string getVersion = "";
    private bool showWindow = false;
    private Rect windowRect;
    private const string VERSION = "1.0 alpha 13";

    public GUISkin skin;

    void Start () {
        StartCoroutine(loadWWW());
    }

    IEnumerator loadWWW()
    {
        www = new WWW("https://sites.google.com/site/virtualvex/project-updates/onelastrally/currentversion.txt");
        yield return www;
        getVersion = www.text;
    }

    void Awake()
    {
        windowRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 200);
    }

    void OnGUI () {
	    GUI.skin = skin;
	    bool check = GUI.Button(new Rect(200, Screen.height-35, 150, 25), "Check for Updates");
	    if(check) 
            showWindow = true;
        if (showWindow)
            windowRect = GUI.Window(0, windowRect, checkForUpdate, "Check for Updates");
    }

    void checkForUpdate(int windowID)
    {
        string windowText = "Attempting to connect to update server, try again later.";
        if (www.isDone)
        {
            if (VERSION.Equals(getVersion))
                windowText = "No update was found. You are running the latest version of\nVirtualVEX.";
            else
            {
                windowText = "An update was found: " + getVersion;
                bool download = GUI.Button(new Rect(30, 50, 100, 25), "Download");
                if (download)
                    Process.Start("https://sites.google.com/site/virtualvex/downloads");
            }
        }
        GUI.Label(new Rect(30, 30, Screen.width - 30, Screen.height - 30), windowText);
        bool ok = GUI.Button(new Rect(30, 150, 50, 25), "OK");
        if (ok)
            showWindow = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
