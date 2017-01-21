using UnityEngine;
using VolumetricLines;
using UniRx;
using UniRx.Triggers;

public class LightSaber : MonoBehaviour {

    [Range(0, 2f)]
    public float lightSaberLength = 0.7f;
    public ViveInput viveInput;

    private VolumetricLineBehavior line;
    private ObservableTriggerTrigger triggerTrigger;

    void Awake() {
        line = GetComponent<VolumetricLineBehavior>();
        triggerTrigger = GetComponent<ObservableTriggerTrigger>();
    }

    void Start() {
        line.EndPos = new Vector3(0, 0, lightSaberLength);
        viveInput.ExplodeSaber.Subscribe(_ => {
            Debug.Log("TouchPad Down");
        }).AddTo(this);

        triggerTrigger.OnTriggerEnterAsObservable().Subscribe(collider => {
            Debug.Log("Collided with tag " + collider.tag);
            collider.gameObject.SendMessage("ApplyDamage", 10);
        }).AddTo(this);
    }

    void Update() {

    }
}
