using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Subclass of vvRobotBase that uses the loaded code to operate the Scooper Bot
/// </summary>
public class Main_ScoopBot : vvRobotBase
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject scoop;

    private vvComponentCollider scoopCollider;

    public float getLPower
    {
        get { return lMotorPowerPrev_; }
    }
    public float getRPower
    {
        get { return rMotorPowerPrev_; }
    }

    void Awake()
    {
        scoopCollider = scoop.GetComponent<vvComponentCollider>();
        robotID_ = "Scooper Bot";
    }

    void FixedUpdate()
    {
        bool timeUp = owner.getTimeLimit > 0 && owner.getTimeLeft <= 0 && tracker_.GetComponent<ModeTrackingScript>().disableOnTimeUp;
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && !timeUp)
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            if (scoopCollider.isColliding)
            {
                if ((lMotorPowerPrev_ > 0 && motor[1] > 0) || (lMotorPowerPrev_ < 0 && motor[1] < 0))
                    setMotors(frontLeft, backLeft, 0);
                else setMotors(frontLeft, backLeft, motor[1]);
                if ((rMotorPowerPrev_ > 0 && motor[0] > 0) || (rMotorPowerPrev_ < 0 && motor[0] < 0))
                    setMotors(frontRight, backRight, 0);
                else setMotors(frontRight, backRight, motor[0]);
            }
            else
            {
                setMotors(frontRight, backRight, motor[0]);
                setMotors(frontLeft, backLeft, motor[1]);
                lMotorPowerPrev_ = motor[1];
                rMotorPowerPrev_ = motor[0];
            }
            setMotor(scoop, -motor[2]/2, motor[2]);
        }

        //stop robot if time is up
        if (timeUp)
        {
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
        }
    }
}
