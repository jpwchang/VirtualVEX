using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the horizontal roller intake
/// </summary>
public class NZIntakeScript : MonoBehaviour {
    public Main_NZBot loader;
    private Vector3 direction;
    private float speed;
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        direction = transform.forward;
        speed = loader.getMotorValue[3];
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.tag == "Red" || other.attachedRigidbody.gameObject.tag == "Blue")
            {
                other.attachedRigidbody.AddForce(direction * speed, ForceMode.Acceleration);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        direction = transform.forward;
        speed = loader.getMotorValue[3];
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.tag == "sack" || other.attachedRigidbody.tag == "bonus_sack")
            {
                if(speed < 0)
                    other.attachedRigidbody.AddForce(direction * (speed * 0.1f), ForceMode.Acceleration);
                else
                    other.attachedRigidbody.AddForce(direction * (speed * 0.05f), ForceMode.Acceleration);
            }
        }
    }
}
