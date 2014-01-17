using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// Controls the camera and game-specific GUI elements
/// </summary>
public class GUIScript : MonoBehaviour {
    public GUISkin skin;
    private GameObject gateRed_;
    private GameObject gateBlue_;
    private bool isBlueRaised_ = false;
    private bool isRedRaised_ = false;
    private bool showMenu_ = false;
    private GameObject tracker_;
    private ModeTrackingScript trackerScript;
    private GameObject field_;
    private Field fieldScript_;
    private int cameraPosition_ = 2;
    private string[] cameraStrings_ = new string[] { "Top Left", "Top Right", "Bottom Left", "Bottom Right" };
    private string[] tossUpCameraStrings_ = new string[] { "Red", "Blue" };
    private string[] viewModes_ = new string[] { "Match View", "Strategic View" };
    private Vector3 tlAngle_ = new Vector3(11.47146f, 135.0f, 0.0f);
    private Vector3 tlPos_ = new Vector3(-3.325411f, 1.811674f, 3.373904f);
    private Vector3 trAngle_ = new Vector3(11.47146f, -135.0f, 0.0f);
    private Vector3 trPos_ = new Vector3(3.325411f, 1.811674f, 3.373904f);
    private Vector3 blAngle_ = new Vector3(11.47146f, 45.0f, 0.0f);
    private Vector3 blPos_ = new Vector3(-3.325411f, 1.811674f, -3.373904f);
    private Vector3 brAngle_ = new Vector3(11.47146f, -45.0f, 0.0f);
    private Vector3 brPos_ = new Vector3(3.325411f, 1.811674f, -3.373904f);
    private Vector3 redAngle_ = new Vector3(11.0583f, 9.572f, 0.0f);
    private Vector3 redPos_ = new Vector3(-0.6641f, 2.03f, -5.2287f);
    private Vector3 blueAngle_ = new Vector3(11.0583f, -9.572f, 0.0f);
    private Vector3 bluePos_ = new Vector3(0.6641f, 2.03f, -5.2287f);
    private Vector3 topAngle_ = new Vector3(90, 0, 0);
    private Vector3 topPos_ = new Vector3(0, 10, 0);
    private int curMenu = 0;
    private int pastMenu = 0;

    /// <summary>
    /// Current position of the camera
    /// </summary>
    public int cameraPos
    {
        get { return cameraPosition_; }
        set { cameraPosition_ = value; }
    }

	// Use this for initialization
	void Start () {
        gateRed_ = GameObject.Find("redGate");
        gateBlue_ = GameObject.Find("blueGate");
        tracker_ = GameObject.Find("Mode Tracker");
        if (tracker_ != null)
            trackerScript = tracker_.GetComponent<ModeTrackingScript>();
        field_ = GameObject.FindGameObjectWithTag("field");
        fieldScript_ = field_.GetComponent<Field>();

        if (trackerScript.getCameraMode == 1)
            cameraPosition_ = 0;
	}

