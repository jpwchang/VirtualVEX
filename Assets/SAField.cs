using UnityEngine;
using System.Collections;

/// <summary>
/// A Field set up for the Sack Attack game
/// </summary>
public class SAField : Field {
    public Transform sack;
    public GUISkin skin;

    private GameObject[] redGoals_;
    private GameObject[] blueGoals_;
    private GameObject blueHigh_;
    private GameObject redHigh_;
    private Vector3 redTile1_;
    private Vector3 redTile2_;
    private Vector3 blueTile1_;
    private Vector3 blueTile2_;
    private int redMLCount_ = 0;
    private int blueMLCount_ = 0;

	// Use this for initialization
	void Start () {
        FieldInit();

        vvIO.vvConsole.setConsoleParent(this);
        redGoals_ = GameObject.FindGameObjectsWithTag("red_goal");
        blueGoals_ = GameObject.FindGameObjectsWithTag("blue_goal");
        blueHigh_ = GameObject.FindGameObjectWithTag("blue_high");
        redHigh_ = GameObject.FindGameObjectWithTag("red_high");

        cmdTable_["match-load"] = new ConsoleAction(matchLoad);
        redTile1_ = new Vector3(-0.88f, 0.77f, 1.47f);
        redTile2_ = new Vector3(0.88f, 0.77f, 1.47f);
        blueTile1_ = new Vector3(-0.88f, 0.77f, -1.47f);
        blueTile2_ = new Vector3(0.88f, 0.77f, -1.47f);
        Transform selectedBot = clawbot;

        switch (scr_.robotType)
        {
            case 0:
                selectedBot = clawbot;
                break;
            case 1:
                selectedBot = scoop;
                break;
            case 2:
                selectedBot = zackbot;
                break;
            case 3:
                selectedBot = shovel;
                break;
        }
        switch (scr_.startTile)
        {
            case 0:
                selectedBot.position = redTile1_;
                selectedBot.eulerAngles = new Vector3(0, 180, 0);
                break;
            case 1:
                selectedBot.position = redTile2_;
                selectedBot.eulerAngles = new Vector3(0, 180, 0);
                break;
            case 2:
                selectedBot.position = blueTile1_;
                selectedBot.eulerAngles = new Vector3(0, 0, 0);
                break;
            case 3:
                selectedBot.position = blueTile2_;
                selectedBot.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
        robot_ = Instantiate(selectedBot);
	}
	
	// Update is called once per frame
	void Update () {
        doUpdate();
        int redTotal = 0;
        int blueTotal = 0;
        foreach(GameObject o in redGoals_)
            redTotal += o.GetComponent<SAGoalScript>().getScore;
        foreach (GameObject g in blueGoals_)
            blueTotal += g.GetComponent<SAGoalScript>().getScore;
        redTotal += redHigh_.GetComponent<SAGoalScript>().getScore;
        blueTotal += blueHigh_.GetComponent<SAGoalScript>().getScore;
        redScore = redTotal;
        blueScore = blueTotal;
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
                if (redMLCount_ < 4)
                {
                    sack.position = scr_.startTile == 0 ? redTile1_ : redTile2_;
                    Instantiate(sack);
                    redMLCount_++;
                }
            }
            if (blueLoad)
            {
                if (blueMLCount_ < 4)
                {
                    sack.position = scr_.startTile == 2 ? blueTile1_ : blueTile2_;
                    Instantiate(sack);
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
                    sack.position = redTile1_;
                    Instantiate(sack);
                    break;
                case "blue":
                    sack.position = blueTile1_;
                    Instantiate(sack);
                    break;
            }
        }
        else
            vvIO.vvConsole.println("Error: no argument found. Please specify \"red\" or \"blue\"");
    }
}
