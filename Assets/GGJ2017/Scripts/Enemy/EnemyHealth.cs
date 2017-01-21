using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //StartCoroutine(TakeDamage(5));
    }

    // Function for taking damage
    IEnumerator TakeDamage(int damage) {
        Debug.Log("Ahhh it hurts took " + damage);
        //this.Health -= damage;

        //renderer.material.color = colors[0];
        //yield WaitForSeconds(.5);
        //renderer.material.color = colors[1];
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(1);
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }
}
