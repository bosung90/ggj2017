using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    [Range(20f, 100f)]
    public float Health = 100f;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //StartCoroutine(TakeDamage(2));
    }

    // Function for taking damage
    IEnumerator TakeDamage(int damage) {
        Debug.Log("Ahhh it hurts took " + damage + " I have health: " + this.Health);
        this.Health -= damage;

        if (this.Health <= 0) {
            Destroy(transform.parent.gameObject);
            yield return null;
        }
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(1);
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    void OnParticleCollision(GameObject other) {
        Debug.LogWarning("OnParticleCollision");
        StartCoroutine(TakeDamage(30));
    }
}
