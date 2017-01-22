using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    public int health = 10;

    public static PlayerBehavior Instance;

    private Subject<Unit> playerDamaged = new Subject<Unit>();
    public IObservable<Unit> PlayerDamaged {
        get { return playerDamaged; }
    }

    void Awake() {
        Instance = this;
    }

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
            playerDamaged.OnNext(Unit.Default);
            GameObject.Find("GameManager").GetComponent<GameManager>().Lose();
        } else {
            playerDamaged.OnNext(Unit.Default);
        }
    }
}
