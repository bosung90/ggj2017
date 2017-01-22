using UnityEngine;
using VolumetricLines;
using UniRx;
using UniRx.Triggers;
using System;

public class LightSaber : MonoBehaviour {

    [Range(0, 2f)]
    public float lightSaberFullLength = 0.7f;
    public ViveInput viveInput;

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
    }

    void Start() {
        viveInput.ExplodeSaber.Subscribe(_ => {
            FlashBang();
        }).AddTo(this);

        triggerTrigger.OnTriggerEnterAsObservable().Subscribe(collider => {
            Debug.Log("Collided with tag " + collider.tag);
            if(collider.tag == "Enemy")
            {
                collider.gameObject.transform.Find("Mesh_Ninja").gameObject.SendMessage("TakeDamage", 10);
            }

        }).AddTo(this);

        viveInput.LightSaberLengthPercentObs.Subscribe(lengthPercent => {
            var lightSaberLength = lightSaberFullLength * lengthPercent;
            _collider.height = lightSaberLength;
            line.EndPos = new Vector3(0, 0, lightSaberLength);
        }).AddTo(this);
    }

    void FlashBang() {
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
