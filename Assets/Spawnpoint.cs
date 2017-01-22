using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {

    // Use this for initialization
    public GameObject spawnObj;
	void Start () {
        float timeDelay = 6f;
        InvokeRepeating("spawn", 2.0f, timeDelay);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawn ()
    {
        Instantiate(spawnObj, transform.position, Quaternion.identity);
    }

}
