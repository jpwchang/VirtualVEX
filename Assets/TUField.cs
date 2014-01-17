using UnityEngine;
using System.Collections;

/// <summary>
/// A field set up for the Toss Up game
/// </summary>
public class TUField : Field {

    public Transform buckyRed;
    public Transform buckyBlue;
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

        redTile2_ = new Vector3(-1.5f, 0.9f, -1.063f);
        redTile1_ = new Vector3(-1.5f, 0.9f, -0.415f);
        blueTile2_ = new Vector3(1.5f, 0.9f, -1.063f);
        blueTile1_ = new Vector3(1.5f, 0.9f, -0.415f);
        Transform selectedBot = clawbot;

        cmdTable_["match-load"] = new ConsoleAction(matchLoad);

        switch(scr_.robotType)
        {
            case 0:
                selectedBot = clawbot;
                break;
            case 1:
                selectedBot = tu_roller;
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
            if (redLoad)
            {
                if (redMLCount_ < 2)
                {
                    buckyRed.position = scr_.startTile == 0 ? redTile1_ : redTile2_;
                    Instantiate(buckyRed);
                    redMLCount_++;
                }
            }
            if (blueLoad)
            {
                if (blueMLCount_ < 2)
                {
                    buckyBlue.position = scr_.startTile == 1 ? blueTile1_ : blueTile2_;
                    Instantiate(buckyBlue);
                    blueMLCount_++;
                }
            }
        }
        if (scr_.showConsole)
        {
            consoleRect_ = GUI.Window(1, consoleRect_, consoleFunction, "Console");
        }
    }

    public override void matchLoad(string arg)
    {
        if (arg != null)
        {
            switch (arg)
            {
                case "red":
                    buckyRed.position = redTile1_;
                    Instantiate(buckyRed);
                    break;
                case "blue":
                    buckyBlue.position = blueTile1_;
                    Instantiate(buckyBlue);
                    break;
            }
        }
        else
            vvIO.vvConsole.println("Error: no argument found. Please specify \"red\" or \"blue\"");
    }
}
