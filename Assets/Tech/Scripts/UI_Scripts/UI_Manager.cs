using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour {
    public static UI_Manager Instance;
    InputActionMap playerMap;

    VisualElement root;
    TemplateContainer pauseRoot, levelRoot, customizeRoot;

    GameObject playerCam;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        pauseRoot = root.Q<TemplateContainer>("PauseMenu");
        levelRoot = root.Q<TemplateContainer>("LevelSelect");
        //customizeRoot = root.Q<TemplateContainer>("CustomizeMenu");

        pauseRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.None;

        playerMap = InputSystem.actions.FindActionMap("Player");

        playerCam = GameObject.FindWithTag("CMcam");

    }

    void EnablePlayer() {
        playerMap.Enable();
        playerCam.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    void DisablePlayer() {
        playerMap.Disable();
        playerCam.SetActive(false);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }



    public void PauseMenuUI() {
        Debug.Log(pauseRoot.style.display);
        if (pauseRoot.style.display == DisplayStyle.None) {
            pauseRoot.style.display = DisplayStyle.Flex;
            DisablePlayer();
        } else {
            pauseRoot.style.display = DisplayStyle.None;
            EnablePlayer();
        }
    }

    public void LevelMenuUI() {
        if (levelRoot.style.display == DisplayStyle.None) {
            levelRoot.style.display = DisplayStyle.Flex;
            DisablePlayer();
        } else {
            levelRoot.style.display = DisplayStyle.None;
            EnablePlayer();
        }
    }
}