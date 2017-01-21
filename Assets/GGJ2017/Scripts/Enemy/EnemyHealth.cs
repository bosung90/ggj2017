using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Function for taking damage
    void TakeDamage(int damage) {
        Debug.Log("Ahhh it hurts took " + damage);
        //this.Health -= damage;
    }
}
