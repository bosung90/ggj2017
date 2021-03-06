﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour {

    public List<AudioClip> ninjaExplodes;

    [Range(20f, 100f)]
    public float Health = 100f;

	public ParticleSystem part;
	public List<ParticleCollisionEvent> collisionEvents;

	public GameObject bloodFX;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GameObject.Find("[CameraRig]").GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        part = GetComponent<ParticleSystem>();
		collisionEvents = new List<ParticleCollisionEvent>();
	}
	
	// Update is called once per frame
	void Update () {
        //StartCoroutine(TakeDamage(2));
    }

    // Function for taking damage
    IEnumerator TakeDamage(int damage) {
        this.Health -= damage;

        if (this.Health <= 0) {
            AudioClip clip = ninjaExplodes[Random.Range(0, ninjaExplodes.Count)];
            audioSource.clip = clip;
            audioSource.time = 0.3f;
            audioSource.Play();

            GameManager.Instance.setScore(1.0f);
            Destroy(transform.parent.gameObject);
            yield return null;
        }
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(1);
        transform.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
    }

    void OnParticleCollision(GameObject other) {

		//if too much blood is instantiated, limit this by creating a flag that will spawn the bloodFX once every 2 seconds or sth
		//int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
		//numCollisionEvents = (numCollisionEvents > 1) ? 1 : numCollisionEvents;

		//Rigidbody rb = other.GetComponent<Rigidbody>();
		//int i = 0;

		//while (i < numCollisionEvents)
		//{
		//	if (rb)
		//	{
		//		Vector3 pos = collisionEvents[i].intersection;
		//		ParticleSystem.VelocityOverLifetimeModule bloodVelocity = 
		//			bloodFX.GetComponent<ParticleSystem> ().velocityOverLifetime;
		//		bloodVelocity.x = UnityEngine.Random.Range (0.0f, 10.0f);
		//		bloodVelocity.y = UnityEngine.Random.Range (0.0f, 5.0f);
		//		bloodVelocity.z = UnityEngine.Random.Range (0.0f, -2.0f);
		//		Instantiate (bloodFX, pos, Quaternion.identity);
		//	}
		//	i++;
		//}

		StartCoroutine(TakeDamage(30));
    }
}
