using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public float fadeSpeed = 1;

    public Image leftCooldown;
    public Image rightCooldown;
    public Image blackout;
    public Slider healthSlider;
    public GameObject descendMessage;
    public GameObject leaveMessage;
    public Image leftSprite;
    public Image rightSprite;
    public TMPro.TextMeshProUGUI floorText;

    public GameObject ropeTipPrefab;
    public GameObject ropeSetPrefab;

    [HideInInspector]
    public bool fading = false;

    public delegate void OnFadeFunction();

    public void OpenRopeTip() {
        Instantiate(ropeTipPrefab, transform);
    }

    public void SetRopeMessage() {
        Instantiate(ropeSetPrefab, transform);
    }

    public void OpenDescendMessage() {
        GameManager.instance.player.GetComponent<PlayerMovement>().canMove = false;
        GameManager.instance.player.GetComponent<PlayerInteractions>().isDescending = true;
        descendMessage.SetActive(true);
    }

    public void CloseDescendMessage() {
        GameManager.instance.player.GetComponent<PlayerMovement>().canMove = true;
        GameManager.instance.player.GetComponent<PlayerInteractions>().isDescending = false;
        descendMessage.SetActive(false);
    }

    public void OpenLeaveMessage() {
        GameManager.instance.player.GetComponent<PlayerMovement>().canMove = false;
        GameManager.instance.player.GetComponent<PlayerInteractions>().isDescending = true;
        leaveMessage.SetActive(true);
    }

    public void CloseLeaveMessage() {
        GameManager.instance.player.GetComponent<PlayerMovement>().canMove = true;
        GameManager.instance.player.GetComponent<PlayerInteractions>().isDescending = false;
        leaveMessage.SetActive(false);
    }

    public void SetFloorNumber(int num) {
        floorText.text = num.ToString();
    }

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
