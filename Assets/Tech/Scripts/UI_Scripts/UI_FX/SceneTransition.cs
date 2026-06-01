using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneTransition : MonoBehaviour {
    public static SceneTransition Instance;

    VisualElement transitionScreen;

    private void Awake() {
        if(Instance != null) { 
            Destroy(gameObject);
            Debug.LogWarning("There is multiple Instances of SceneTransition");
        }
        Instance = this;

        transitionScreen = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("TransitionScreen");
    }

    public void LoadLevel(int sceneID) => StartCoroutine(StartTransition(sceneID));

    IEnumerator StartTransition(int sceneID) {
        StartCoroutine(ColorFadeUI.FadeIn(transitionScreen));

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneID);

        StartCoroutine(ColorFadeUI.FadeOut(transitionScreen));
    }
}
