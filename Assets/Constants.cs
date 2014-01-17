using UnityEngine;
using System.Collections;

/// <summary>
/// Contains various global constants shared by two or more scripts
/// </summary>
public class Constants {
#if UNITY_STANDALONE_WIN
    public static readonly string VERSION = "3.0 Beta";
#else
    public static readonly string VERSION = "3.0 Beta (Linux64)";
#endif
    public static readonly string abtText = "VirtualVEX v. " + VERSION + "\nOpen source VEX robotics competition simulator"
            + "\n\n<size=14>Credits:</size>\n<b>VirtualVEX Development Team:</b>\nMain programming: Jonathan Chang"
            + "\nAdditional programming: Caleb Nelson\nDocumentation: Liam Hardiman\n"
            + "<b>Program Testers:</b>"
            + "\nMani Gnanasivam\nKendrick Dlima\nNikhil Desai\nArt Kalb"
            + "\n\nDeveloped in Unity v." + Application.unityVersion;
    public static readonly string DESC_SOLO = "Description: Solo Untimed mode provides a standard competition field and disables the timer, " +
        "allowing you to practice for as long as you want. This mode is useful for general driver practice and training.";
    public static readonly string DESC_SKILLS = "Description: Robot Skills mode replicates the conditions of an actual Robot Skills match."+
        "The timer is set to one minute and the robot will be disabled when time is up. This mode is useful for practicing preplanned skills routines.";
}
