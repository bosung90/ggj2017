using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;

public class StartButton : MonoBehaviour {

    private ObservableTriggerTrigger trigger;
    private Renderer _renderer;

    public UnityEvent HandleStartTriggerEnter;

    void Awake() {
        trigger = GetComponent<ObservableTriggerTrigger>();
        _renderer = GetComponent<Renderer>();
    }

	// Use this for initialization
	void Start () {

        GameManager.Instance.currentState.Subscribe(gameState => {
            if (gameState == GameState.PLAYING) {
                _renderer.material.color = Color.red;
                Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => {
                    this.gameObject.SetActive(false);
                }).AddTo(this);
            } else {
                this.gameObject.SetActive(true);
                _renderer.material.color = Color.white;
            }
        }).AddTo(this);

        trigger.OnTriggerEnterAsObservable()
            .Subscribe(collider => {
            if(GameManager.Instance.currentState.Value == GameState.NOT_PLAYING)
            {
                HandleStartTriggerEnter.Invoke();
            }
        }).AddTo(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
