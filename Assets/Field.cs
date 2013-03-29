using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using vvIO;

/// <summary>
/// Generic VEX field. This class can be inherited to create specific fields for each game.
/// </summary>
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
    public Transform shovel;
    protected Object robot_;
    protected bool useTimer_;
    protected float timeLimit_;
    protected float timeLeft_;
    protected float actualTimeLimit_;

    /// <summary>
    /// Get the robot currently attached to the field
    /// </summary>
    public Object getRobot
    {
        get { return robot_; }
    }

    /// <summary>
    /// Get the time limit for the currently loaded match
    /// </summary>
    public float getTimeLimit
    {
        get
        {
            return actualTimeLimit_;
        }
    }

    /// <summary>
    /// Get the time remaining for the currently loaded match
    /// </summary>
    public float getTimeLeft
    {
        get
        {
            return timeLeft_;
        }
    }

    /// <summary>
    /// Overload this function to provide match loading capablities for the game
    /// </summary>
    /// <param name="arg"></param>
    public virtual void matchLoad(string arg) { return; }

	// Use this for initialization
	protected void FieldInit () {
        consoleRect_ = new Rect(Screen.width - 420, 210, 400, 300);
        tracker = GameObject.Find("Mode Tracker");
        scr_ = tracker.GetComponent<ModeTrackingScript>();
        timeLimit_ = tracker.GetComponent<ModeTrackingScript>().timeLimit;
        timeLeft_ = timeLimit_;
        actualTimeLimit_ = timeLimit_;
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
        cmdTable_["set-waypoint"] = new ConsoleAction(waypointSet);

        outBuffer_ = "VirtualVEX, Version 2.0.0 (Beta)\n"
            + "This is a development build. Some features may be unstable.\n"
            + "Console initialized\n\n";
	}

    /// <summary>
    /// Updates the remaining time
    /// </summary>
    protected void doUpdate()
    {
        if (scr_.getViewMode == ModeTrackingScript.VIEWMODE_STRATEGIC && timeLimit_ > 0 && timeLeft_ > 0)
            timeLimit_ += Time.deltaTime;
        if (useTimer_ && timeLimit_ > 0 && timeLeft_ > 0) //How much time is left in the match?
        {
            timeLeft_ = timeLimit_ - Time.timeSinceLevelLoad;
        }
    }

    /// <summary>
    /// Reset the state of the currently loaded match
    /// </summary>
    /// <param name="arg"></param>
    public void reset(string arg)
    {
        int index = Application.loadedLevel;
        Application.LoadLevel(index);
    }
    
    /// <summary>
    /// Pause the simulation
    /// </summary>
    /// <param name="arg"></param>
    public void pause(string arg)
    {
        Time.timeScale = 0;
        vvConsole.println("Simulation paused");
    }

    /// <summary>
    /// Resume the simulation, if it is paused
    /// </summary>
    /// <param name="arg"></param>
    public void unpause(string arg)
    {
        Time.timeScale = 1;
        vvConsole.println("Resuming simulation...");
    }

    /// <summary>
    /// Remove the time limit
    /// </summary>
    /// <param name="arg"></param>
    public void notime(string arg)
    {
        useTimer_ = false;
        vvConsole.println("Success: timer is no longer in effect");
    }
    
    /// <summary>
    /// Destroy the robot.
    /// </summary>
    /// <param name="arg"></param>
    protected void destroy(string arg)
    {
        Object.Destroy((robot_ as Transform).gameObject);
        vvConsole.println("Destroying robot");
    }

    /// <summary>
    /// Changes the time limit for the currently loaded match.
    /// Please note that time elapsed will NOT be reset.
    /// </summary>
    /// <param name="arg">The new time limit</param>
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

    /// <summary>
    /// Change the currently loaded robot to a different type.
    /// Match will not be reset, but the robot will be
    /// </summary>
    /// <param name="arg">The new robot</param>
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
                case "roller":
                    robot_ = Instantiate(zackbot);
                    break;
                case "shovel":
                    robot_ = Instantiate(shovel);
                    break;
                default:
                    vvConsole.println("No argument specified. Defaulting to clawbot.");
                    robot_ = Instantiate(clawbot);
                    break;
            }
        }
    }

    /// <summary>
    /// Return the current status of the simulation
    /// </summary>
    /// <param name="arg">Flags</param>
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

    protected void waypointSet(string arg)
    {
        if (arg == null)
            vvConsole.println("ERROR: No argument provided. USAGE: set-waypoint <index>");
        else
        {
            int index = 1;
            try
            {
                index = System.Int32.Parse(arg);
            }
            catch (System.FormatException)
            {
                index = 1;
            }
            if (scr_.setIndex(index))
                vvConsole.println("Successfully set current waypoint " + arg);
            else
                vvConsole.println("ERROR: Index outside current waypoint count");
        }
    }
}
