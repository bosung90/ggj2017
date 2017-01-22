using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Lose()
    {
        // Stop Enemy Spawn
        GameObject[] spawn = GameObject.FindGameObjectsWithTag("Spawn");

        // Destroy all current enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyBehavior>().DestroyEnemy();
        }


        // Trigger Score Screen

    }
}
