using System;
using UniRx;
using UnityEngine;

public class ViveInput : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private IObservable<Unit> touchPadDown;
    private IObservable<Unit> hairTriggerDown;
    private IObservable<Unit> gripPressDown;
    private IObservable<Unit> applicationButtonDown;
    public IObservable<Unit> applicationButtonUp { get; private set; }
    public IObservable<Unit> applicationButton { get; private set; }
    private IObservable<Vector2> touchPadAxis;

    private bool isExplodeSaberCooldown = false;
    private float lightSaberLengthPercent = 1f;
    public IObservable<float> LightSaberLengthPercentObs { get; private set; }
    public IObservable<Unit> ExplodeSaber { get; private set; }
    public IObservable<float> LightSaberSpeed { get; private set; }

    public IObservable<Unit> FireLaser
    {
        get
        {
            return hairTriggerDown;
        }
    }

    void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        LightSaberSpeed = Observable
            .EveryUpdate()
            .Select(_ => (Controller.velocity + Controller.angularVelocity).sqrMagnitude)
            .AsObservable();

        touchPadDown = Observable
            .EveryUpdate()
            .Where(_ => Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            .AsUnitObservable();

        hairTriggerDown = Observable
            .EveryUpdate()
            .Where(_ => Controller.GetHairTriggerDown())
            .AsUnitObservable();

        gripPressDown = Observable
            .EveryUpdate()
            .Where(_ => Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            .AsUnitObservable();

        applicationButtonDown = Observable
           .EveryUpdate()
           .Where(_ => Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
           .AsUnitObservable();

        applicationButtonUp = Observable
            .EveryUpdate()
            .Where(_ => Controller.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            .AsUnitObservable();

        applicationButton = Observable
            .EveryUpdate()
            .Where(_ => Controller.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu))
            .AsUnitObservable();

        touchPadAxis = Observable
            .EveryUpdate()
            .Select(_ => Controller.GetAxis())
            .Where(axis => axis != Vector2.zero)
            .AsObservable();

        LightSaberLengthPercentObs = Observable
            .EveryUpdate()
            .Select(_ => Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            .Do(touchDown => {
                if (touchDown && !isExplodeSaberCooldown) {
                    lightSaberLengthPercent -= 1f * Time.deltaTime;
                    if (lightSaberLengthPercent < 0f) lightSaberLengthPercent = 0f;
                } else {
                    lightSaberLengthPercent += 1f * Time.deltaTime;
                    if (lightSaberLengthPercent > 1f) lightSaberLengthPercent = 1f;
                }
            })
            .Select(_ => lightSaberLengthPercent)
            .DistinctUntilChanged()
            .AsObservable();

        ExplodeSaber = Observable.EveryUpdate()
            .Where(_ => Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            .Where(_ => lightSaberLengthPercent == 0f)
            .Do(_ => lightSaberLengthPercent = 1f)
            .Do(_ => isExplodeSaberCooldown = true)
            .Do(_ => Observable.Timer(TimeSpan.FromMilliseconds(10000f)).Subscribe(__ => isExplodeSaberCooldown = false))
            .AsUnitObservable();
    }
}
