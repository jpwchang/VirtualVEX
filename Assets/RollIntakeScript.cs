using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the horizontal roller intake
/// </summary>
public class RollIntakeScript : MonoBehaviour
{
    public Main_RollerBot loader;
    private Vector3 direction;
    private float speed;
    private string[] validTags_; //The tags of valid intakeable objects
    // Use this for initialization
    void Start()
    {
        validTags_ = new string[] { "Red", "Blue", "sack", "bonus_sack", "red_big", "blue_big" };
    }

    //Test whether the collider is a valid object for this intake
    private bool isIntakeable(Collider other)
    {
        foreach (string s in validTags_)
            if (other.attachedRigidbody.gameObject.tag.Equals(s))
                return true;
        return false;
    }

    //Called when the roller trigger encounters an object
    void OnTriggerStay(Collider other)
    {
        direction = -1.0f * transform.forward;
        speed = loader.motor[3];
        if (other.attachedRigidbody)
        {
            if (isIntakeable(other)) //If the object is intakeable, apply a force to intake it
            {
                if(speed >= 0) //We apply more force if intaking, because gravity helps with outtaking
                    other.attachedRigidbody.AddForce(direction * (speed * 0.2f), ForceMode.Acceleration);
                else
                    other.attachedRigidbody.AddForce(direction * (speed * 0.01f), ForceMode.Acceleration);
            }
        }
    }
}
