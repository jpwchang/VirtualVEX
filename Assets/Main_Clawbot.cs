using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Subclass of vvRobotBase that uses the loaded code to operate the Clawbot
/// </summary>
public class Main_Clawbot : vvRobotBase
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject lift;
    public GameObject claw1;
    public GameObject claw2;

    private vvComponentCollider armCollider_;
    private float armMotorPowerPrev_;

    void Awake()
    {
        robotID_ = "Clawbot";
        armCollider_ = lift.GetComponent<vvComponentCollider>();
    }

    void FixedUpdate()
    {
        bool timeUp = owner.getTimeLimit > 0 && owner.getTimeLeft <= 0 && tracker_.GetComponent<ModeTrackingScript>().disableOnTimeUp;
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && !timeUp)
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            if (armCollider_.isColliding)
            {
                if ((lMotorPowerPrev_ > 0 && motor[1] > 0) || (lMotorPowerPrev_ < 0 && motor[1] < 0))
                    setMotors(frontLeft, backLeft, 0);
                else setMotors(frontLeft, backLeft, motor[1]/1.1f);
                if ((rMotorPowerPrev_ > 0 && motor[0] > 0) || (rMotorPowerPrev_ < 0 && motor[0] < 0))
                    setMotors(frontRight, backRight, 0);
                else setMotors(frontRight, backRight, motor[0]/1.1f);
                if((armMotorPowerPrev_ > 0 && motor[2] < 0) || (armMotorPowerPrev_ < 0 && motor[2] > 0))
                    setMotor(lift, 0);
                else 
                {
                    if(Mathf.Abs(motor[2]) > 1)
                    {
                        lift.rigidbody.isKinematic = false;
                        setMotor(lift, -motor[2]);
                    }
                    else
                        lift.rigidbody.isKinematic = true;
                }
            }
            else
            {
                setMotors(frontRight, backRight, motor[0]/1.1f);
                setMotors(frontLeft, backLeft, motor[1]/1.1f);
                if (Mathf.Abs(motor[2]) > 1)
                {
                    lift.rigidbody.isKinematic = false;
                    setMotor(lift, -motor[2]);
                }
                else
                    lift.rigidbody.isKinematic = true;
                lMotorPowerPrev_ = motor[1];
                rMotorPowerPrev_ = motor[0];
                armMotorPowerPrev_ = motor[2];
            }

            setMotor(claw1, motor[3] * 6, MovementMode.DIRECT_SIDEWAYS);
            setMotor(claw2, -motor[3] * 6, MovementMode.DIRECT_SIDEWAYS);
        }

        //stop robot if time is up
        if (timeUp)
        {
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
            lift.transform.Translate(0, 0, 0);
            claw1.rigidbody.AddRelativeTorque(0, 0, 0);
            claw2.rigidbody.AddRelativeTorque(0, 0, 0);
        }
    }
}