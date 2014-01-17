using UnityEngine;
using System.Collections;

public class vvComponentCollider : MonoBehaviour {
    public GameObject parent;

    private vvRobotBase parentScript_;
    private bool collision_ = false;
    private float velocity_;

    public bool isColliding
    {
        get { return collision_; }
    }
    public float getVelocity
    {
        get { return velocity_; }
    }

	// Use this for initialization
	void Start () {
        parentScript_ = parent.GetComponent<vvRobotBase>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnCollisionEnter(Collision c)
    {
        if (c.collider.tag == "red_trough" || c.collider.tag == "blue_trough" || c.collider.tag == "collidable")
            collision_ = true;
        velocity_ = c.relativeVelocity.magnitude;
    }

    void OnCollisionExit(Collision c)
    {
        if (c.collider.tag == "red_trough" || c.collider.tag == "blue_trough" || c.collider.tag == "collidable")
            collision_ = false;
    }
}
