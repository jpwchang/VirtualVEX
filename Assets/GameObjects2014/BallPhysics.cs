using UnityEngine;
using System.Collections;

/// <summary>
/// This script changes the Toss Up Ball's center of mass to emulate
/// the weight at the bottom. In order to simulate the fact that the weight
/// does not shift instantly, the CM update occurs once every 0.1 seconds.
/// </summary>
public class BallPhysics : MonoBehaviour {

    public bool debug = false;
    private float loopStartTime_; //Start time of the current iteration of the CM update loop
    private const float TIME_STEP = 0.1f; //Constat value for CM update timestep

	// Use this for initialization
	void Start () {
        loopStartTime_ = Time.timeSinceLevelLoad;
	}
	
	void FixedUpdate()
    {
        //How much time has elapsed since last update?
        float deltaT = Time.timeSinceLevelLoad - loopStartTime_;
        if (deltaT >= TIME_STEP) //This will run if we have reached our desired time step (0.1 s)
        {
            loopStartTime_ = Time.timeSinceLevelLoad;
            //Convert the negative y unit vector to ball's local coordinate system
            Vector3 down = transform.InverseTransformDirection(0, -1, 0);
            //Normalize the vector
            Vector3 normDown = down.normalized;
            if (debug) print(normDown);
            //Set the center of mass to face the ground with a magnitude of 0.12 (about half the radius)
            this.rigidbody.centerOfMass = down * 0.12f;
        }
    }
}
