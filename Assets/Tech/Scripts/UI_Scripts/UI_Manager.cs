using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour 
{
    public static UI_Manager Instance;
    

    VisualElement root;
    TemplateContainer pauseRoot, levelRoot, customizeRoot, dialogueRoot;

    

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        pauseRoot = root.Q<TemplateContainer>("PauseMenu");
        levelRoot = root.Q<TemplateContainer>("LevelSelect");
        //customizeRoot = root.Q<TemplateContainer>("CustomizeMenu");
        dialogueRoot = root.Q<TemplateContainer>("DialogueMenu");

        pauseRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.None;
    }

    public void PauseMenuUI() {
        if (pauseRoot.style.display == DisplayStyle.None) {
            pauseRoot.style.display = DisplayStyle.Flex;
            InputManager.Instance.DisablePlayer();
        } else {
            pauseRoot.style.display = DisplayStyle.None;
            InputManager.Instance.EnablePlayer();
        }
    }

    public void LevelMenuUI() {
        if (levelRoot.style.display == DisplayStyle.None) {
            levelRoot.style.display = DisplayStyle.Flex;
            InputManager.Instance.DisablePlayer();
        } else {
            levelRoot.style.display = DisplayStyle.None;
            InputManager.Instance.EnablePlayer();
        }
    }
}