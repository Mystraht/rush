using UnityEngine;
using System.Collections;

public class Split : MonoBehaviour {

    public enum SplitAxis {
        xAxis,
        zAxis
    }

    public SplitAxis splitAxis;
    private string direction;
    
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /**
     * Récupère la direction du split
     */
    public string getDirection() {
        inverseDirection();
        return direction;
    }


    /**
     * Inverse la direction du split
     */
    void inverseDirection () {
        if (splitAxis == SplitAxis.xAxis) {
            direction = direction == "right" ? "left" : "right";
        } else {
            direction = direction == "forward" ? "back" : "forward";
        }
    }
}
