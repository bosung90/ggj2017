using UnityEngine;
using UniRx;

public enum GameState{ START, PLAYING, LOSE };

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
    public GameObject spawnPoint;

	public ReactiveProperty<GameState> currentState { get; private set; }
	public ReactiveProperty<float> currentScore { get; private set; }

	void Awake() {
		Instance = this;
		currentState = new ReactiveProperty<GameState> ();
		currentScore = new ReactiveProperty<float> ();
	}

	// Use this for initialization
	void Start () {
		currentScore.Value = 0;
        currentState.Value = GameState.LOSE;
		currentScore.Subscribe(score => {
			ScoreManager.instance.incrementScore(score);
		});
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
        currentScore.Value = 0;
        currentState.Value = GameState.PLAYING;
        GameObject newSpawn = Instantiate(spawnPoint, new Vector3(6.425321f, 1.0f, 50f), transform.rotation);
        GameObject.Find("Player").GetComponent<PlayerBehavior>().health = 10;
    }

	public void setScore(float newVal) {
        Debug.Log(newVal);
		currentScore.Value += newVal;
        Debug.Log(currentScore.Value);
	}

    public void Lose()
    {
        currentState.Value = GameState.LOSE;
        // Stop Enemy Spawn
        GameObject[] spawn = GameObject.FindGameObjectsWithTag("spawn");
        for(int i=0; i< spawn.Length; i++)
        {
            Destroy(spawn[i]);
        }

        // Destroy all current enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        
        // Trigger Score Screen
    }
}
