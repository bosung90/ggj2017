using UnityEngine;
using VolumetricLines;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

public class LightSaber : MonoBehaviour {

    public List<AudioClip> enemyHits, swishSounds;
    private AudioSource audioSource, swishAudioSource, gunAudio, pulseExplosion;

    [Range(0, 2f)]
    public float lightSaberFullLength = 0.7f;
    public ViveInput viveInput;

    public GameObject circularWave;
    public GameObject Laser;

    private CapsuleCollider _collider;
    private Renderer _renderer;
    private float originalLightSaverFactor;
    private float originalLightSaverLineWidth;

    private VolumetricLineBehavior line;
    private ObservableTriggerTrigger triggerTrigger;

    void Awake() {
        line = GetComponent<VolumetricLineBehavior>();
        triggerTrigger = GetComponent<ObservableTriggerTrigger>();
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<CapsuleCollider>();
        originalLightSaverFactor = _renderer.material.GetFloat("_LightSaberFactor");
        originalLightSaverLineWidth = _renderer.material.GetFloat("_LineWidth");
        audioSource = GetComponents<AudioSource>()[0];
        swishAudioSource = GetComponents<AudioSource>()[1];
        gunAudio = GetComponents<AudioSource>()[2];
        pulseExplosion = GetComponents<AudioSource>()[3];
        swishAudioSource.clip = swishSounds[UnityEngine.Random.Range(0, swishSounds.Count)];

    }

    void Start() {

        viveInput.LightSaberSpeed.Subscribe(speed => {
            swishAudioSource.volume = speed / 60f;
        }).AddTo(this);

        viveInput.ExplodeSaber.Subscribe(_ => {
            FlashBang();
        }).AddTo(this);

        triggerTrigger.OnTriggerEnterAsObservable().Subscribe(collider => {
            Debug.Log("Collided with tag " + collider.tag);
            if(collider.tag == "Enemy")
            {
                AudioClip clip = enemyHits[UnityEngine.Random.Range(0, enemyHits.Count)];
                audioSource.clip = clip;
                audioSource.time = 0.3f;
                audioSource.Play();

                collider.gameObject.SendMessage("TakeDamage", 25);
            }
        }).AddTo(this);

        viveInput.LightSaberLengthPercentObs.Subscribe(lengthPercent => {
            var lightSaberLength = lightSaberFullLength * lengthPercent;
            _collider.height = lightSaberLength;
            line.EndPos = new Vector3(0, 0, lightSaberLength);
        }).AddTo(this);

        viveInput.FireLaser.Subscribe(_ =>
        {
            GameObject laser = Instantiate(Laser);
            gunAudio.time = 0.3f;
            gunAudio.Play();
            laser.transform.position = this.transform.position;
            laser.transform.rotation = this.transform.rotation;
            Destroy(laser, 4f);
        }).AddTo(this);
    }

    void FlashBang() {
        GameObject wave = Instantiate(circularWave);
        wave.transform.position = transform.position;
        Destroy(wave, wave.GetComponent<ParticleSystem>().main.duration);
        pulseExplosion.Play();
        _renderer.material.SetFloat("_LightSaberFactor", 5f);
        _renderer.material.SetFloat("_LineWidth", 10f);
        Observable.Timer(TimeSpan.FromMilliseconds(200f)).Subscribe(_ => {
            _renderer.material.SetFloat("_LightSaberFactor", originalLightSaverFactor);
            _renderer.material.SetFloat("_LineWidth", originalLightSaverLineWidth);
        }).AddTo(this);
    }

    void Update() {

    }
}
