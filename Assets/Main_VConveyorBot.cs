using UnityEngine;
using System.Collections;
using System.Reflection;

/// <summary>
/// Subclass of vvRobotBase that uses the loaded code to operate the Vertical ConveyorBot
/// </summary>
public class Main_VConveyorBot : vvRobotBase
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;
    public GameObject assembly;
    public GameObject stopper;

    void Awake()
    {
        robotID_ = "Vertical Conveyor Bot";
    }

    void FixedUpdate()
    {
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && (owner.getTimeLeft > 0 || owner.getTimeLimit == 0))
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            setMotors(frontRight, backRight, motor[0]);
            setMotors(frontLeft, backLeft, motor[1]);
            if (Mathf.Abs(motor[3]) > 1)
                stopper.collider.isTrigger = true;
            else
                stopper.collider.isTrigger = false;
            if (assembly.transform.localPosition.y <= 0.25 && -motor[2] < 1)
                setMotor(assembly, 0);
            else if (assembly.transform.localPosition.y >= 0.6 && -motor[2] > 1)
                setMotor(assembly, 0);
            else
                setMotor(assembly, -motor[2] / 5000, MovementMode.LINEAR_VERTICAL);
        }

        //stop robot if time is up
        if (owner.getTimeLimit > 0 && owner.getTimeLeft <= 0)
        {
            frontRight.motorTorque = 0;
            frontLeft.motorTorque = 0;
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
            assembly.transform.Translate(0, 0, 0);
        }
    }
}