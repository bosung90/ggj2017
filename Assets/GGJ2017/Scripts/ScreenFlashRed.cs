using UnityEngine;
using UniRx;
using DG;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlashRed : MonoBehaviour {

    private Image image;

    void Awake() {
        image = GetComponent<Image>();
    }

	// Use this for initialization
	void Start () {
        PlayerBehavior.Instance.PlayerDamaged.Subscribe(_ => {
            StartCoroutine(FlashRed());
        }).AddTo(this);
    }

    IEnumerator FlashRed() {
        yield return DOTween.ToAlpha(() => image.color, color => image.color = color, 0.3f, 0.1f)
            .Play().WaitForCompletion();
        yield return DOTween.ToAlpha(() => image.color, color => image.color = color, 0f, 0.5f)
            .Play().WaitForCompletion();
    }
}
