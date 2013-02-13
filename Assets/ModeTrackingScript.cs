using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// Global object which tracks and controls run-time user options. Also draws non-game-specific GUI elements
/// </summary>
public class ModeTrackingScript : MonoBehaviour {
    public int mode = 0;
    public int robotType = 0;
    public int startTile = 2;
    public int timeLimit = 0;
    public GUISkin skin;
    public GameObject waypointObject;
    public const int VIEWMODE_MATCH = 0;
    public const int VIEWMODE_STRATEGIC = 1;
    private bool matchloads = true;
    private bool score = true;
    private bool gates = true;
    private bool cameraControls = true;
    private bool timer = true;
    private bool physics = false;
    private bool useconsole = false;
    private bool showFileMenu = false;
    private bool showSimMenu = false;
    private bool showViewMenu = false;
    private bool showHelpMenu = false;
    private bool showAbtWindow = false;
    private int viewMode_ = 0;
    private string[] viewModes_ = new string[] { "Match View", "Strategic View" };
    private bool statusBar_ = true;
    private bool showMenu_ = false;
    private bool chooseWP_ = false;
    private int curMenu = 0;
    private int pastMenu = 0;
    private Rect aboutRect;
    private GameObject tracker;
    private GameObject field;
    private Field fieldScript;
    private GameObject timekeeper_ = null;
    private Vector2 aboutScrPos_ = Vector2.zero;
    private List<WaypointDescription> waypoints_;
    private int curWaypoint_ = 0;
    private GameObject wpObject_ = null;

    /// <summary>
    /// Get the timekeeping object
    /// </summary>
    public GameObject timekeeper
    {
        get
        {
            return timekeeper_;
        }
        set
        {
            timekeeper_ = value;
        }
    }

    /// <summary>
    /// Determines whether or not to show the match load controls
    /// </summary>
    public bool showML
    {
        get
        {
            return matchloads;
        }
    }
    /// <summary>
    /// Determines whether or not to show the score
    /// </summary>
    public bool showScore
    {
        get
        {
            return score;
        }
    }
    /// <summary>
    /// Determines whether or not to show gate controls (Gateway only)
    /// </summary>
    public bool showGates
    {
        get
        {
            return gates;
        }
    }
    /// <summary>
    /// Determines whether or not to show camera controls (ignored in strategic view)
    /// </summary>
    public bool showCC
    {
        get
        {
            return cameraControls;
        }
    }
    /// <summary>
    /// Determines whether or not to show remaining time
    /// </summary>
    public bool showTimer
    {
        get
        {
            return timer;
        }
    }
    /// <summary>
    /// Show or hide the Physics Analysis Window
    /// </summary>
    public bool showPhysics
    {
        get
        {
            return physics;
        }
    }
    /// <summary>
    /// Show or hide the Console
    /// </summary>
    public bool showConsole
    {
        get
        {
            return useconsole;
        }
    }
    /// <summary>
    /// Show or hide the About window
    /// </summary>
    public bool showAbt
    {
        get { return showAbtWindow; }
        set { showAbtWindow = value; }
    }
    /// <summary>
    /// Show or hide the status bar
    /// </summary>
    public bool showStatusBar
    {
        get { return statusBar_; }
    }
    /// <summary>
    /// The current view (strategic or match)
    /// </summary>
    public int getViewMode
    {
        get { return viewMode_; }
    }

    public bool setIndex(int value)
    {
        if (value < waypoints_.Count)
        {
            curWaypoint_ = value;
            return true;
        }
        return false;
    }

    public void advanceWaypoint()
    {
        curWaypoint_++;
        if (curWaypoint_ < waypoints_.Count)
        {
            wpObject_ = (GameObject)Instantiate(waypointObject);
            wpObject_.transform.position = waypoints_[curWaypoint_].worldPos;
        }
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        tracker = GameObject.Find("Mode Tracker");
        aboutRect = new Rect(Screen.width / 2 - 225, Screen.height / 2 - 250, 450, 500);
        waypoints_ = new List<WaypointDescription>();
	}

    /// <summary>
    /// Main loop
    /// </summary>
    void Update()
    {
        if (field == null && Application.loadedLevel != 0)
        {
            field = GameObject.FindGameObjectWithTag("field");
            fieldScript = field.GetComponent<Field>();
        }
        if (curWaypoint_ == 0 && waypoints_.Count > 0 && wpObject_ == null)
        {
            wpObject_ = (GameObject)Instantiate(waypointObject);
            wpObject_.transform.position = waypoints_[0].worldPos;
        }
        if (curWaypoint_ >= waypoints_.Count && wpObject_ != null)
            wpObject_ = null;
    }

    /// <summary>
    /// Draw the window GUI (menubar and statusbar)
    /// </summary>
    void OnGUI()
    {
        GUI.depth = 1;
        GUI.skin = skin;
        
        if(Application.loadedLevel != 0)
        {
            if (viewMode_ == VIEWMODE_STRATEGIC)
            {
                if(GUI.Button(new Rect(Screen.width - 170, 75, 125, 40), "Add Waypoint"))
                    chooseWP_ = true;
                if (chooseWP_)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Vector3 temp = Input.mousePosition;
                        WaypointDescription wp = new WaypointDescription(new Vector2(temp.x, Screen.height-temp.y));
                        Vector3 world = Camera.main.ScreenToWorldPoint(new Vector3(temp.x, temp.y, 9.21f));
                        wp.worldPos = new Vector3(world.x, 0.0f, world.z);
                        waypoints_.Add(wp);
                        print("" + wp.ToString());
                        chooseWP_ = false;
                    }
                }

                for (int i = 0; i < waypoints_.Count; i++)
                    GUI.Box(new Rect(waypoints_[i].screenCoords.x, waypoints_[i].screenCoords.y, 30, 30), "" + (i + 1));
            }
            CreateMenuBar();
            if(statusBar_)
                GUI.Box(new Rect(0, Screen.height - 22, Screen.width, 22), GUI.tooltip, "status");

