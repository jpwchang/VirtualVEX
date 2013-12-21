using UnityEngine;
using System.Collections;

/// <summary>
/// A field set up for the Toss Up game
/// </summary>
public class TUField : Field {

    public Transform bucky;
    public Transform beachball;
    public GUISkin skin;

    private GameObject redCyl_;
    private GameObject blueCyl_;
    private GameObject goalZone_;
    private GameObject midZone_;
    private Vector3 redTile1_;
    private Vector3 redTile2_;
    private Vector3 blueTile1_;
    private Vector3 blueTile2_;
    private int redMLCount_;
    private int blueMLCount_;

	// Use this for initialization
	void Start () {
        FieldInit();

        vvIO.vvConsole.setConsoleParent(this);

        Transform selectedBot = clawbot;

        switch(scr_.robotType)
        {
            case 0:
                selectedBot = clawbot;
                break;
            case 1:
                selectedBot = nz;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
        doUpdate();
	}
}
