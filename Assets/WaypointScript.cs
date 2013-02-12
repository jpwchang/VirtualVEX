using UnityEngine;
using System.Collections;

/// <summary>
/// Controls behavior of the on-screen Waypoint object
/// </summary>
public class WaypointScript : MonoBehaviour {
    private GameObject tracker_;
    private ModeTrackingScript trackerSript_;

    void Start()
    {
        tracker_ = GameObject.Find("Mode Tracker");
        trackerSript_ = tracker_.GetComponent<ModeTrackingScript>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "bot")
        {
            print("Waypoint Triggered");
            trackerSript_.advanceWaypoint();
            Destroy(this.gameObject);
        }
    }
}
