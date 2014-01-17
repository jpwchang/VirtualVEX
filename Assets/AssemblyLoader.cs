using UnityEngine;
using System.Collections;
using System.Reflection;
using vvIO;
using System;

//This file uses code originally written by Emil Johanssen ("AngryAnt") and made available on the public domain.

/// <summary>
/// Specifies directions for motor power to be applied
/// </summary>
public enum MovementMode
{
    DIRECT_VERTICAL,
    DIRECT_HORIZONTAL,
    DIRECT_SIDEWAYS,
    LINEAR_VERTICAL,
    LINEAR_HORIZONTAL,
    LINEAR_SIDEWAYS
}

/// <summary>
/// Base class for all robots in VirtualVEX. vvRobotBase handles the loading of user code
/// from a precompiled DLL file. This loaded code is used in the control of all inherited classes.
/// Other functions common to all robots are also implemented in this base class. These include
/// a timer function that keeps track of the current time and, if applicable, stops the robot
/// at the time limit. The Physics Window and User Code window are also implemented here.
/// vvRobotBase also contains definitions for the VirtualVEX developer API functions. These
/// functions allow developers to easily configure their own robots.
/// </summary>
public class vvRobotBase : MonoBehaviour
{
    protected string assemblyURL_;
    protected string ErrorString_ = "";
    protected WWW WWW_;
    protected bool complete_ = true;
    protected bool retried_ = false;
    public GameObject timer;
    protected float[] motor;
    protected float[] SensorValue;
    public GUISkin skin;
    private float pastVelocity_ = 0;
    private float acceleration_ = 0;
    protected string text_;
    protected string robotID_ = "ERROR";
    protected GameObject tracker_;
    private Rect physWinRect_;
    private bool physWinFirstShow_ = true;
    private bool useTimer_;
    protected Field owner;
    protected float lMotorPowerPrev_;
    protected float rMotorPowerPrev_;
    protected const int DIRECT_VERTICAL = 0;
    protected const int DIRECT_HORIZONTAL = 1;
    protected const int DIRECT_SIDEWAYS = 2;
    protected const int LINEAR_VERTICAL = 3;
    protected const int LINEAR_HORIZONTAL = 4;
    protected const int LINEAR_SIDEWAYS = 5;

    //*************************************************************************************************
    //Public methods
    //These methods can be called by custom robot classes you create.

    /// <summary>
    /// Assign a given power level to a wheel.
    /// This allows you to move the robot's drive base.
    /// A common use for this method is in driver control, passing in a joystick axis
    /// as the power level.
    /// This method can only be used on wheel colliders (usually found on drivebases).
    /// For a version that applies to other motor placements, see the overloaded version:
    /// setMotor(GameObject, float, int).
    /// </summary>
    /// <param name="motor">Which motor to apply this to</param>
    /// <param name="powerLevel">The power to supply to this motor</param>
    public void setMotor(WheelCollider motor, float powerLevel)
    {
        motor.motorTorque = powerLevel;
        motor.brakeTorque = 127 - Mathf.Abs(motor.motorTorque);
    }

