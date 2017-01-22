using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    public int health = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took 1 damage");
        if(health <= 0)
        {
            Lose();
        }
    }

    private void Lose()
    {

    }
}
