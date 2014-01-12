using UnityEngine;
using System.Collections;

/// <summary>
/// Returns fallen objects to the field
/// </summary>
public class FieldObjectReturner : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        ReturnableObject obj = other.attachedRigidbody.gameObject.GetComponent<ReturnableObject>();
        if(obj != null)
        {
            //Restore the object to its last known on-field position
            other.transform.position = obj.getPosition;
        }
    }
}
