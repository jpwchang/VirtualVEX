using UnityEngine;
using System.Collections;

/// <summary>
/// Script attached to goals in Sack Attack for score counting purposes
/// </summary>
public class SAGoalScript : MonoBehaviour {
    public GameObject field;
    public bool isHigh;
    public bool isFloor;
    private int score_;

    public int getScore
    {
        get { return score_; }
    }

	// Use this for initialization
	void Start () {

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "sack")
        {
            if (isHigh)
                score_ += 10;
            else if (isFloor)
                score_ += 1;
            else
                score_ += 5;
        }
        else if (other.attachedRigidbody.gameObject.tag == "bonus_sack")
        {
            if (isHigh)
                score_ += 15;
            else if (isFloor)
                score_ += 6;
            else
                score_ += 10;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "sack")
        {
            if (isHigh)
                score_ -= 10;
            else if (isFloor)
                score_ -= 1;
            else
                score_ -= 5;
        }
        else if (other.attachedRigidbody.gameObject.tag == "bonus_sack")
        {
            if (isHigh)
                score_ -= 15;
            else if (isFloor)
                score_ -= 6;
            else
                score_ -= 10;
        }
    }
}
