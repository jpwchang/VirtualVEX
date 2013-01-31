using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the tank tread (conveyor) intake
/// </summary>
public class IntakeScript : MonoBehaviour {
    public vvRobotBase loader;
    public float speed;
    public float visualSpeedScalar;

    private Vector3 direction;
    private float currentScroll;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerStay(Collider other)
    {
        
        // Get the direction of the conveyor belt (transform.forward is a built in Vector3 which is used to get the forward facing direction)
        // * Remember Vector3's can used for position AND direction AND rotation
        direction = transform.forward;
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.tag == "Red" || other.attachedRigidbody.gameObject.tag == "Blue" || other.attachedRigidbody.gameObject.tag == "Doubler" || other.attachedRigidbody.gameObject.tag == "Negator")
            {
                direction = transform.forward;
                other.attachedRigidbody.useGravity = false;
                if (-loader.motor[3] > 1)
                {
                    speed = loader.motor[3] / 100;
                    // Add a WORLD force to the other objects
                    // Ignore the mass of the other objects so they all go the same speed (ForceMode.Acceleration)
                    other.attachedRigidbody.AddForce(direction * speed, ForceMode.Acceleration);
                }
                else if (Mathf.RoundToInt(loader.motor[3]) == 0)
                {
                    speed = 0;
                    // Add a WORLD force to the other objects
                    // Ignore the mass of the other objects so they all go the same speed (ForceMode.Acceleration)
                    other.attachedRigidbody.AddForce(direction * speed, ForceMode.Acceleration);
                }
                else
                {
                    direction = transform.forward;
                    Vector3.Reflect(direction, direction);
                    speed = loader.motor[3] / 220;
                    // Add a WORLD force to the other objects
                    // Ignore the mass of the other objects so they all go the same speed (ForceMode.Acceleration)
                    other.attachedRigidbody.AddForce(direction * speed);
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
            other.attachedRigidbody.useGravity = true;
    }
}