    void OnGUI()
    {
        GUI.skin = skin;
	    if(Application.loadedLevel == 1)
	    {
		    isRedRaised_ = !(gateRed_.transform.rotation.z == 0);
		    isBlueRaised_ = !(gateBlue_.transform.rotation.z == 0);
		    if(trackerScript != null && trackerScript.showGates)
		    {
			    bool redButton = GUI.Button(new Rect(150, 75, 140, 20), "Raise Red Gate [R]");
			    bool blueButton = GUI.Button(new Rect(150, 100, 140, 20), "Raise Blue Gate [B]");
			    if((redButton || Input.GetKeyDown("r")) && ! isRedRaised_)
			    {
				    gateRed_.transform.Rotate(0, 0, 90);
				    gateRed_.transform.Translate(0.0f, -1.8288f, 0.0f);
			    }
			    if((blueButton || Input.GetKeyDown("b")) && ! isBlueRaised_)
			    {
				    gateBlue_.transform.Rotate(0, 0, 90);
				    //gateBlue.transform.Translate(-0.9144, 0, -1.2);
			    }
		    }
	    }
	
	    if(trackerScript != null && trackerScript.showCC && trackerScript.getViewMode == ModeTrackingScript.VIEWMODE_MATCH)
	    {
		    GUI.Box(new Rect(Screen.width-220, 100, 220, 110), "Camera Position");
            if (trackerScript.getCameraMode == 0)
                cameraPosition_ = GUI.SelectionGrid(new Rect(Screen.width - 210, 140, 200, 65), cameraPosition_, cameraStrings_, 2);
            else if (trackerScript.getCameraMode == 1)
                cameraPosition_ = GUI.Toolbar(new Rect(Screen.width - 210, 140, 200, 30), cameraPosition_, tossUpCameraStrings_);
	    }

        if (trackerScript.getViewMode == 0)
        {
            if (trackerScript.getCameraMode == 0)
            {
                switch (cameraPosition_)
                {
                    case 0:
                        transform.eulerAngles = tlAngle_;
                        transform.position = tlPos_;
                        break;
                    case 1:
                        transform.eulerAngles = trAngle_;
                        transform.position = trPos_;
                        break;
                    case 2:
                        transform.eulerAngles = blAngle_;
                        transform.position = blPos_;
                        break;
                    case 3:
                        transform.eulerAngles = brAngle_;
                        transform.position = brPos_;
                        break;
                    default:
                        break;
                }
            }
            else if(trackerScript.getCameraMode == 1)
            {
                switch(cameraPosition_)
                {
                    case 0:
                        transform.eulerAngles = redAngle_;
                        transform.position = redPos_;
                        break;
                    case 1:
                        transform.eulerAngles = blueAngle_;
                        transform.position = bluePos_;
                        break;
                    default:
                        break;
                }
            }
        }
        else if (trackerScript.getViewMode == 1)
        {
            transform.eulerAngles = topAngle_;
            transform.position = topPos_;
        }

        CreateMenuBar();

        if (trackerScript.showStatusBar)
        {
            GUI.Box(new Rect(0, Screen.height - 22, Screen.width, 22), GUI.tooltip, "status");
            GUI.Label(new Rect(0, Screen.height - 12, Screen.width, 12), trackerScript.status, "labelRight");
        }
    }

