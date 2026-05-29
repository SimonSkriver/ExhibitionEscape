using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour 
{
    public static UI_Manager Instance;
    public PauseUI pauseUI { get; private set; }
    public DialogueUI dialogueUI { get; private set; }

    public VisualElement root { get; private set; }
    public TemplateContainer HUD_Root { get; private set; }
    public TemplateContainer pauseRoot { get; private set; }
    public TemplateContainer settingsRoot { get; private set; }
    public TemplateContainer levelRoot { get; private set; }
    public TemplateContainer customizeRoot { get; private set; }
    public TemplateContainer dialogueRoot { get; private set; }

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        HUD_Root = root.Q<TemplateContainer>("PlayerHUD");
        pauseRoot = root.Q<TemplateContainer>("PauseMenu");
        settingsRoot = root.Q<TemplateContainer>("SettingsMenu");
        levelRoot = root.Q<TemplateContainer>("LevelSelect");
        customizeRoot = root.Q<TemplateContainer>("CustomizeMenu");
        dialogueRoot = root.Q<TemplateContainer>("DialogueMenu");

        // UI Scripts
        pauseUI = GetComponent<PauseUI>();
        dialogueUI = GetComponent<DialogueUI>();

        // Apply display style
        pauseRoot.style.display = DisplayStyle.None;
        levelRoot.style.display = DisplayStyle.None;
        dialogueRoot.style.display = DisplayStyle.None;

        //StartCoroutine(ScreenTransition());
    }

    private void Start() {
        ShowPlayerCustomizeUI();
    }

    public void ShowPlayerHUD() => HUD_Root.style.display = DisplayStyle.Flex;
    public void HidePlayerHUD() => HUD_Root.style.display = DisplayStyle.None;
    public void ShowPauseMenuUI() => pauseRoot.style.display = DisplayStyle.Flex;
    public void HidePauseMenuUI() => pauseRoot.style.display = DisplayStyle.None;

    public void ShowLevelMenuUI() {
        levelRoot.style.display = DisplayStyle.Flex;
        InputManager.Instance.DisablePlayer();
    }
    public void HideLevelMenuUI() {
        levelRoot.style.display = DisplayStyle.None;
        InputManager.Instance.EnablePlayer();
    }

    public void ShowSettingsMenuUI() => settingsRoot.style.display = DisplayStyle.Flex;
    public void HideSettingsMenuUI() => settingsRoot.style.display = DisplayStyle.None;

    public void ShowPlayerCustomizeUI() {
        customizeRoot.style.display = DisplayStyle.Flex;
        HidePlayerHUD();
        GameManager.CameraManager.GetChild(1).gameObject.SetActive(true);
        InputManager.Instance.DisablePlayer();
    }
    public void HidePlayerCustomizeUI() {
        customizeRoot.style.display = DisplayStyle.None;
        ShowPlayerHUD();
        GameManager.CameraManager.GetChild(1).gameObject.SetActive(false);
        InputManager.Instance.EnablePlayer();
    }
    public void ShowDialogueUI() {
        dialogueRoot.style.display = DisplayStyle.Flex;
        ShowPlayerHUD();
        InputManager.Instance.DisablePlayer();
    }

    public void HideDialogueUI() {
        dialogueRoot.style.display = DisplayStyle.None;
        InputManager.Instance.EnablePlayer();
    }

    public IEnumerator ScreenTransition() {
        float alpha = 0;

        // Fade in
        while (alpha < 1 ) {
            yield return new WaitForSeconds(0.01f);
            alpha += 0.01f;
            root.Q<VisualElement>("TransitionScreen").style.backgroundColor = new Color(1, 1, 1, alpha);
            if (alpha > 1) { alpha = 1; }
            Debug.Log(alpha);
        }

        yield return new WaitUntil(() => alpha.Equals(1));
        yield return new WaitForSeconds(1f);

        // Fade out
        while (alpha > 0) {
            yield return new WaitForSeconds(0.01f);
            alpha -= 0.01f;
            root.Q<VisualElement>("TransitionScreen").style.backgroundColor = new Color(1, 1, 1, alpha);
            Debug.Log(alpha);
        }
    }

    
}