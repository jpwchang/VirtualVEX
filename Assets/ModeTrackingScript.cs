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
    private bool disable_ = false;
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
        get { return matchloads; }
        set { matchloads = value; }
    }
    /// <summary>
    /// Determines whether or not to show the score
    /// </summary>
    public bool showScore
    {
        get { return score; }
        set { score = value; }
    }
    /// <summary>
    /// Determines whether or not to show gate controls (Gateway only)
    /// </summary>
    public bool showGates
    {
        get { return gates; }
        set { gates = value; }
    }
    /// <summary>
    /// Determines whether or not to show camera controls (ignored in strategic view)
    /// </summary>
    public bool showCC
    {
        get { return cameraControls; }
        set { cameraControls = value; }
    }
    /// <summary>
    /// Determines whether or not to show remaining time
    /// </summary>
    public bool showTimer
    {
        get { return timer; }
        set { timer = value; }
    }
    /// <summary>
    /// Show or hide the Physics Analysis Window
    /// </summary>
    public bool showPhysics
    {
        get { return physics; }
        set { physics = value; }
    }
    /// <summary>
    /// Show or hide the Console
    /// </summary>
    public bool showConsole
    {
        get { return useconsole; }
        set { useconsole = value; }
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
        set { statusBar_ = value; }
    }
    /// <summary>
    /// The current view (strategic or match)
    /// </summary>
    public int getViewMode
    {
        get { return viewMode_; }
        set { viewMode_ = value; }
    }

    public int getWaypoint
    {
        get { return curWaypoint_; }
        set { curWaypoint_ = value; }
    }

    /// <summary>
    /// Should we disable robot when time is up?
    /// </summary>
    public bool disableOnTimeUp
    {
        get { return disable_; }
        set { disable_ = value; }
    }

    public bool setIndex(int value)
    {
        if (value <= waypoints_.Count)
        {
            curWaypoint_ = value-1;
            Destroy(wpObject_);
            wpObject_ = (GameObject)Instantiate(waypointObject);
            wpObject_.transform.position = waypoints_[curWaypoint_].worldPos;
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
                chooseWP_ = GUI.Toggle(new Rect(Screen.width - 170, 75, 125, 40), chooseWP_, "Add Waypoint", "Button");
                if (chooseWP_)
                {
                    GUI.Box(new Rect(Screen.width / 2 - 150, 50, 300, 40), "Click on location to place waypoint");
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

            if (showAbtWindow)
                aboutRect = GUI.Window(4, aboutRect, abtFunction, "About VirtualVEX");
        }
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

    public void wpReset()
    {
        if (wpObject_ != null)
        {
            Destroy(wpObject_);
            wpObject_ = null;
        }
        waypoints_ = new List<WaypointDescription>();
    }
}
