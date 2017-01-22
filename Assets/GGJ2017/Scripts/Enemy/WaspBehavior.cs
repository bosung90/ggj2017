using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WaspBehavior : MonoBehaviour {

    private Transform Player;
    private Vector3 playerHead;
    private Animator anim;

    public bool isReadyToAttack = true;

    private IDisposable timerDisposable = null;

    private IObservable<Unit> Idle;
    private IObservable<Unit> Fly;
    private IObservable<Unit> Attack;

    private AudioSource waspAttack;

    public float statMultiplyer = 1.0f;
    // Enemy Info
    public float MoveSpeed = 10;
    public float AttackRange = 4;
    public float MinRange = 3;
    public float AttackDamage = 1;

    private readonly string ANIM_KEY = "AnimationID";
    private readonly string IDLE = "Idle";
    private readonly string FLYING = "Flying";
    private readonly string ATTACK = "Attack";

    void Awake() {
        anim = GetComponent<Animator>();
        Player = GameObject.Find("Camera (eye)").transform;
        waspAttack = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start() {
        Color color = Color.blue;
        if (statMultiplyer >= 1.5f) {
            color = Color.grey;
        } else if (statMultiplyer >= 1.4f) {
            color = Color.cyan;
        } else if (statMultiplyer >= 1.3f) {
            color = Color.green;
        } else if (statMultiplyer >= 1.1f) {
            color = Color.yellow;
        }
        transform.gameObject.GetComponentInChildren<Renderer>().material.SetColor("_Color", color);
        MoveSpeed *= statMultiplyer;
        AttackRange *= statMultiplyer;
        MinRange *= statMultiplyer;
        AttackDamage *= statMultiplyer;

        Observable.EveryUpdate()
            .Select(_ => new Vector3(Player.position.x, Player.position.y, Player.position.z))
            .Select(playerHead => Vector3.Distance(transform.position, playerHead) <= AttackRange)
            .DistinctUntilChanged()
            .Where(withinAttackRange => !withinAttackRange)
            .Subscribe(withinAttackRange => {
                anim.SetTrigger(FLYING);
            }).AddTo(this);

        Observable.EveryUpdate()
            .Select(_ => new Vector3(Player.position.x, Player.position.y, Player.position.z))
            .Select(playerHead => Vector3.Distance(transform.position, playerHead) <= AttackRange)
            .Where(withinAttackRange => withinAttackRange && isReadyToAttack)
            .Subscribe(__ => {
                if(isReadyToAttack) {
                    anim.SetTrigger(ATTACK);
                    isReadyToAttack = false;
                    if (timerDisposable != null) timerDisposable.Dispose();
                    timerDisposable = Observable.Timer(TimeSpan.FromSeconds(3f)).Subscribe(_ => {
                        isReadyToAttack = true;
                    }).AddTo(this);
                }
            }).AddTo(this);
    }

    void Update() {
        playerHead = new Vector3(Player.position.x, Player.position.y, Player.position.z);

        transform.LookAt(playerHead);

        if (Vector3.Distance(transform.position, playerHead) <= AttackRange) {
            if (Vector3.Distance(transform.position, playerHead) <= MinRange) {
                transform.position = (transform.position - playerHead).normalized * MinRange + playerHead;
            }

            //if (isReadyToAttack) {
            //    isReadyToAttack = false;
            //    anim.SetTrigger(ATTACK);
            //if (timerDisposable != null) timerDisposable.Dispose();
            //timerDisposable = Observable.Timer(TimeSpan.FromSeconds(3f)).Subscribe(_ => {
            //    isReadyToAttack = true;
            //}).AddTo(this);
            //}

        } else {
            //anim.SetTrigger(FLYING);
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
    }

    //IEnumerator AttackCooldown() {
    //    anim.SetInteger("AnimationID", 0);
    //    yield return new WaitForSeconds(3f);
    //    isReadyToAttack = true;
    //}

    void AttackPlayer() {
        Debug.Log("Attack player!");
        waspAttack.Play();
        GameObject.Find("Player").GetComponent<PlayerBehavior>().TakeDamage((int)AttackDamage);
    }
}
