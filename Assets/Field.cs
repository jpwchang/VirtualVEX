using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using vvIO;

public class Field : vvConsoleParent {
    public int redScore = 0;
    public int blueScore = 0;
    public int redTotal = 0;
    public int blueTotal = 0;
    public Transform clawbot;
    public Transform scoop;
    public Transform conv;
    public Transform nz;
    public Transform vertical;
    public Transform dbot;
    public Transform zackbot;
    protected Object robot_;
    protected bool useTimer_;
    protected float timeLimit_;
    protected float timeLeft_;

    public Object getRobot
    {
        get { return robot_; }
    }

    public float getTimeLimit
    {
        get
        {
            return timeLimit_;
        }
    }

    public float getTimeLeft
    {
        get
        {
            return timeLeft_;
        }
    }

    public virtual void matchLoad(string arg) { return; }

	// Use this for initialization
	protected void FieldInit () {
        consoleRect_ = new Rect(Screen.width - 420, 210, 400, 300);
        tracker = GameObject.Find("Mode Tracker");
        scr_ = tracker.GetComponent<ModeTrackingScript>();
        timeLimit_ = tracker.GetComponent<ModeTrackingScript>().timeLimit;
        timeLeft_ = timeLimit_;
        useTimer_ = true;
        cmdTable_ = new Dictionary<string, ConsoleAction>();

        cmdTable_["reset"] = new ConsoleAction(reset);
        cmdTable_["pause"] = new ConsoleAction(pause);
        cmdTable_["unpause"] = new ConsoleAction(unpause);
        cmdTable_["no-time"] = new ConsoleAction(notime);
        cmdTable_["destroy"] = new ConsoleAction(destroy);
        cmdTable_["set-time-limit"] = new ConsoleAction(setTimeLimit);
        cmdTable_["about"] = new ConsoleAction(showAbout);
        cmdTable_["change-bot"] = new ConsoleAction(changeBot);
        cmdTable_["status"] = new ConsoleAction(status);

        outBuffer_ = "VirtualVEX, Version 2.0.0 Beta\n"
            + "This is a development build.\n"
            + "Console initialized\n\n";
	}

    protected void doUpdate()
    {
        if (useTimer_ && timeLimit_ > 0 && timeLeft_ > 0) //How much time is left in the match?
        {
            timeLeft_ = timeLimit_ - Time.timeSinceLevelLoad;
        }
    }

    public void reset(string arg)
    {
        int index = Application.loadedLevel;
        Application.LoadLevel(index);
    }
    
    public void pause(string arg)
    {
        Time.timeScale = 0;
        vvConsole.println("Simulation paused");
    }

    public void unpause(string arg)
    {
        Time.timeScale = 1;
        vvConsole.println("Resuming simulation...");
    }

    public void notime(string arg)
    {
        useTimer_ = false;
        vvConsole.println("Success: timer is no longer in effect");
    }
    
    protected void destroy(string arg)
    {
        Object.Destroy((robot_ as Transform).gameObject);
        vvConsole.println("Destroying robot");
    }

    protected void setTimeLimit(string arg)
    {
        if (arg == null)
        {
            vvConsole.println("No argument found when int was expected. Defaulting to 0.");
            arg = "0";
        }
        try
        {
            int limit = System.Int32.Parse(arg);
            timeLimit_ = limit;
            timeLeft_ = timeLimit_ - Time.timeSinceLevelLoad;
            vvConsole.println("Success: time limit is now " + arg);
        }
        catch (System.FormatException)
        {
            vvConsole.println("Caught bad time value: " + arg);
        }
    }

    protected void changeBot(string arg)
    {
        if (arg != null)
        {
            Object.Destroy((robot_ as Transform).gameObject);
            switch (arg)
            {
                case "claw":
                    robot_ = Instantiate(clawbot);
                    break;
                case "conveyor":
                    robot_ = Instantiate(conv);
                    break;
                case "h-roller":
                    robot_ = Instantiate(nz);
                    break;
                case "v-conveyor":
                    robot_ = Instantiate(vertical);
                    break;
                case "v-roller":
                    robot_ = Instantiate(dbot);
                    break;
                case "scooper":
                    robot_ = Instantiate(scoop);
                    break;
                default:
                    vvConsole.println("No argument specified. Defaulting to clawbot.");
                    robot_ = Instantiate(clawbot);
                    break;
            }
        }
    }

    protected void status(string arg)
    {
        vvConsole.println("Loaded scene name: " + Application.loadedLevelName);
        vvConsole.println("Platform name: " + Application.platform);
        vvConsole.println("Program resolution: " + Screen.width + "x" + Screen.height);
        vvConsole.println("Total program uptime: " + Time.time + "s");
        vvConsole.println("Simulation uptime: " + Time.timeSinceLevelLoad + "s");
        vvConsole.println("Graphics quality: " + QualitySettings.GetQualityLevel() + "/" + QualitySettings.names.Length);
        vvConsole.println("Current Framerate: " + 1 / Time.deltaTime);
        vvConsole.println("Number of connected joysticks: " + Input.GetJoystickNames().Length);
        if (arg != null && arg.Equals("-verbose"))
        {
            vvConsole.println("Loaded scene index: " + Application.loadedLevel);
            vvConsole.println("Unity player version: " + Application.unityVersion);
            vvConsole.println("Host system info:");
            vvConsole.println("\tOperating system: " + SystemInfo.operatingSystem);
            vvConsole.println("\tProcessor: " + SystemInfo.processorType);
            vvConsole.println("\tAvailable processors: " + SystemInfo.processorCount);
            vvConsole.println("\tGraphics processor Name: " + SystemInfo.graphicsDeviceName);
            vvConsole.println("\tGraphics processor ID: " + SystemInfo.graphicsDeviceID);
            foreach (string s in Input.GetJoystickNames())
                vvConsole.println("Found joystick: " + s);
        }
    }
}
