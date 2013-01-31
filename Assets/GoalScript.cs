using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

    public GameObject o;
    GatewayField score;
    int numObjects = 0;
    int owner = 0; //0=No objects scored 1=Red 2=Blue
    private int redScore = 0;
    private int blueScore = 0;
    private int redBonus = 0;
    private int blueBonus = 0;
    private int multiplier = 1;
    private bool hasNegator = false;
    private bool hasDoubler = false;
	// Use this for initialization
	void Start () {
        score = o.GetComponent<GatewayField>();
	}

    public int getBlueScore
    {
        get
        {
            return multiplier*(blueScore + blueBonus);
        }
    }

    public int getRedScore
    {
        get
        {
            return multiplier*(redScore + redBonus);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "Red")
        {
            if (owner == 0 && numObjects == 0)
            {
                owner = 1;
                redBonus++;
            }
            redScore++;
            numObjects++;
            print(redScore);
        }
        else if (other.attachedRigidbody.gameObject.tag == "Blue")
        {
            if (owner == 0 && numObjects == 0)
            {
                owner = 2;
                blueBonus++;
            }
            blueScore++;
            numObjects++;
        }
        else if (other.attachedRigidbody.gameObject.tag == "Doubler")
        {
            if (!hasNegator)
                multiplier = 2;
            else
                multiplier = 1;
            hasDoubler = true;
        }
        else if(other.attachedRigidbody.gameObject.tag == "Negator")
        {
            if(!hasDoubler)
                multiplier = 0;
            else
                multiplier = 1;
            hasNegator = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.gameObject.tag == "Red")
        {
            if (owner == 1 && numObjects == 1)
            {
                owner = 0;
                redBonus--;
            }
            redScore--;
            numObjects--;
        }
        else if (other.attachedRigidbody.gameObject.tag == "Blue")
        {
            if (owner == 2 && numObjects == 1)
            {
                owner = 0;
                blueBonus--;
            }
            blueScore--;
            numObjects--;
        }
        else if (other.attachedRigidbody.gameObject.tag == "Doubler")
        {
            if(!hasNegator)
                multiplier = 1;
            else
                multiplier = 0;
        }
        else if (other.attachedRigidbody.gameObject.tag == "Negator")
        {
            if(!hasDoubler)
                multiplier = 1;
            else
                multiplier = 2;
        }
    }
}
