using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Subclass of vvRobotBase that uses the loaded code to operate the 2013 Horizontal Roller bot
/// </summary>
public class Main_RollerBot : vvRobotBase
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject armL1;
    public GameObject armL2;
    public GameObject armL3;
    public GameObject armR1;
    public GameObject armR2;
    public GameObject armR3;
    public GameObject intake;
    public GameObject tray;

    private vvComponentCollider intakeCollider;
    private vvComponentCollider r1Collider;
    private vvComponentCollider l1Collider;

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
        intakeCollider = tray.GetComponent<vvComponentCollider>();
        r1Collider = armR1.GetComponent<vvComponentCollider>();
        l1Collider = armL1.GetComponent<vvComponentCollider>();
        robotID_ = "Roller Bot";
    }

    void FixedUpdate()
    {
        bool timeUp = owner.getTimeLimit > 0 && owner.getTimeLeft <= 0 && tracker_.GetComponent<ModeTrackingScript>().disableOnTimeUp;
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && !timeUp)
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            if (intakeCollider.isColliding || r1Collider.isColliding || l1Collider.isColliding)
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
            if (Mathf.Abs(motor[2]) > 5)
            {
                tray.rigidbody.isKinematic = false;
                if (motor[2] > 0)
                {
                    setMotor(armL1, -motor[2]);
                    setMotor(armL2, -motor[2]);
                    setMotor(armL3, -motor[2]);
                    setMotor(armR1, -motor[2]);
                    setMotor(armR2, -motor[2]);
                    setMotor(armR3, -motor[2]);
                }
            }
            else
                tray.rigidbody.isKinematic = true;
            if (Mathf.Abs(motor[3]) > 5)
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