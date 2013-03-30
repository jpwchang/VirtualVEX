using UnityEngine;
using System.Collections;

public class Main_ShovelBot : vvRobotBase {
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
    public GameObject anchor;
    public GameObject scoop;

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
        r1Collider = armR1.GetComponent<vvComponentCollider>();
        l1Collider = armL1.GetComponent<vvComponentCollider>();
        robotID_ = "Scooper Bot";
    }

    void FixedUpdate()
    {
        //The robot should only move if the round hasn't ended
        //The only exeption is in solo untimed, which has no end.
        if (complete_ && (owner.getTimeLeft > 0 || owner.getTimeLimit == 0))
        {
            //Apply operator inputs to the proper mechanisms on the robot.
            if (r1Collider.isColliding || l1Collider.isColliding)
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
                anchor.rigidbody.isKinematic = false;
                if (motor[2] > 0)
                {
                    setMotor(armL1, -motor[2]/1.2f);
                    setMotor(armL2, -motor[2]/1.2f);
                    setMotor(armL3, -motor[2]/1.2f);
                    setMotor(armR1, -motor[2]/1.2f);
                    setMotor(armR2, -motor[2]/1.2f);
                    setMotor(armR3, -motor[2]/1.2f);
                }
            }
            else
                anchor.rigidbody.isKinematic = true;
            if (Mathf.Abs(motor[3]) < 5 && Mathf.Abs(motor[2]) < 5)
                scoop.rigidbody.isKinematic = true;
            else
                scoop.rigidbody.isKinematic = false;
            setMotor(scoop, motor[3]/10);
        }

        //stop robot if time is up
        if (owner.getTimeLimit > 0 && owner.getTimeLeft <= 0)
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
