using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Unity.Cinemachine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    InputActionMap playerMap;

    VisualElement root;
    GameObject playerCam;

    bool showPauseMenu;

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;

        playerMap = InputSystem.actions.FindActionMap("Player");

        playerCam = GameObject.FindWithTag("CMcam");
        
    }


    public void ShowPauseMenu() {
        showPauseMenu = !showPauseMenu;
        Debug.Log(showPauseMenu);

        if (showPauseMenu) {
            root.Q<TemplateContainer>("PauseMenu").style.display = DisplayStyle.Flex;
            playerMap.Disable();
            playerCam.SetActive(false);
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

        } else {
            root.Q<TemplateContainer>("PauseMenu").style.display = DisplayStyle.None;
            playerCam.SetActive(true);
            playerMap.Enable();
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
}
