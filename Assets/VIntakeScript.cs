using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the vertical roller intake
/// </summary>
public class VIntakeScript : MonoBehaviour {
    public Main_Dbot loader;
    private Vector3 direction;
    private float speed;
    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        direction = transform.up;
        speed = -loader.getMotorValue[3]/5;
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.gameObject.tag == "Red" || other.attachedRigidbody.gameObject.tag == "Blue")
            {
                if (-loader.getMotorValue[3] > 0)
                    other.attachedRigidbody.useGravity = false;
                else
                    other.attachedRigidbody.useGravity = true;
                other.attachedRigidbody.AddForce(direction * speed, ForceMode.Acceleration);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
            other.attachedRigidbody.useGravity = true;
    }
}
