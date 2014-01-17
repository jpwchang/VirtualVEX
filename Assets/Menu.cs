using UnityEngine;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Provides all Main Menu functions and draws the actual menu GUI
/// </summary>
public class Menu : MonoBehaviour {
	public Texture2D logo_;
    public Texture2D contrib_;
    public Texture2D aboutScreen;
    public GameObject tracker;
    public GUISkin skin;
    public GUISkin controlSkin;
    public GUISkin transparent;
    public string str = "Loading News...";
    private Vector2 aboutScrPos_ = Vector2.zero;
    private int robotType_ = 0;
    private int curTab_ = 0;
    private int level_ = 0;
    private int gameType_ = 0;
    private int cameraMode_ = 1;
    private WWW www;
    private WWW updater;
    private string[] robotsTossup = { "Clawbot", "Roller Bot" };
    private string[] robotsSack = { "Clawbot", "Scooper Bot", "Roller Bot", "Shovel Bot" };
    private string[] robotsGateway = { "Clawbot", "ConveyorBot", "H. Roller", "V. ConveyorBot", "V. Roller" };
    private string[] startTilesSack = { "Red 1", "Red 2", "Blue 1", "Blue 2" };
    private string[] tabs = { "SIMULATION", "GETTING STARTED", "NEWS & UPDATES" };
    private string[] games = { "Gateway", "Sack Attack", "Toss Up" };
    private string[] cameraModes_ = { "4 corners (Gateway, Sack Attack)", "1-sided (Toss Up)" };
    private string[] startTilesGateway = { "Blue 1", "Red 1", "Blue 2", "Red 2" };
    private string[] startTilesToss_ = { "Red 1", "Blue 1", "Red 2", "Blue 2" };
    private string[] gameTypes = { "Solo Untimed", "Robot Skills", "Custom" };
    private string[] startTiles;
    private string[] toolStrs;
    private Rect windowRect;
    private bool showAboutWindow;
    private bool showSettingsWindow;
    private string getVersion = "";
    private bool showWindow = false;
    private Rect updateRect;
    private Rect settingsRect;
    private int startTile = 2;
    private int curGameType;
    private string timeLimit = "0";
    private int fieldType = 0;
    private const int GAME_GATEWAY = 0;
    private const int GAME_SACK = 1;
    private const int GAME_TOSS = 2;
    private const int CAMERA_4CORNERS = 0;
    private const int CAMERA_1SIDE = 1;
    private bool disable_;

    /// <summary>
    /// Custom lightweight absolute value function
    /// </summary>
    /// <param name="num">The input number</param>
    /// <returns>Absolute value of the input</returns>
    private double abs(double num)
    {
        if(num < 0)
            return -1 * num;
        return num;
    }

