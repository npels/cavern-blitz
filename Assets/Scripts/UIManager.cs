using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public float fadeSpeed = 1;

    public Image attackCooldown;
    public Image mineCooldown;
    public TMPro.TextMeshProUGUI oreText;
    public Image blackout;
    public Slider healthSlider;

    [HideInInspector]
    public bool fading = false;

    public delegate void OnFadeFunction();

    public IEnumerator FadeOut(OnFadeFunction func = null) {
        while (fading) yield return null;

        fading = true;
        while (blackout.color.a < 0.95f) {
            Color c = blackout.color;
            c = Color.Lerp(c, Color.black, Time.deltaTime * fadeSpeed);
            blackout.color = c;
            yield return null;
        }

        blackout.color = Color.black;
        yield return null;

        fading = false;

        if (func != null) {
            func();
        }
    }

    public IEnumerator FadeIn(OnFadeFunction func = null) {
        while (fading) yield return null;

        fading = true;
        while (blackout.color.a > 0.05f) {
            Color c = blackout.color;
            c = Color.Lerp(c, Color.clear, Time.deltaTime * fadeSpeed);
            blackout.color = c;
            yield return null;
        }

        blackout.color = Color.clear;
        fading = false;

        if (func != null) {
            func();
        }
    }

    public void SetHealth(float percentage) {
        healthSlider.value = percentage;
    }
}
