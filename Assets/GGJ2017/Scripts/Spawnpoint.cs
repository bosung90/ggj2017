using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {

    public float timeDelay = 10f;
    // Use this for initialization
    public GameObject spawnObj;
	void Start () {
        InvokeRepeating("spawn", 5.0f, timeDelay);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawn ()
    {
        Instantiate(spawnObj, transform.position, Quaternion.identity);
    }

}