            if (showAbtWindow)
                aboutRect = GUI.Window(4, aboutRect, abtFunction, "About VirtualVEX");
        }
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

    /// <summary>
    /// Draws the about window
    /// </summary>
    /// <param name="windowID"></param>
    void abtFunction(int windowID)
    {
        GUI.Box(new Rect(5, 23, 440, 170), "", "transparent");
        aboutScrPos_ = GUI.BeginScrollView(new Rect(10, 200, 435, 230),
        aboutScrPos_, new Rect(0, 0, 400, 400));
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), Constants.abtText);
        GUI.EndScrollView();
        bool ok = GUI.Button(new Rect(30, 450, 50, 25), "OK") || GUI.Button(new Rect(405, 0, 40, 18), "", "close");
        if (ok)
            showAbtWindow = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
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

        GUI.Box(new Rect(0, 0, Screen.width, 22), "", "menubar");
        if (GUI.Button(new Rect(0, 1, 30, 20), "File", b1style))
            curMenu = curMenu == 1? 0 : 1;
        if(GUI.Button(new Rect(30, 1, 70, 20), "Simulation", b2style))
            curMenu = curMenu == 2 ? 0 : 2;
        if (GUI.Button(new Rect(100, 1, 40, 20), "View", b3style))
            curMenu = curMenu == 3 ? 0 : 3;
        if (GUI.Button(new Rect(140, 1, 40, 20), "Help", b4style))
            curMenu = curMenu == 4 ? 0 : 4;

        switch(curMenu)
        {
            case 1:
            {
                GUI.Box(new Rect(0, 22, 130, 40), "", "menudrop");
                if (GUI.Button(new Rect(0, 22, 130, 20), new GUIContent("        Main Menu", "Close the simulation and return to the main menu"), "dropdownitem"))
                {
                    Destroy(tracker);
                    Application.LoadLevel(0);
                }
                if (GUI.Button(new Rect(0, 42, 130, 20), new GUIContent("        Exit", "Quit the application"), "dropdownitem"))
                    Application.Quit();
                break;
            }
            case 2:
            {
                GUI.Box(new Rect(30, 22, 130, 80), "", "menudrop");
                if (GUI.Button(new Rect(30, 22, 130, 20), "        " + pblabel, "dropdownitem"))
                {
                    if (Time.timeScale == 1)
                        fieldScript.pause(null);
                    else
                        fieldScript.unpause(null);
                }
                if (GUI.Button(new Rect(30, 42, 130, 20), new GUIContent("        Stop timer",  "Stop the match timer"), "dropdownitem"))
                    fieldScript.notime(null);
                if (GUI.Button(new Rect(30, 62, 130, 20), new GUIContent("        Reset", "Reset the simulation to start parameters"), "dropdownitem"))
                    fieldScript.reset(null);
                if(GUI.Button(new Rect(30, 82, 130, 20), new GUIContent("        Restart Waypoints", "Restart the waypoints (if any) from the beginning"), "dropdownitem"))
                    curWaypoint_ = 0;
                break;
            }
            case 3:
            {
                GUI.Box(new Rect(100, 22, 130, 190), "", "menudrop");
                cameraControls = GUI.Toggle(new Rect(100, 22, 130, 20), cameraControls, new GUIContent(" Camera Controls", "Toggle visibility of camera position controls"), "menutoggle");
                timer = GUI.Toggle(new Rect(100, 42, 130, 20), timer, new GUIContent(" Timer", "Toggle visibility of game timer"), "menutoggle");
                score = GUI.Toggle(new Rect(100, 62, 130, 20), score, new GUIContent(" Score", "Toggle scorekeeper"), "menutoggle");
                useconsole = GUI.Toggle(new Rect(100, 82, 130, 20), useconsole, new GUIContent(" Console", "Show or hide the command line window"), "menutoggle");
                physics = GUI.Toggle(new Rect(100, 102, 130, 20), physics, new GUIContent(" Physics Window", "Show or hide robot physics statistics"), "menutoggle");
                matchloads = GUI.Toggle(new Rect(100, 122, 130, 20), matchloads, new GUIContent(" Match Loads", "Toggle visibility of Match Loads buttons"), "menutoggle");
                statusBar_ = GUI.Toggle(new Rect(100, 142, 130, 20), statusBar_, new GUIContent(" Status Bar", "Show or hide the Status Bar"), "menutoggle");
                GUI.Box(new Rect(100, 162, 130, 10), "", "separator");
                viewMode_ = GUI.SelectionGrid(new Rect(100, 172, 130, 40), viewMode_, viewModes_, 1, "menutoggle");
                break;
            }
            case 4:
            {
                GUI.Box(new Rect(140, 22, 130, 40), "", "menudrop");
                if (GUI.Button(new Rect(140, 22, 130, 20), new GUIContent("        Documentation", "View help on how to use VirtualVEX (This will open in your browser)"), "dropdownitem"))
                    Process.Start("https://sites.google.com/site/virtualvex/knowledge-base"); ;
                if (GUI.Button(new Rect(140, 42, 130, 20), new GUIContent("        About", "About VirtualVEX"), "dropdownitem"))
                {
                    showAbtWindow = true;
                    curMenu = 0;
                }
                break;
            }
        }
    }
}
