using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Subclass of vvRobotBase that uses the loaded code to operate the Vertical Roller Bot
/// </summary>
public class Main_Dbot : vvRobotBase
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject armL1;
    public GameObject armL2;
    public GameObject armR1;
    public GameObject armR2;
    public GameObject intake;
    public GameObject holder;

    void Awake()
    {
        robotID_ = "Vertical Roller Bot";
    }

    void FixedUpdate()
    {
        bool timeUp = owner.getTimeLimit > 0 && owner.getTimeLeft <= 0 && tracker_.GetComponent<ModeTrackingScript>().disableOnTimeUp;
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && !timeUp)
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            setMotors(frontRight, backRight, motor[0]);
            setMotors(frontLeft, backLeft, motor[1]);
            if (Mathf.Abs(motor[2]) > 5)
            {
                holder.rigidbody.isKinematic = false;
                setMotor(armL1, -motor[2] / 10);
                setMotor(armL2, -motor[2] / 10);
                setMotor(armR1, -motor[2] / 10);
                setMotor(armR2, -motor[2] / 10);
            }
            else
                holder.rigidbody.isKinematic = true;
            if (Mathf.Round(motor[3]) != 0)
                intake.collider.isTrigger = true;
            else
                intake.collider.isTrigger = false;
        }

        //stop robot if time is up
        if (timeUp)
        {
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
            armL1.rigidbody.AddRelativeTorque(0, 0, 0);
            armL2.rigidbody.AddRelativeTorque(0, 0, 0);
            armR1.rigidbody.AddRelativeTorque(0, 0, 0);
            armR2.rigidbody.AddRelativeTorque(0, 0, 0);
        }
    }
}