    /// <summary>
    /// Access the update server and initialize GUI positions
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        curGameType = GAME_TOSS;
        toolStrs = robotsSack;
        startTiles = startTilesSack;
	    www = new WWW("https://sites.google.com/site/virtualvex/project-updates/onelastrally/Update.txt");
	    yield return www;
        StartCoroutine(loadWWW());
	    str = www.text;
        windowRect = new Rect(Screen.width / 2 - 225, Screen.height / 2 - 250, 450, 500);
        updateRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 200);
        settingsRect = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400);
        showAboutWindow = false;
        showSettingsWindow = false;
        disable_ = false;
    }

    /// <summary>
    /// Main loop
    /// </summary>
    void Update()
    {
        if (curGameType == GAME_SACK)
        {
            toolStrs = robotsSack;
            startTiles = startTilesSack;
        }
        else if (curGameType == GAME_GATEWAY)
        {
            toolStrs = robotsGateway;
            startTiles = startTilesGateway;
        }
        else if(curGameType == GAME_TOSS)
        {
            toolStrs = robotsTossup;
            startTiles = startTilesToss_;
        }
    }

    /// <summary>
    /// Draw the Menu GUI
    /// </summary>
    void OnGUI () {
        GUI.skin = transparent;
        GUI.Box(new Rect(0, 6, 400, 80), logo_);
        GUI.Label(new Rect(Screen.width - 200, 16, 200, 80), "Start Page");
        GUI.Label(new Rect(10, Screen.height - 35, 100, 35), Constants.VERSION, "versionlabel");
        GUI.Box(new Rect(Screen.width - 270, Screen.height - 90, 270, 90), contrib_);
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(10, 100, 800, 450));
        curTab_ = GUI.Toolbar(new Rect(0, 0, 500, 50), curTab_, tabs);
        GUI.skin = controlSkin;
        if (curTab_ == 0)
        {
            GUI.Box(new Rect(5, 65, 210, 205), "Starting Tile");
            GUI.Box(new Rect(220, 65, 130, 300), "Robot Type");
            GUI.Box(new Rect(355, 65, 130, 300), "Game Type");
            robotType_ = GUI.SelectionGrid(new Rect(225, 95, 120, 33*toolStrs.Length), robotType_, toolStrs, 1);
            startTile = GUI.SelectionGrid(new Rect(10, 95, 200, 160), startTile, startTiles, 2);
            gameType_ = GUI.SelectionGrid(new Rect(360, 95, 120, 99), gameType_, gameTypes, 1);
            bool start = GUI.Button(new Rect(10, 280, 150, 40), "Start Simulation");
            tracker.GetComponent<ModeTrackingScript>().robotType = robotType_;
            tracker.GetComponent<ModeTrackingScript>().startTile = startTile;
            tracker.GetComponent<ModeTrackingScript>().disableOnTimeUp = disable_;
            bool advanced = GUI.Button(new Rect(170, 280, 150, 40), "Advanced Options");
            if (advanced)
                showSettingsWindow = true;
            switch(gameType_)
            { 
                case 0:
		            fieldType = 0;
                    timeLimit = "0";
                    disable_ = false;
                    GUI.Box(new Rect(490, 90, 250, 100), Constants.DESC_SOLO, "news");
                    break;
                case 1:
		            fieldType = 1;
                    timeLimit = "60";
                    disable_ = true;
                    GUI.Box(new Rect(490, 90, 250, 100), Constants.DESC_SKILLS, "news");
                    break;
                case 2:
                    GUI.Box(new Rect(490, 65, 300, 300), "Custom Game Settings");
                    GUI.Label(new Rect(495, 95, 100, 20), "Time Limit");
                    timeLimit = GUI.TextField(new Rect(560, 95, 100, 20), timeLimit);
                    GUI.Box(new Rect(480, 115, 300, 10), "", "separator");
                    GUI.Label(new Rect(495, 125, 100, 20), "Field Type");
                    fieldType = GUI.SelectionGrid(new Rect(495, 150, 100, 50), fieldType, new string[] { "Normal Field", "Skills Field" }, 1, "toggle");
                    GUI.Box(new Rect(480, 200, 300, 10), "", "separator");
                    disable_ = GUI.Toggle(new Rect(495, 210, 200, 20), disable_, "Disable bot when time is up", "checkbox");
                    break;
            }
            if (start)
            {
                ModeTrackingScript mts = tracker.GetComponent<ModeTrackingScript>();
                mts.mode = fieldType;
                mts.getCameraMode = cameraMode_;
                if (fieldType == 0)
                {
                    switch(curGameType)
                    {
                        case GAME_GATEWAY:
                            level_ = 0;
                            break;
                        case GAME_SACK:
                            level_ = 2;
                            break;
                        case GAME_TOSS:
                            level_ = 3;
                            break;
                    }
                }
                else if (fieldType == 1)
                {
                    switch(curGameType)
                    {
                        case GAME_GATEWAY:
                            level_ = 1;
                            break;
                        case GAME_SACK:
                            level_ = 2;
                            break;
                        case GAME_TOSS:
                            level_ = 3;
                            break;
                    }
                }
                try
                {
                    int limit = System.Int32.Parse(timeLimit);
                    mts.timeLimit = limit;
                }
                catch (System.FormatException)
                {
                    mts.timeLimit = 0;
                    UnityEngine.Debug.Log("Caught bad time value: " + timeLimit);
                }
                Application.LoadLevel(level_ + 1);
            }
        }
        else if (curTab_ == 1)
        {
            GUI.Box(new Rect(5, 65, 300, 120), "Documentation", "score");
            GUI.Label(new Rect(10, 90, 290, 110), "The VirtualVEX online documentation provides full help on all features of the program.");
            bool help = GUI.Button(new Rect(10, 155, 130, 25), "View Documentation");
            GUI.Box(new Rect(310, 65, 300, 120), "Website", "score");
            GUI.Label(new Rect(315, 90, 290, 110), "Visit the official VirtualVEX website for news, downloads, help, source code, and much more.");
            bool site = GUI.Button(new Rect(315, 155, 130, 25), "Visit Website");
            bool abt = GUI.Button(new Rect(10, 280, 150, 40), "About VirtualVEX");
            if (abt)
                showAboutWindow = true;
            if (help)
                Application.OpenURL("https://sites.google.com/site/virtualvex/knowledge-base");
            if (site)
                Application.OpenURL("https://sites.google.com/site/virtualvex/");
        }
        else if (curTab_ == 2)
        {
            GUI.Box(new Rect(5, 65, 400, 350), "Latest News", "score");
            if (!www.isDone)
                GUI.Box(new Rect(10, 90, 390, 320), "Loading News");
            else
                GUI.Box(new Rect(10, 90, 390, 320), str, "news");
            GUI.Box(new Rect(410, 65, 300, 120), "Check for Updates", "score");
            GUI.Label(new Rect(415, 90, 290, 110), "Keep VirtualVEX up to date by checking for the latest patches and updates.");
            bool check = GUI.Button(new Rect(420, 155, 150, 25), "Check Now");
            if (check)
                showWindow = true;
        }
        if (showWindow)
            updateRect = GUI.Window(1, updateRect, checkForUpdate, "Check for Updates");
        if (showAboutWindow)
            windowRect = GUI.Window(0, windowRect, abtFunction, "About VirtualVEX");
        if (showSettingsWindow)
            settingsRect = GUI.Window(2, settingsRect, settingsFunction, "Advanced Options");
	    bool exit = GUI.Button(new Rect(5, 420, 150, 25), "Exit VirtualVEX");
	    GUI.EndGroup();
	    if(exit)
	        Application.Quit();
    }

    /// <summary>
    /// Access the update server
    /// </summary>
    /// <returns></returns>
    IEnumerator loadWWW()
    {
        updater = new WWW("https://sites.google.com/site/virtualvex/project-updates/onelastrally/currentversion.txt");
        yield return updater;
        getVersion = updater.text;
    }

    /// <summary>
    /// Show the about window
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
            showAboutWindow = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    /// <summary>
    /// Check current version against version on server to determine if there are updates
    /// </summary>
    /// <param name="windowID"></param>
    void checkForUpdate(int windowID)
    {
        string windowText = "Attempting to connect to update server, try again later.";
        if (updater.isDone)
        {
            if (Constants.VERSION.Equals(getVersion))
                windowText = "No update was found. You are running the latest version of\nVirtualVEX.";
            else
            {
                windowText = "An update was found: " + getVersion;
                bool download = GUI.Button(new Rect(30, 50, 100, 25), "Download");
                if (download)
                    Application.OpenURL("https://sites.google.com/site/virtualvex/downloads");
            }
        }
        GUI.Label(new Rect(30, 30, Screen.width - 30, Screen.height - 30), windowText);
        bool ok = GUI.Button(new Rect(30, 150, 50, 25), "OK") || GUI.Button(new Rect(355, 0, 40, 18), "", "close");
        if (ok)
            showWindow = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    /// <summary>
    /// Draw the advanced settings window
    /// </summary>
    /// <param name="windowID"></param>
    void settingsFunction(int windowID)
    {
        GUI.Label(new Rect(30, 30, Screen.width - 30, Screen.height - 30), "Change Game:");
        curGameType = GUI.SelectionGrid(new Rect(30, 50, 150, 60), curGameType, games, 1, "toggle");
        GUI.Label(new Rect(30, 120, Screen.width - 30, Screen.height - 30), "Camera mode:");
        cameraMode_ = GUI.SelectionGrid(new Rect(30, 140, 250, 40), cameraMode_, cameraModes_, 1, "toggle");
        bool close = GUI.Button(new Rect(30, 340, 120, 30), "OK") || GUI.Button(new Rect(355, 0, 40, 18), "", "close");
        if (close)
        {
            showSettingsWindow = false;
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
