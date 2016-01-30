using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

    public enum TargetColor {
        Green,
        Red,
        Blue,
        Yellow
    }

    public TargetColor targetColor;

    // Use this for initialization
    void Start () {
        //GetComponent<Renderer>().material.color = Color.green;
        switch (targetColor) {
            case TargetColor.Green:
                GetComponent<Renderer>().material.color = Color.green;
                break;
            case TargetColor.Blue:
                GetComponent<Renderer>().material.color = Color.blue;
                break;
            case TargetColor.Red:
                GetComponent<Renderer>().material.color = Color.red;
                break;
            case TargetColor.Yellow:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
