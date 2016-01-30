using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour {
    public static List<Teleporter> list = new List<Teleporter>();

    public int id;

	// Use this for initialization
	void Start () {
        list.Add(this);
	}
	

	// Update is called once per frame
	void Update () {
	    
	}


    /**
     * Renvoie la position du teleporteur lié au teleporteur actuel
     */
    public Vector3 getLinkedTeleporterPosition() {
        for (int i = 0; i < Teleporter.list.Count; i++) {
            if (Teleporter.list[i].GetComponent<Teleporter>().id == id && Teleporter.list[i] != this) {
                return Teleporter.list[i].transform.position;
            }
        }

        return Vector3.zero;
    }


    void destroy() {
        list.RemoveAt(list.IndexOf(this));
    }
}
