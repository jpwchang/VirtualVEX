using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the camera and game-specific GUI elements
/// </summary>
public class GUIScript : MonoBehaviour {
    public GUISkin skin;
    private GameObject gateRed_;
    private GameObject gateBlue_;
    private bool isBlueRaised_ = false;
    private bool isRedRaised_ = false;
    private GameObject tracker_;
    private ModeTrackingScript trackerScript;
    private int cameraPosition_ = 2;
    private string[] cameraStrings_ = new string[] { "Top Left", "Top Right", "Bottom Left", "Bottom Right" };
    private Vector3 tlAngle_ = new Vector3(11.47146f, 135.0f, 0.0f);
    private Vector3 tlPos_ = new Vector3(-3.325411f, 1.811674f, 3.373904f);
    private Vector3 trAngle_ = new Vector3(11.47146f, -135.0f, 0.0f);
    private Vector3 trPos_ = new Vector3(3.325411f, 1.811674f, 3.373904f);
    private Vector3 blAngle_ = new Vector3(11.47146f, 45.0f, 0.0f);
    private Vector3 blPos_ = new Vector3(-3.325411f, 1.811674f, -3.373904f);
    private Vector3 brAngle_ = new Vector3(11.47146f, -45.0f, 0.0f);
    private Vector3 brPos_ = new Vector3(3.325411f, 1.811674f, -3.373904f);
    private Vector3 topAngle_ = new Vector3(90, 0, 0);
    private Vector3 topPos_ = new Vector3(0, 10, 0);

    /// <summary>
    /// Current position of the camera
    /// </summary>
    public int cameraPos
    {
        get { return cameraPosition_; }
        set { cameraPosition_ = value; }
    }

	// Use this for initialization
	void Start () {
        gateRed_ = GameObject.Find("redGate");
        gateBlue_ = GameObject.Find("blueGate");
        tracker_ = GameObject.Find("Mode Tracker");
	}

    void OnGUI()
    {
        GUI.skin = skin;
        if(tracker_ != null)
            trackerScript = tracker_.GetComponent<ModeTrackingScript>();
	    if(Application.loadedLevel == 1)
	    {
		    isRedRaised_ = !(gateRed_.transform.rotation.z == 0);
		    isBlueRaised_ = !(gateBlue_.transform.rotation.z == 0);
		    if(trackerScript != null && trackerScript.showGates)
		    {
			    bool redButton = GUI.Button(new Rect(150, 75, 140, 20), "Raise Red Gate [R]");
			    bool blueButton = GUI.Button(new Rect(150, 100, 140, 20), "Raise Blue Gate [B]");
			    if((redButton || Input.GetKeyDown("r")) && ! isRedRaised_)
			    {
				    gateRed_.transform.Rotate(0, 0, 90);
				    gateRed_.transform.Translate(0.0f, -1.8288f, 0.0f);
			    }
			    if((blueButton || Input.GetKeyDown("b")) && ! isBlueRaised_)
			    {
				    gateBlue_.transform.Rotate(0, 0, 90);
				    //gateBlue.transform.Translate(-0.9144, 0, -1.2);
			    }
		    }
	    }
	
	    if(trackerScript != null && trackerScript.showCC && trackerScript.getViewMode == ModeTrackingScript.VIEWMODE_MATCH)
	    {
		    GUI.Box(new Rect(Screen.width-220, 100, 220, 110), "Camera Position");
		    cameraPosition_ = GUI.SelectionGrid(new Rect(Screen.width-210, 140, 200, 65), cameraPosition_, cameraStrings_, 2);
	    }

        if (trackerScript.getViewMode == 0)
        {
            switch (cameraPosition_)
            {
                case 0:
                    transform.eulerAngles = tlAngle_;
                    transform.position = tlPos_;
                    break;
                case 1:
                    transform.eulerAngles = trAngle_;
                    transform.position = trPos_;
                    break;
                case 2:
                    transform.eulerAngles = blAngle_;
                    transform.position = blPos_;
                    break;
                case 3:
                    transform.eulerAngles = brAngle_;
                    transform.position = brPos_;
                    break;
                default:
                    break;
            }
        }
        else if (trackerScript.getViewMode == 1)
        {
            transform.eulerAngles = topAngle_;
            transform.position = topPos_;
        }
    }
}
