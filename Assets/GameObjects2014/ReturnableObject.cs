using UnityEngine;
using System.Collections;

/// <summary>
/// Represents any object that can be returned to the field upon leaving
/// </summary>
public class ReturnableObject : MonoBehaviour {

    private Vector3 position_; //Track the object's position

    public Vector3 getPosition
    {
        get { return position_; }
    }
	
	// Update is called once per frame
	void Update () {
        //Don't update the position if it is outside the field
        if (Mathf.Abs(this.transform.position.x) < 1.6 && Mathf.Abs(this.transform.position.z) < 1.4)
            position_ = this.transform.position;
	}
}
