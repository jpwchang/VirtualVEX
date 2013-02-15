using UnityEngine;
using System.Collections;
using vvIO;

/// <summary>
/// Controls behavior of the on-screen Waypoint object
/// </summary>
public class WaypointScript : MonoBehaviour {
    private GameObject tracker_;
    private ModeTrackingScript trackerScript_;

    void Start()
    {
        tracker_ = GameObject.Find("Mode Tracker");
        trackerScript_ = tracker_.GetComponent<ModeTrackingScript>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "bot")
        {
            vvConsole.println("Waypoint " + (trackerScript_.getWaypoint + 1) + " Reached");
            trackerScript_.advanceWaypoint();
            Destroy(this.gameObject);
        }
    }
}
