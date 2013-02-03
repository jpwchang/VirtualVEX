using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using vvIO;

/// <summary>
/// Represents an object that has an attached Console
/// </summary>
public class vvConsoleParent : MonoBehaviour {
    protected Rect consoleRect_;
    protected string inBuffer_ = "";
    protected string outBuffer_ = "";
    protected Dictionary<string, ConsoleAction> cmdTable_;
    protected GameObject tracker;
    protected ModeTrackingScript scr_;

    protected Vector2 scrollPosition;

    /// <summary>
    /// Wrapper around a console command taking one argument, which can be null.
    /// </summary>
    /// <param name="arg">The argument to pass</param>
    protected delegate void ConsoleAction(string arg);

    public string cOutBuf
    {
        get { return outBuffer_; }
        set { outBuffer_ = value; }
    }

	// Use this for initialization
	void Start () {
        tracker = GameObject.Find("Mode Tracker");
        scr_ = tracker.GetComponent<ModeTrackingScript>();
        cmdTable_["about"] = new ConsoleAction(showAbout);
	}

    /// <summary>
    /// Shows and controls the console window. Call this in OnGUI if you are making a class that uses the Console.
    /// </summary>
    protected void consoleFunction(int windowId)
    {
        inBuffer_ = GUI.TextArea(new Rect(15, 270, 200, 20), inBuffer_, "TextField");
        bool button = GUI.Button(new Rect(220, 267.5f, 40, 25), "Go");
        if (inBuffer_.Contains("\n"))
        {
            inBuffer_ = inBuffer_.Trim();
            handleCommand(inBuffer_);
        }
        if (button)
            handleCommand(inBuffer_);
        GUI.BeginGroup(new Rect(0, 5, 390, 250));
        GUI.Box(new Rect(10, 20, 380, 230), "", "TextField");
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(380), GUILayout.Height(230), GUILayout.MinHeight(230));
        GUILayout.TextArea(outBuffer_);
        GUILayout.EndScrollView();
        GUI.EndGroup();
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    /// <summary>
    /// Evaluates a console command
    /// </summary>
    /// <param name="cmd">The command to evaluate</param>
    protected void handleCommand(string cmd)
    {
        string[] tokens = cmd.Split(new char[] { ' ' });
        try
        {
            ConsoleAction action = cmdTable_[tokens[0]];
            if (action != null)
            {
                vvConsole.print(">");
                foreach (string s in tokens)
                    vvConsole.print(s + " ");
                vvConsole.println();
                if (tokens.Length > 1)
                    action(tokens[1]);
                else
                    action(null);
            }
        }
        catch (KeyNotFoundException)
        {
            vvConsole.println("Error: " + tokens[0] + " is not a valid command");
        }
    }

    protected void showAbout(string arg)
    {
        scr_.showAbt = true;
    }
}
