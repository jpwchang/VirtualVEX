using UnityEngine;
using System.Collections;

public class About : MonoBehaviour {
    private Rect windowRect;
    private bool showWindow;
    public GUISkin skin;
	// Use this for initialization
	void Start () {
        windowRect = new Rect(Screen.width/2-200, Screen.height/2-200, 400, 400);
        showWindow = false;
	}

    void OnGUI()
    {
        GUI.skin = skin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 500, 400));
        bool abt = GUI.Button(new Rect(80, 330, 50, 25), "About");
        if (abt)
            showWindow = true;
        GUI.EndGroup();
        if (showWindow)
            windowRect = GUI.Window(0, windowRect, abtFunction, "About VirtualVEX");
    }

    void abtFunction(int windowID)
    {
        string windowText = "VirtualVEX v. 1.0.0 (Alpha 12)\nOpen source VEX robotics competition simulator"
            + "\nProduced in collaboration with Team 254\n\nCredits:\nVirtualVEX Development Team:\nMain programming: Jonathan Chang"
            + "\nAdditional programming: Caleb Nelson\nDocumentation: Liam Hardiman\nTeam 254:\n"
            + "Programming head: Eric Bakan\nVEX head: Bhargava Manja\nProduct Testers:"
            + "\nMain Gnanasivam\nKendrick Dlima\nNikhil Desai\nArt Kalb"
            + "\n\nDeveloped in Unity v.3.5.0f5";
	    GUI.Label(new Rect(30, 30, Screen.width-30, Screen.height-30), windowText);
	    bool ok = GUI.Button(new Rect(30, 350, 50, 25), "OK");
	    if(ok)
		    showWindow = false;
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }
}
