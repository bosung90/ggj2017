using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {

	public static Spawnpoint instance;


    public float timeDelay = 10f;
    // Use this for initialization
    public GameObject spawnObj;
    public GameObject teleportPS;

	void Awake () {
		instance = this;
	}

	void Start () {
        InvokeRepeating("spawn", 5.0f, timeDelay);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawn ()
    {
        int currentScore = (GameManager.Instance) ? GameManager.Instance.currentScore.Value : 0;
		float statMultiplyer = 1.0f;
		if (currentScore >= 40) {
			statMultiplyer = 1.5f;
		} else if (currentScore >= 30) {
			statMultiplyer = 1.4f;
		} else if(currentScore >= 20){
			statMultiplyer = 1.3f;
		} else if(currentScore >= 10) {
			statMultiplyer = 1.1f;
		}
        GameObject teleInstance = Instantiate(teleportPS, transform.position, Quaternion.identity);
		spawnObj.GetComponent<EnemyBehavior>().statMultiplyer = (float)System.Math.Round ((double)Random.Range (1.0f, statMultiplyer), 1);
        Instantiate(spawnObj, transform.position, Quaternion.identity);

        Destroy(teleInstance, 2);
    }

}