    /// <summary>
    /// Draws the Menu Bar and dropdown menus
    /// </summary>
    private void CreateMenuBar()
    {
        string pblabel = Time.timeScale == 0 ? "Unpause" : "Pause";
        string b1style = curMenu == 1 ? "menuitempress" : "menuitem";
        string b2style = curMenu == 2 ? "menuitempress" : "menuitem";
        string b3style = curMenu == 3 ? "menuitempress" : "menuitem";
        string b4style = curMenu == 4 ? "menuitempress" : "menuitem";

        GUI.Box(new Rect(0, 0, Screen.width, 22), ""); //The bar itself

        switch (curMenu)
        {
            case 1:
                {
                    GUI.Box(new Rect(0, 20, 130, 42), "", "menudrop");
                    if (GUI.Button(new Rect(0, 22, 130, 20), new GUIContent("        Main Menu", "Close the simulation and return to the main menu"), "dropdownitem"))
                    {
                        Destroy(tracker_);
                        Application.LoadLevel(0);
                    }
                    if (GUI.Button(new Rect(0, 42, 130, 20), new GUIContent("        Exit", "Quit the application"), "dropdownitem"))
                        Application.Quit();
                    break;
                }
            case 2:
                {
                    GUI.Box(new Rect(30, 20, 130, 102), "", "menudrop");
                    if (GUI.Button(new Rect(30, 22, 130, 20), "        " + pblabel, "dropdownitem"))
                    {
                        if (Time.timeScale == 1)
                            fieldScript_.pause(null);
                        else
                            fieldScript_.unpause(null);
                    }
                    if (GUI.Button(new Rect(30, 42, 130, 20), new GUIContent("        Stop timer", "Stop the match timer"), "dropdownitem"))
                        fieldScript_.notime(null);
                    if (GUI.Button(new Rect(30, 62, 130, 20), new GUIContent("        Reset", "Reset the simulation to start parameters"), "dropdownitem"))
                        fieldScript_.reset(null);
                    if (GUI.Button(new Rect(30, 82, 130, 20), new GUIContent("        Restart Waypoints", "Restart the waypoints (if any) from the beginning"), "dropdownitem"))
                        trackerScript.getWaypoint = 0;
                    if (GUI.Button(new Rect(30, 102, 130, 20), new GUIContent("        Clear Waypoints", "Remove all currently set waypoints"), "dropdownitem"))
                    {
                        trackerScript.wpReset();
                    }
                    break;
                }
            case 3:
                {
                    GUI.Box(new Rect(100, 20, 130, 192), "", "menudrop");
                    trackerScript.showCC = GUI.Toggle(new Rect(100, 22, 130, 20), trackerScript.showCC, new GUIContent(" Camera Controls", "Toggle visibility of camera position controls"), "menutoggle");
                    trackerScript.showTimer = GUI.Toggle(new Rect(100, 42, 130, 20), trackerScript.showTimer, new GUIContent(" Timer", "Toggle visibility of game timer"), "menutoggle");
                    trackerScript.showScore = GUI.Toggle(new Rect(100, 62, 130, 20), trackerScript.showScore, new GUIContent(" Score", "Toggle scorekeeper"), "menutoggle");
                    trackerScript.showConsole = GUI.Toggle(new Rect(100, 82, 130, 20), trackerScript.showConsole, new GUIContent(" Console", "Show or hide the command line window"), "menutoggle");
                    trackerScript.showPhysics = GUI.Toggle(new Rect(100, 102, 130, 20), trackerScript.showPhysics, new GUIContent(" Physics Window", "Show or hide robot physics statistics"), "menutoggle");
                    trackerScript.showML = GUI.Toggle(new Rect(100, 122, 130, 20), trackerScript.showML, new GUIContent(" Match Loads", "Toggle visibility of Match Loads buttons"), "menutoggle");
                    trackerScript.showStatusBar = GUI.Toggle(new Rect(100, 142, 130, 20), trackerScript.showStatusBar, new GUIContent(" Status Bar", "Show or hide the Status Bar"), "menutoggle");
                    GUI.Box(new Rect(100, 162, 130, 10), "", "separator");
                    trackerScript.getViewMode = GUI.SelectionGrid(new Rect(100, 172, 130, 40), trackerScript.getViewMode, viewModes_, 1, "menutoggle");
                    break;
                }
            case 4:
                {
                    GUI.Box(new Rect(140, 20, 130, 42), "", "menudrop");
                    if (GUI.Button(new Rect(140, 22, 130, 20), new GUIContent("        Documentation", "View help on how to use VirtualVEX (This will open in your browser)"), "dropdownitem"))
                        Process.Start("https://sites.google.com/site/virtualvex/knowledge-base"); ;
                    if (GUI.Button(new Rect(140, 42, 130, 20), new GUIContent("        About", "About VirtualVEX"), "dropdownitem"))
                    {
                        trackerScript.showAbt = true;
                        curMenu = 0;
                    }
                    break;
                }
        }

        if (GUI.Button(new Rect(0, 1, 30, 20), "File", b1style))
            curMenu = curMenu == 1 ? 0 : 1;
        if (GUI.Button(new Rect(30, 1, 70, 20), "Simulation", b2style))
            curMenu = curMenu == 2 ? 0 : 2;
        if (GUI.Button(new Rect(100, 1, 40, 20), "View", b3style))
            curMenu = curMenu == 3 ? 0 : 3;
        if (GUI.Button(new Rect(140, 1, 40, 20), "Help", b4style))
            curMenu = curMenu == 4 ? 0 : 4;
    }

    /// <summary>
    /// Check for mouse clicks and close the open menu if click is detected
    /// </summary>
    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0) && showMenu_)
        {
            showMenu_ = false;
            curMenu = 0;
        }
        if (curMenu > 0)
            showMenu_ = true;
    }
}
