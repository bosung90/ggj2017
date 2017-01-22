using UnityEngine;
using UniRx;
using System;

public enum GameState { PLAYING, NOT_PLAYING };

public class GameManager : MonoBehaviour {

    private Vector3 playerInitPos;
    private Quaternion playerInitRotation;

    public static GameManager Instance;
    public GameObject spawnPoint;
    public GameObject FlyingSpawnPoint;

    public ReactiveProperty<GameState> currentState { get; private set; }
    public ReactiveProperty<int> currentScore { get; private set; }

    void Awake() {
        Instance = this;
        currentState = new ReactiveProperty<GameState>(GameState.NOT_PLAYING);
        currentScore = new ReactiveProperty<int>(0);
    }

    // Use this for initialization
    void Start() {
        currentScore.Subscribe(score => {
            ScoreManager.instance.incrementScore(score);
        });
        currentState.Subscribe(state => {
            switch (state) {
                case GameState.PLAYING: break;
                case GameState.NOT_PLAYING: Lose(); break;
            }
        });

        playerInitPos = PlayerBehavior.Instance.transform.FindChild("[CameraRig]").position;
        playerInitRotation = PlayerBehavior.Instance.transform.FindChild("[CameraRig]").rotation;
    }

    // Update is called once per frame
    void Update() {

    }

    public void StartGame() {
        currentScore.Value = 0;
        currentState.Value = GameState.PLAYING;

        Instantiate(spawnPoint, new Vector3(UnityEngine.Random.Range(-50f, 50f), 1.0f, UnityEngine.Random.Range(-50f, 50f)), transform.rotation);

        Observable.Timer(TimeSpan.FromSeconds(13f))
            .Where(_ => currentState.Value == GameState.PLAYING)
            .RepeatUntilDestroy(this)
            .Subscribe(_ => {
                Instantiate(spawnPoint, new Vector3(UnityEngine.Random.Range(-50f, 50f), 1.0f, UnityEngine.Random.Range(-50f, 50f)), transform.rotation);
            }).AddTo(this);

        Observable.Timer(TimeSpan.FromSeconds(17f))
            .Where(_ => currentState.Value == GameState.PLAYING)
            .RepeatUntilDestroy(this)
            .Subscribe(_ => {
                Instantiate(FlyingSpawnPoint, new Vector3(UnityEngine.Random.Range(-50f, 50f), 10.0f, UnityEngine.Random.Range(-50f, 50f)), transform.rotation);
            }).AddTo(this);

        GameObject.Find("Player").GetComponent<PlayerBehavior>().health = 10;
    }

    public void setScore(float newVal) {
        currentScore.Value += (int)newVal;
    }

    public void Lose() {
        PlayerBehavior.Instance.transform.FindChild("[CameraRig]").position = playerInitPos;
        PlayerBehavior.Instance.transform.FindChild("[CameraRig]").rotation = playerInitRotation;

        currentState.Value = GameState.NOT_PLAYING;
        // Stop Enemy Spawn
        GameObject[] spawn = GameObject.FindGameObjectsWithTag("spawn");
        for (int i = 0; i < spawn.Length; i++) {
            Destroy(spawn[i]);
        }

        // Destroy all current enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++) {
            Destroy(enemies[i]);
        }

        // Trigger Score Screen
    }
}
