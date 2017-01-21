using UnityEngine;
using VolumetricLines;
using UniRx;

public class LightSaber : MonoBehaviour {

    [Range(0, 2f)]
    public float lightSaberLength = 0.7f;
    public ViveInput viveInput;

    private VolumetricLineBehavior line;

    void Awake() {
        line = GetComponent<VolumetricLineBehavior>();
    }

    void Start() {
        line.EndPos = new Vector3(0, 0, lightSaberLength);
        viveInput.ExplodeSaber.Subscribe(_ => {
            Debug.Log("TouchPad Down");
        }).AddTo(this);
    }

    void Update() {

    }
}
