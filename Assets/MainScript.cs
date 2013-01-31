using UnityEngine;
using System.Collections;

/*
 * This is the main script file. All robot code can be entered here.
 * The pre_auton, auton, and driver_control functions mirror their ROBOTC counterparts
 * and accept ROBOTC syntax. However, one key difference is that the while loop should NOT be 
 * included in driver control as Unity's Update function occurs in its own control loop.
 */

//class to emulate the VEX motor module
class Motor
{
    float gearRatio = 1;
    private string type = "";
    private GameObject attached;
    private WheelCollider wheel;
    private int dir;
    public void setDir(int v) { dir = v; }
    public void setType(string v) { type = v; }
    public string getType() { return type; }
    public void setAttached(GameObject o) { attached = o; }
    public void setAttached(WheelCollider w) { wheel = w; }
    public string id() { return attached.name; }
    public void convertMotor(float v)
    {
        if (type == "drive")
        {
            wheel.motorTorque = dir * gearRatio * (v / 2);
            wheel.brakeTorque = gearRatio * (float)63.5 - Mathf.Abs(wheel.motorTorque);
        }
        else if (type == "jointVert")
            attached.rigidbody.AddRelativeTorque(dir * v/10, 0, 0);
        else if (type == "jointHoriz")
            attached.rigidbody.AddRelativeTorque(0, dir * v/10, 0);
        else
            MonoBehaviour.print("A fatal error occured");
    }
}
public class MainScript : MonoBehaviour {
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public GameObject arm1;
    public GameObject arm2;
    public GameObject claw1;
    public GameObject claw2;
    JointMotor armMotor1;
	//start declarations for VEX / ROBOTC emulation constants
	const int port1 = 0;
	const int port2 = 1;
	const int port3 = 2;
	const int port4 = 3;
	const int port5 = 4;
	const int port6 = 5;
	const int port7 = 6;
	const int port8 = 7;
	const int port9 = 8;
	const int port10 = 9;
	const int Ch2 = 1;
	const int Ch3 = 2;
    const int Ch2Xmtr2 = 3;
    const int Ch3Xmtr2 = 4;
	float[] motor = new float[10];
	float[] vexRT = new float[16];
    Motor[] motorConfig = new Motor[10];
	//end declarations

    void config(int port, string v, GameObject o, int d)
    {
        motorConfig[port].setType(v);
        motorConfig[port].setAttached(o);
        motorConfig[port].setDir(d);
    }
    void config(int port, string v, WheelCollider w, int d)
    {
        motorConfig[port].setType(v);
        motorConfig[port].setAttached(w);
        motorConfig[port].setDir(d);
    }
	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
            motorConfig[i] = new Motor();
        config(port1, "drive", frontRight, 1);
        config(port2, "drive", frontLeft, 1);
        config(port3, "jointVert", arm1, 1);
        config(port4, "jointHoriz", claw1, 1);
        config(port5, "jointVert", arm2, 1);
        config(port6, "jointHoriz", claw2, -1);
	}
	//Driver control code goes here
	void driver_control()
	{
        motor[port1] = vexRT[Ch2];
        motor[port2] = vexRT[Ch3];
        motor[port3] = vexRT[Ch2Xmtr2];
        motor[port4] = vexRT[Ch3Xmtr2];
        motor[port5] = vexRT[Ch2Xmtr2];
        motor[port6] = vexRT[Ch3Xmtr2];
	}
    
	// Update is called once per frame
	void Update () {
        //Contains interpreter code to translate ROBOTC syntax to UnityScript syntax.
        //May need to be altered to work with different control setups.
        vexRT[Ch2] = Input.GetAxis("Horizontal") * 127;
        vexRT[Ch3] = Input.GetAxis("Vertical") * 127;
        vexRT[Ch2Xmtr2] = Input.GetAxis("Fire1") * 127;
        vexRT[Ch3Xmtr2] = Input.GetAxis("Fire2") * 127;
        driver_control();
        for (int i = 0; i < 6; i++)
        {
            motorConfig[i].convertMotor(motor[i]);
        }
        /*
        frontRight.motorTorque = gearRatio * (motor[port1] / 2);
        frontLeft.motorTorque = gearRatio * (motor[port2] / 2);
        frontRight.brakeTorque = (gearRatio * (float)63.5) - Mathf.Abs(frontRight.motorTorque);
        frontLeft.brakeTorque = (gearRatio * (float)63.5) - Mathf.Abs(frontLeft.motorTorque);
        arm1.rigidbody.AddRelativeTorque(motor[port3]/10, 0, 0);
        arm2.rigidbody.AddRelativeTorque(motor[port3]/10, 0, 0);
        claw1.rigidbody.AddRelativeTorque(0, motor[port4], 0);
        claw2.rigidbody.AddRelativeTorque(0, -motor[port4], 0);
         * */
	}
}
