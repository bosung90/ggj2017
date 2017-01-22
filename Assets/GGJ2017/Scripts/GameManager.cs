using UnityEngine;
using UniRx;

public enum GameState{ START, PLAYING, LOSE };

public class GameManager : MonoBehaviour {

	public static GameManager Instance;

	public ReactiveProperty<GameState> currentState { get; private set; }

	public float score {get; set;}

	void Awake() {
		Instance = this;
		currentState = new ReactiveProperty<GameState> ();
	}

	// Use this for initialization
	void Start () {
		currentState.Subscribe (state => {
			switch(state) {
			case GameState.START : break;
			case GameState.PLAYING : break;
			case GameState.LOSE : Lose(); break;
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame() {
        currentState.Value = GameState.PLAYING;
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
