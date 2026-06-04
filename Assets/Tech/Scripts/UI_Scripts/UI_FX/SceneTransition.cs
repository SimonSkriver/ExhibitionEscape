using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneTransition : MonoBehaviour {
    public static SceneTransition Instance;

    VisualElement transitionScreen;

    private void Awake() {
        Time.timeScale = 1;
        if (Instance != null && Instance != this) { 
            Destroy(gameObject);
            Debug.LogWarning("There is multiple Instances of SceneTransition");
        }
        Instance = this;

        transitionScreen = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("TransitionScreen");
        StartCoroutine(ColorFadeUI.FadeOut(transitionScreen));
    }

    public void LoadLevel(int sceneID) {
        StartCoroutine(ColorFadeUI.FadeIn(transitionScreen));
        SceneManager.LoadScene(sceneID);
    }
}
