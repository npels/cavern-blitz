using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public List<GameObject> tutorialScreens;

    private int index = 0;

    public void NextScreen() {
        tutorialScreens[index].SetActive(false);
        if (index + 1 < tutorialScreens.Count) {
            tutorialScreens[++index].SetActive(true);
        } else {
            TutorialManager.instance.inTutorial = false;
            gameObject.SetActive(false);
        }
    }
}
