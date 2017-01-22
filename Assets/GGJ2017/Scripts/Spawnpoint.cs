using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {

    public float timeDelay = 10f;
    // Use this for initialization
    public GameObject spawnObj;
    public GameObject teleportPS;
	void Start () {
        InvokeRepeating("spawn", 5.0f, timeDelay);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawn ()
    {
        GameObject teleInstance = Instantiate(teleportPS, transform.position, Quaternion.identity);
        Instantiate(spawnObj, transform.position, Quaternion.identity);
        Destroy(teleInstance, 2);

    }

}
