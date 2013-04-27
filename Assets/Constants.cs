using UnityEngine;
using System.Collections;

/// <summary>
/// Contains various global constants shared by two or more scripts
/// </summary>
public class Constants {
    public static readonly string abtText = "VirtualVEX v. 2.0.1\nOpen source VEX robotics competition simulator"
            + "\nProduced in collaboration with Team 254\n\nCredits:\nVirtualVEX Development Team:\nMain programming: Jonathan Chang"
            + "\nAdditional programming: Caleb Nelson\nDocumentation: Liam Hardiman\nTeam 254:\n"
            + "Programming head: Richard Lin\nVEX head: Jonathan Chang\nProgram Testers:"
            + "\nMani Gnanasivam\nKendrick Dlima\nNikhil Desai\nArt Kalb"
            + "\n\nDeveloped in Unity v." + Application.unityVersion;
#if UNITY_STANDALONE_WIN
    public static readonly string VERSION = "2.0.1";
#else
    public static readonly string VERSION = "2.0.1";
#endif
}
