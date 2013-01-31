using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatewayField : Field {
    public GameObject[] goals;
    public Transform redBall;
    public Transform redCyl;
    public Transform blueBall;
    public Transform blueCyl;
    public Transform doubler;
    public Transform negator;
    public GameObject redGate;
    public GameObject blueGate;
    public GUISkin skin;

    private int numRedBalls = 0;
    private int numRedCyls = 0;
    private int numBlueBalls = 0;
    private int numBlueCyls = 0;
    private bool redDoubler = false;
    private bool blueDoubler = false;
    private bool blueNegator = false;
    private bool redNegator = false;

    void Start()
    {
        FieldInit();
        vvIO.vvConsole.setConsoleParent(this);
        cmdTable_["gates-down"] = new ConsoleAction(gatesDown);
        cmdTable_["doubler"] = new ConsoleAction(Doubler);
        cmdTable_["negator"] = new ConsoleAction(Negator);
        cmdTable_["match-load"] = new ConsoleAction(matchLoad);
        redGate = GameObject.Find("redGate");
        blueGate = GameObject.Find("blueGate");
        goals = GameObject.FindGameObjectsWithTag("goal");
        Transform selectedBot = clawbot;
        switch (scr_.robotType)
        {
            case 0:
                selectedBot = clawbot;
                break;
            case 1:
                selectedBot = conv;
                break;
            case 2:
                selectedBot = nz;
                break;
            case 3:
                selectedBot = vertical;
                break;
            case 4:
                selectedBot = dbot;
                break;
        }
        switch (scr_.startTile)
        {
            case 0:
                selectedBot.position = new Vector3(-1.5f, 0.77f, 0.3f);
                selectedBot.eulerAngles = new Vector3(0, 90, 0);
                break;
            case 1:
                selectedBot.position = new Vector3(1.5f, 0.77f, 0.3f);
                selectedBot.eulerAngles = new Vector3(0, -90, 0);
                break;
            case 2:
                selectedBot.position = new Vector3(-0.88f, 0.77f, -1.47f);
                selectedBot.eulerAngles = new Vector3(0, 0, 0);
                break;
            case 3:
                selectedBot.position = new Vector3(0.88f, 0.77f, -1.47f);
                selectedBot.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
        robot_ = Instantiate(selectedBot);
    }
    void Update()
    {
        redTotal = 0;
        blueTotal = 0;
        foreach (GameObject g in goals)
        {
            redTotal += g.GetComponent<GoalScript>().getRedScore;
            blueTotal += g.GetComponent<GoalScript>().getBlueScore;
        }
        redScore = redTotal;
        blueScore = blueTotal;
    }

    void OnGUI()
    {
        GUI.skin = skin;
        if (scr_.showScore)
        {
            GUI.Box(new Rect(20, 75, 120, 30), "Red: " + (redScore), "Score");
            GUI.Box(new Rect(20, 105, 120, 30), "Blue: " + (blueScore), "Score");
        }
        if (scr_.showML)
        {
            bool redLoad = GUI.Button(new Rect(295, 75, 110, 20), "Red match load");
            bool blueLoad = GUI.Button(new Rect(295, 100, 110, 20), "Blue match load");
            bool redDouble = GUI.Button(new Rect(410, 75, 100, 20), "Red Doubler");
            bool redNeg = GUI.Button(new Rect(515, 75, 100, 20), "Red Negator");
            bool blueDouble = GUI.Button(new Rect(410, 100, 100, 20), "Blue Doubler");
            bool blueNeg = GUI.Button(new Rect(515, 100, 100, 20), "Blue Negator");
            if (redLoad)
            {
                if (numRedBalls < 4)
                {
                    Instantiate(redBall);
                    numRedBalls++;
                }
                else if (numRedCyls < 4)
                {
                    Instantiate(redCyl);
                    numRedCyls++;
                }
            }
            if (blueLoad)
            {
                if (numBlueBalls < 4)
                {
                    Instantiate(blueBall);
                    numBlueBalls++;
                }
                else if (numBlueCyls < 4)
                {
                    Instantiate(blueCyl);
                    numBlueCyls++;
                }
            }
            if (redDouble)
            {
                doubler.position = redBall.position;
                if (!redDoubler)
                {
                    Instantiate(doubler);
                    redDoubler = true;
                }
            }
            if (blueDouble)
            {
                doubler.position = blueBall.position;
                if (!blueDoubler)
                {
                    Instantiate(doubler);
                    blueDoubler = true;
                }
            }
            if (redNeg)
            {
                negator.position = new Vector3(1.5f, 1.4f, 0.3f);
                if (!redNegator)
                {
                    Instantiate(negator);
                    redNegator = true;
                }
            }
            if (blueNeg)
            {
                negator.position = new Vector3(-1.5f, 1.4f, 0.3f);
                if (!blueNegator)
                {
                    Instantiate(negator);
                    blueNegator = true;
                }
            }
        }
        if (scr_.showConsole)
        {
            consoleRect_ = GUI.Window(1, consoleRect_, consoleFunction, "Console");
        }
    }

    public void gatesDown(string arg)
    {
        redGate.transform.eulerAngles = new Vector3(0, 0, 0);
        blueGate.transform.eulerAngles = new Vector3(0, 0, 0);
        redGate.transform.position = new Vector3(0.0f, 0.8241888f, 0.0f);
        blueGate.transform.position = new Vector3(-1.879837f, 0.8241888f, 0.0f);
    }

    public void Doubler(string arg)
    {
        if (arg != null)
        {
            switch (arg)
            {
                case "red":
                    doubler.position = redBall.position;
                    Instantiate(doubler);
                    break;
                case "blue":
                    doubler.position = blueBall.position;
                    Instantiate(doubler);
                    break;
            }
        }
        else
            Instantiate(doubler);
    }

    public void Negator(string arg)
    {
        if (arg != null)
        {
            switch (arg)
            {
                case "red":
                    negator.position = new Vector3(1.5f, 1.4f, 0.3f);
                    Instantiate(negator);
                    break;
                case "blue":
                    negator.position = new Vector3(-1.5f, 1.4f, 0.3f);
                    Instantiate(negator);
                    break;
            }
        }
        else
            Instantiate(negator);
    }

    public override void matchLoad(string arg)
    {
        if (arg != null)
        {
            switch (arg)
            {
                case "red-ball":
                    Instantiate(redBall);
                    break;
                case "red-cyl":
                    Instantiate(redCyl);
                    break;
                case "blue-ball":
                    Instantiate(blueBall);
                    break;
                case "blue-cyl":
                    Instantiate(blueCyl);
                    break;
            }
        }
        else
            Instantiate(redBall);
    }
}
