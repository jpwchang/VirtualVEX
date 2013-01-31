var tex : Texture;
var tracker : GameObject;
var skin : GUISkin;
var str : String = "Loading News...";
var toolbar = 0;
var www : WWW;
var toolStrs : String[] = ["Clawbot", "ConveyorBot", "H. Roller", "V. ConveyorBot"];

function Start()
{
	www = WWW("https://sites.google.com/site/virtualvex/project-updates/onelastrally/Update.txt");
	yield www;
	str = www.text;
}
function OnGUI () {
	GUI.skin = skin;
	GUI.Box(Rect(0, 0, Screen.width, Screen.height), tex);
	GUI.BeginGroup(Rect(Screen.width/2-40, Screen.height/2-145, 400, 370));
	GUI.Box(Rect(0, 50, 400, 320), "Latest News");
	if(www.isDone)
		GUI.Box(Rect(10, 70, 380, 290), str);
	else
		GUI.Box(Rect(10, 70, 380, 290), "Loading News...");
	GUI.EndGroup();
	GUI.BeginGroup(Rect(Screen.width/2 - 200, Screen.height/2 - 150 , 500, 400));
	GUI.Box(Rect(0, 0, 500, 55), "Robot Type");
	toolbar = GUI.Toolbar(Rect(5, 20, 450, 30), toolbar, toolStrs);
	GUI.Box(Rect(0, 55, 160, 320), "New Simulation");
	var solo : boolean = GUI.Button(Rect(20, 110, 120, 30), "Solo Untimed");
	var roboSkills : boolean = GUI.Button(Rect(20, 150, 120, 30), "Robot Skills");
	var exit : boolean = GUI.Button(Rect(20, 330, 50, 25), "Exit");
	tracker.GetComponent("ModeTrackingScript").robotType=toolbar;
	if(solo)
	{
		tracker.GetComponent("ModeTrackingScript").mode = 0;
		Application.LoadLevel(1);
	}
	if(roboSkills)
	{
		tracker.GetComponent("ModeTrackingScript").mode = 1;
		Application.LoadLevel(2);
	}
	GUI.EndGroup();
	if(exit)
	Application.Quit();
}