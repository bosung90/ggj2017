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
    private IObservable<Vector2> touchPadAxis;

    public IObservable<Unit> ExplodeSaber {
        get {
            return touchPadDown;
        }
    }

    void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();

        touchPadDown = Observable
            .EveryUpdate()
            .Select(_ => Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            .DistinctUntilChanged()
            .Where(touchDown => touchDown)
            .Select(_ => Unit.Default)
            .AsObservable();

        hairTriggerDown = Observable
            .EveryUpdate()
            .Select(_ => Controller.GetHairTriggerDown())
            .DistinctUntilChanged()
            .Where(touchDown => touchDown)
            .Select(_ => Unit.Default)
            .AsObservable();

        gripPressDown = Observable
            .EveryUpdate()
            .Select(_ => Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            .DistinctUntilChanged()
            .Where(touchDown => touchDown)
            .Select(_ => Unit.Default)
            .AsObservable();

        applicationButtonDown = Observable
           .EveryUpdate()
           .Select(_ => Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
           .DistinctUntilChanged()
           .Where(touchDown => touchDown)
           .Select(_ => Unit.Default)
           .AsObservable();

        touchPadAxis = Observable
            .EveryUpdate()
            .Select(_ => Controller.GetAxis())
            .Where(axis => axis != Vector2.zero)
            .AsObservable();
    }
}