    /// <summary>
    /// Assign a given power level to a motor on the virtual robot.
    /// This allows components of your robot other than the drivebase to move.
    /// This method is designed for moving parts that do not have an actual simulated motor.
    /// One example of such a part is a linear slider.
    /// This method simulates motor power by directly adding a force to the given component, or GameObject.
    /// As a result, you must specify a movement type. This tells the program what
    /// direction the component will be moving in and whether the motion is linear or rotational.
    /// There are 3 available movement types for linear motion:
    /// <para>LINEAR_VERTICAL: Straight up and down.</para>
    /// <para>LINEAR_HORIZONTAL: Straight forward and backward.</para>
    /// <para>LINEAR_SIDEWAYS: Straight left and right.</para>
    /// For compatibility reasons, this method also enables angular motion for arms. However, this method
    /// of angular motion is obsolete and the hinge joint version of setMotor should be used instead. For
    /// compatibility, the following angulat movement types are allowed:
    /// <para>DIRECT_VERTICAL: Rotational, up and down, facing forward (or backward). Perfect for arms.</para>
    /// <para>DIRECT_HORIZONTAL: Rotational, side to side. Perfect for claws.</para>
    /// <para>DIRECT_SIDEWAYS: Rotational, up and down, facing left or right.</para>
    /// All directions are relative to the robot. The default direction is DIRECT_VERTICAL.
    /// </summary>
    /// <param name="motor">Which motor to apply this to</param>
    /// <param name="powerLevel">The power to supply to this motor</param>
    /// <param name="applyMode">The direction this power should be applied in</param>
    public void setMotor(GameObject motor, float powerLevel, MovementMode applyMode = MovementMode.DIRECT_VERTICAL)
    {
        switch (applyMode)
        {
            case MovementMode.DIRECT_VERTICAL:
                motor.rigidbody.AddRelativeTorque(powerLevel, 0, 0);
                break;
            case MovementMode.DIRECT_HORIZONTAL:
                motor.rigidbody.AddRelativeTorque(0, powerLevel, 0);
                break;
            case MovementMode.DIRECT_SIDEWAYS:
                motor.rigidbody.AddRelativeTorque(0, 0, powerLevel);
                break;
            case MovementMode.LINEAR_VERTICAL:
                motor.transform.Translate(0, 0, powerLevel);
                break;
            case MovementMode.LINEAR_HORIZONTAL:
                motor.transform.Translate(0, powerLevel, 0);
                break;
            case MovementMode.LINEAR_SIDEWAYS:
                motor.transform.Translate(powerLevel, 0, 0);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Assign a given power level to a motor on the virtual robot.
    /// This allows components of your robot other than the drivebase to move.
    /// This version of setMotor is designed for parts that rotate, such as arms and claws.
    /// The part must have a HingeJoint component (representing the motor axle) for this method to work.
    /// The orientation of the HingeJoint automatically determines the axis of rotation.
    /// </summary>
    /// <param name="motor">Which motor to apply this to</param>
    /// <param name="velocity">The motor's target velocity</param>
    /// <param name="powerLevel">How much power the motor gets</param>
    public void setMotor(GameObject motor, float velocity, float powerLevel)
    {
        if (velocity == 0.0f)
            motor.hingeJoint.useMotor = false;
        else
        {
            JointMotor hjMotor = motor.hingeJoint.motor;
            hjMotor.targetVelocity = velocity;
            hjMotor.force = 100;
            motor.hingeJoint.motor = hjMotor;
        }
    }

    /// <summary>
    /// Set two drive motors to the same power level.
    /// Useful for 4-wheel drivebases.
    /// </summary>
    /// <param name="motor1">The first affected motor</param>
    /// <param name="motor2">The second affected motor</param>
    /// <param name="powerLevel">The power to supply to both motors</param>
    public void setMotors(WheelCollider motor1, WheelCollider motor2, float powerLevel)
    {
        setMotor(motor1, powerLevel);
        setMotor(motor2, powerLevel);
    }

    /// <summary>
    /// Returns the motor power array
    /// </summary>
    public float[] getMotorValue
    {
        get { return motor; }
    }

    //End of public methods
    //*************************************************************************************************

    //*************************************************************************************************
    //Unity framework methods
    //Do NOT edit the following methods unless you know what you're doing!
    //They are required by the application for interpreting the ROBOTC code.
    public void Start()
    {
        timer = GameObject.Find("Main Camera");
        tracker_ = GameObject.Find("Mode Tracker");
        tracker_.GetComponent<ModeTrackingScript>().timekeeper = timer;
        owner = GameObject.FindGameObjectWithTag("field").GetComponent<Field>();
        motor = new float[10];
        useTimer_ = true;
        physWinRect_ = new Rect(Screen.width - 320, 300, 300, 150);
#if UNITY_STANDALONE_WIN
        assemblyURL_ = "file://" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VirtualVEX\\main.dll";
#else
        assemblyURL_ = "file://" + Application.dataPath + "/main.dll";
#endif
        SensorValue = new float[20];
        if (assemblyURL_ != "")
        {
            ReloadAssembly(assemblyURL_);
        }
    }

    public void Update()
    {
        if (!complete_)
        {
            vvConsole.println("Initializing robot: " + robotID_);
            if (WWW_.error != null && !retried_)
            {
                vvConsole.println("Failed to locate user code assembly in standard location. Now retrying in application data folder. If you are running portable VirtualVEX, ignore this message.");
                retried_ = true;
                assemblyURL_ = "file://" + Application.dataPath + "\\main.dll";
                ReloadAssembly(assemblyURL_);
            }

            else if (WWW_.error != null)
            {
                ErrorString_ = WWW_.error;
                complete_ = true;
                SendMessage("OnAssemblyLoadFailed", assemblyURL_);
            }

            else if (WWW_.isDone)
            {
                Assembly assembly = LoadAssembly();
                complete_ = true;
                if (assembly != null)
                {
                    Debug.Log("Done");
                    SendMessage("OnAssemblyLoaded", new WWWAssembly(assemblyURL_, assembly));
                    vvConsole.println("Successfully loaded user code.");
                    vvConsole.println("User code location is: " + assemblyURL_);
                }
                else
                {
                    Debug.Log("Failed");
                    SendMessage("OnAssemblyLoadFailed", assemblyURL_);
                }
            }
        }
        else
        {
            //Load the user code
            Assembly a = LoadAssembly();

            System.Type sourceType = a.GetType("source");
            System.Type type = sourceType.BaseType;

            //Retrieve the controller values from the code
            FieldInfo rtData = type.GetField("vexRT");
            float[] vexrt = (rtData.GetValue(null) as float[]);
            //retrieve the setRTValue function so we can call it
            MethodInfo setrt = type.GetMethod("setRTValue");
            //retrieve the setSensorValue function so we can call it
            MethodInfo setSensorValue = type.GetMethod("setSensorValue");

            //Joysticks detected - use joystick input
            if (Input.GetJoystickNames().Length > 0)
            {
                float f1 = Mathf.Abs(Input.GetAxis("Fire1")) < 0.1f ? 0 : Input.GetAxis("Fire1");
                float f2 = Mathf.Abs(Input.GetAxis("Fire2")) < 0.1f ? 0 : Input.GetAxis("Fire2");

                setrt.Invoke(null, new object[] { 1, Input.GetAxis("Horizontal") * 127 });
                setrt.Invoke(null, new object[] { 2, Input.GetAxis("Vertical") * 127 });
                setrt.Invoke(null, new object[] { 3, f1 * 127 });
                setrt.Invoke(null, new object[] { 4, f2 * 127 });
                setrt.Invoke(null, new object[] { 5, Input.GetAxis("Fire3") });
                setrt.Invoke(null, new object[] { 6, Input.GetAxis("Jump") });
                setrt.Invoke(null, new object[] { 7, Input.GetAxis("Btn6U") });
                setrt.Invoke(null, new object[] { 8, Input.GetAxis("Btn6D") });
                setrt.Invoke(null, new object[] { 9, Input.GetAxis("Btn5U2") });
                setrt.Invoke(null, new object[] { 10, Input.GetAxis("Btn5D2") });
                setrt.Invoke(null, new object[] { 11, Input.GetAxis("Btn6U2") });
                setrt.Invoke(null, new object[] { 12, Input.GetAxis("Btn6D2") });
            }
            //No joysticks detected - allow keyboard input
            else
            {
                setrt.Invoke(null, new object[] { 1, Input.GetAxis("AltHorizontal") * 127 });
                setrt.Invoke(null, new object[] { 2, Input.GetAxis("AltVertical") * 127 });
                setrt.Invoke(null, new object[] { 3, Input.GetAxis("AltFire1") * 127 });
                setrt.Invoke(null, new object[] { 4, Input.GetAxis("AltFire2") * 127 });
            }
            //start driver control
            MethodInfo m = sourceType.GetMethod("driver_control");
            m.Invoke(null, null); //call the user code's driver_control function
            //Get the motor values from the code
            FieldInfo motorData = type.GetField("motor");
            motor = (motorData.GetValue(null) as float[]);
            updateSensors(1.0f);
            //Update the robot's local copy of sensor values
            setSensorValue.Invoke(null, new object[] { SensorValue });

            text_ = "Time Remaining: " + (int)owner.getTimeLeft;
            acceleration_ = (rigidbody.velocity.magnitude - pastVelocity_) / Time.fixedDeltaTime;
            pastVelocity_ = rigidbody.velocity.magnitude;
        }
    }

    private void updateSensors(float step)
    {
        //update sensor values
        if (motor[0] > 0.5f)
            SensorValue[0] += step;
        else if (motor[0] < -0.5f)
            SensorValue[0] -= step;
        if (motor[1] > 0.5f)
            SensorValue[2] += step;
        else if (motor[1] < -0.5f)
            SensorValue[2] -= step;
        if (motor[2] > 0.5f)
            SensorValue[4] += step;
        else if (motor[2] < 0.5f)
            SensorValue[4] -= step;
    }

    void OnGUI()
    {
        GUI.skin = null;
        if (tracker_ != null)
        {
            ModeTrackingScript mts = tracker_.GetComponent<ModeTrackingScript>();
            //Display remaining time
            if (mts.showTimer)
                GUI.Box(new Rect(Screen.width - 170, 20, 150, 50), text_);
            GUI.skin = skin;
            if (mts.showPhysics)
            {
                physWinRect_ = GUI.Window(0, physWinRect_, physWinFunc, "Physics Analysis Window");
                if (physWinFirstShow_)
                {
                    GUI.BringWindowToFront(0);
                    physWinFirstShow_ = false;
                }
            }
            else
                physWinFirstShow_ = true;
        }
    }

    void physWinFunc(int windowId)
    {
        string content = "Coordinates: " + this.rigidbody.position + "\n"
            + "Velocity Vector: (" + System.String.Format("{0:0.###}", this.rigidbody.velocity.x) + ", "
            + System.String.Format("{0:0.###}", this.rigidbody.velocity.z) + ")\n"
            + "Velocity Magnitude: " + System.String.Format("{0:0.###}", this.rigidbody.velocity.magnitude) + " m/s\n"
            + "Angular Velocity: " + System.String.Format("{0:0.###}", this.rigidbody.angularVelocity.magnitude) + "\n"
            + "Acceleration: " + System.String.Format("{0:0.##}", acceleration_) + "m/s2\n"
            + "Current Momentum: " + System.String.Format("{0:0.###}", (this.rigidbody.velocity.magnitude * this.rigidbody.mass));
        GUI.Label(new Rect(15, 30, Screen.width - 15, Screen.height - 30), content);
        GUI.DragWindow(new Rect(0, 0, 10000, 20));
    }

    public string AssemblyURL
    {
        get
        {
            return assemblyURL_;
        }
        set
        {
            if (assemblyURL_ != value)
            {
                ReloadAssembly(value);
            }
        }
    }

    public float Progress
    {
        get
        {
            return complete_ ? 1.0f : WWW_.progress;
        }
    }

    public string Error
    {
        get
        {
            return ErrorString_;
        }
    }

    public void ReloadAssembly(string url)
    {
        complete_ = false;
        ErrorString_ = "";
        assemblyURL_ = url;
        WWW_ = new WWW(assemblyURL_);
    }
    protected Assembly LoadAssembly()
    {
        try
        {
            return Assembly.Load(WWW_.bytes);
        }
        catch (System.Exception e)
        {
            ErrorString_ = e.ToString();
            return null;
        }
    }
    //End of Unity framework methods
    //**********************************************************************************************
}

/// <summary>
/// Class to represent the Assembly. Originally written by Emil Johanssen.
/// </summary>
public class WWWAssembly
{
    private string m_URL;
    private Assembly m_Assembly;

    public string URL
    {
        get
        {
            return m_URL;
        }
    }

    public Assembly Assembly
    {
        get
        {
            return m_Assembly;
        }
    }

    public WWWAssembly(string url, Assembly assembly)
    {
        m_URL = url;
        m_Assembly = assembly;
    }
}