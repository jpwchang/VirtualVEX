var skin : GUISkin;
var gateRed : GameObject;
gateRed = GameObject.Find("redGate");
var gateBlue : GameObject;
gateBlue = GameObject.Find("blueGate");
var isRedRaised : boolean = false;
var isBlueRaised : boolean = false;
var tracker : GameObject;
tracker = GameObject.Find("Mode Tracker");
var cameraPosition: int = 2;
var cameraStrings : String[] = ["Top Left", "Top Right", "Bottom Left", "Bottom Right"];

function OnGUI()
{
	GUI.skin = skin;
	if(Application.loadedLevel == 1)
	{
		isRedRaised = !(gateRed.transform.rotation.z == 0);
		isBlueRaised = !(gateBlue.transform.rotation.z == 0);
		if(tracker != null && tracker.GetComponent("ModeTrackingScript").showGates)
		{
			var redButton : boolean = GUI.Button(Rect(150, 75, 140, 20), "Raise Red Gate [R]");
			var blueButton : boolean = GUI.Button(Rect(150, 100, 140, 20), "Raise Blue Gate [B]");
			if((redButton || Input.GetKeyDown("r")) && ! isRedRaised)
			{
				gateRed.transform.Rotate(0, 0, 90);
				gateRed.transform.Translate(0, -1.8288, 0);
			}
			if((blueButton || Input.GetKeyDown("b")) && ! isBlueRaised)
			{
				gateBlue.transform.Rotate(0, 0, 90);
				//gateBlue.transform.Translate(-0.9144, 0, -1.2);
			}
		}
	}
	
	if(tracker != null && tracker.GetComponent("ModeTrackingScript").showCC)
	{
		GUI.Box(Rect(Screen.width-220, 100, 220, 110), "Camera Position");
		cameraPosition = GUI.SelectionGrid(Rect(Screen.width-210, 140, 200, 65), cameraPosition, cameraStrings, 2);
	}
	
	switch(cameraPosition)
	{
		case 0:
		transform.eulerAngles = Vector3(11.47146, 135.0, 0.0);
		transform.position = Vector3(-3.325411, 1.811674, 3.373904);
		break;
		case 1:
		transform.eulerAngles = Vector3(11.47146, -135.0, 0.0);
		transform.position = Vector3(3.325411, 1.811674, 3.373904);
		break;
		case 2:
		transform.eulerAngles = Vector3(11.47146, 45.0, 0.0);
		transform.position = Vector3(-3.325411, 1.811674, -3.373904);
		break;
		case 3:
		transform.eulerAngles = Vector3(11.47146, -45.0, 0.0);
		transform.position = Vector3(3.325411, 1.811674, -3.373904);
		break;
		default:
		break;
	}
}