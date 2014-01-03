using UnityEngine;
using System.Collections;

/// <summary>
/// Script attached to individual goals (including floor zones) for TossUp scoring
/// </summary>
public class TUGoalScript : MonoBehaviour {

    public int type; //0 for midzone, 1 for goal zone, 2 for tube
    private const int MIDZONE = 0;
    private const int GOALZONE = 1;
    private const int TUBE = 2;
    private int redScore_;
    private int blueScore_;

	// Use this for initialization
	void Start () {
        redScore_ = 0;
        blueScore_ = 0;
	}

    public int getRedScore
    {
        get { return redScore_;  }
    }
    public int getBlueScore
    {
        get { return blueScore_; }
    }
	
	void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody.gameObject.tag == "Red")
        {
            switch(type)
            {
                case MIDZONE:
                    redScore_++;
                    break;
                case GOALZONE:
                    redScore_ += 2;
                    break;
                case TUBE:
                    redScore_ += 5;
                    break;
            }
        }
        else if(other.attachedRigidbody.gameObject.tag == "Blue")
        {
            switch (type)
            {
                case MIDZONE:
                    blueScore_++;
                    break;
                case GOALZONE:
                    blueScore_ += 2;
                    break;
                case TUBE:
                    blueScore_ += 5;
                    break;
            }
        }
        else if (other.attachedRigidbody.gameObject.tag == "red_big")
        {
            switch (type)
            {
                case MIDZONE:
                    redScore_++;
                    break;
                case GOALZONE:
                    redScore_ += 5;
                    break;
                case TUBE:
                    redScore_ += 10;
                    break;
            }
        }
        else if (other.attachedRigidbody.gameObject.tag == "blue_big")
        {
            switch (type)
            {
                case MIDZONE:
                    blueScore_++;
                    break;
                case GOALZONE:
                    blueScore_ += 5;
                    break;
                case TUBE:
                    blueScore_ += 10;
                    break;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "Red")
        {
            switch (type)
            {
                case MIDZONE:
                    redScore_--;
                    break;
                case GOALZONE:
                    redScore_ -= 2;
                    break;
                case TUBE:
                    redScore_ -= 5;
                    break;
            }
        }
        else if (other.attachedRigidbody.gameObject.tag == "Blue")
        {
            switch (type)
            {
                case MIDZONE:
                    blueScore_--;
                    break;
                case GOALZONE:
                    blueScore_ -= 2;
                    break;
                case TUBE:
                    blueScore_ -= 5;
                    break;
            }
        }
        else if (other.attachedRigidbody.gameObject.tag == "red_big")
        {
            switch (type)
            {
                case MIDZONE:
                    redScore_--;
                    break;
                case GOALZONE:
                    redScore_ -= 5;
                    break;
                case TUBE:
                    redScore_ -= 10;
                    break;
            }
        }
        else if (other.attachedRigidbody.gameObject.tag == "blue_big")
        {
            switch (type)
            {
                case MIDZONE:
                    blueScore_--;
                    break;
                case GOALZONE:
                    blueScore_ -= 5;
                    break;
                case TUBE:
                    blueScore_ -= 10;
                    break;
            }
        }
    }
}
