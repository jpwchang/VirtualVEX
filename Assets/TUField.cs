using UnityEngine;
using System.Collections;

/// <summary>
/// A field set up for the Toss Up game
/// </summary>
public class TUField : Field {

    public Transform bucky;
    public Transform beachball;
    public GUISkin skin;

    private GameObject[] goals_;
    private Vector3 redTile1_;
    private Vector3 redTile2_;
    private Vector3 blueTile1_;
    private Vector3 blueTile2_;
    private int redMLCount_;
    private int blueMLCount_;

	// Use this for initialization
	void Start () {
        FieldInit();

        vvIO.vvConsole.setConsoleParent(this);

        goals_ = GameObject.FindGameObjectsWithTag("goal");

        redTile2_ = new Vector3(-1.52f, 0.77f, -1.063f);
        redTile1_ = new Vector3(-1.52f, 0.77f, -0.415f);
        blueTile2_ = new Vector3(1.52f, 0.77f, -1.063f);
        blueTile1_ = new Vector3(1.52f, 0.77f, -0.415f);
        Transform selectedBot = clawbot;

        switch(scr_.robotType)
        {
            case 0:
                selectedBot = clawbot;
                break;
            case 1:
                selectedBot = nz;
                break;
        }
        switch (scr_.startTile)
        {
            case 0:
                selectedBot.position = redTile1_;
                selectedBot.eulerAngles = new Vector3(0, 90, 0);
                break;
            case 1:
                selectedBot.position = blueTile1_;
                selectedBot.eulerAngles = new Vector3(0, -90, 0);
                break;
            case 2:
                selectedBot.position = redTile2_;
                selectedBot.eulerAngles = new Vector3(0, 90, 0);
                break;
            case 3:
                selectedBot.position = blueTile2_;
                selectedBot.eulerAngles = new Vector3(0, -90, 0);
                break;
        }
        robot_ = Instantiate(selectedBot);
	}
	
	// Update is called once per frame
	void Update () {
        doUpdate();

        int blueTotal = 0;
        int redTotal = 0;
        foreach(GameObject o in goals_)
        {
            blueTotal += o.GetComponent<TUGoalScript>().getBlueScore;
            redTotal += o.GetComponent<TUGoalScript>().getRedScore;
        }
        blueScore = blueTotal;
        redScore = redTotal;
	}

    void OnGUI()
    {
        GUI.depth = 1;
        GUI.skin = skin;
        if (scr_.showScore)
        {
            GUI.Box(new Rect(20, 75, 120, 30), "Red: " + (redScore), "Score");
            GUI.Box(new Rect(20, 105, 120, 30), "Blue: " + (blueScore), "Score");
        }

        if (scr_.showML)
        {
            bool redLoad = GUI.Button(new Rect(160, 75, 110, 25), "Red match load");
            bool blueLoad = GUI.Button(new Rect(160, 105, 110, 25), "Blue match load");
            
        }
        if (scr_.showConsole)
        {
            consoleRect_ = GUI.Window(1, consoleRect_, consoleFunction, "Console");
        }
    }
}
