using UnityEngine;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour 
{
    public static UI_Manager Instance;
    public PauseUI pauseUI { get; private set; }
    public DialogueUI dialogueUI { get; private set; }

    #region Roots
    public VisualElement root { get; private set; }
    public TemplateContainer HUD_Root { get; private set; }
    public TemplateContainer pauseRoot { get; private set; }
    public TemplateContainer settingsRoot { get; private set; }
    public TemplateContainer warningRoot { get; private set; }
    public TemplateContainer customizeRoot { get; private set; }
    public TemplateContainer dialogueRoot { get; private set; }
    public TemplateContainer creditsRoot { get; private set; }
    #endregion

    private void Awake() {
        if (Instance != null) { Destroy(gameObject); }
        Instance = this;

        root = GetComponent<UIDocument>().rootVisualElement;
        HUD_Root = root.Q<TemplateContainer>("PlayerHUD");
        pauseRoot = root.Q<TemplateContainer>("PauseMenu");
        settingsRoot = root.Q<TemplateContainer>("SettingsMenu");
        warningRoot = root.Q<TemplateContainer>("WARNING_BOX");
        customizeRoot = root.Q<TemplateContainer>("CustomizeMenu");
        dialogueRoot = root.Q<TemplateContainer>("DialogueMenu");
        creditsRoot = root.Q<TemplateContainer>("Credits");

        // UI Scripts
        pauseUI = GetComponent<PauseUI>();
        dialogueUI = GetComponent<DialogueUI>();

        // Apply display style
        pauseRoot.style.display = DisplayStyle.None;
        settingsRoot.style.display = DisplayStyle.None;
        dialogueRoot.style.display = DisplayStyle.None;

        // Credits Button
        creditsRoot.Q<Button>("LEAVE_ISLAND").RegisterCallback<ClickEvent>(OnCreditsButton);
        creditsRoot.Q<Button>("LEAVE_ISLAND").RegisterCallback<NavigationSubmitEvent>(OnCreditsButton);
    }

    private void Start() => ShowPlayerCustomizeUI();

    public void ShowPlayerHUD() => HUD_Root.style.display = DisplayStyle.Flex;
    public void HidePlayerHUD() => HUD_Root.style.display = DisplayStyle.None;
    public void ShowPauseMenuUI() => pauseRoot.style.display = DisplayStyle.Flex;
    public void HidePauseMenuUI() => pauseRoot.style.display = DisplayStyle.None;
    public void ShowWARNING() => root.Q("WARNING").style.display = DisplayStyle.Flex;
    public void HideWARNING() => root.Q("WARNING").style.display = DisplayStyle.None;
    public void ShowSettingsMenuUI() => settingsRoot.style.display = DisplayStyle.Flex;
    public void HideSettingsMenuUI() => settingsRoot.style.display = DisplayStyle.None;

    public void ShowPlayerCustomizeUI() {
        customizeRoot.style.display = DisplayStyle.Flex;
        HidePlayerHUD();
        GameManager.CameraManager.GetChild(1).gameObject.SetActive(true);
        InputManager.Instance.canPauseGame = false;
        InputManager.Instance.DisablePlayer();
    }
    public void HidePlayerCustomizeUI() {
        customizeRoot.style.display = DisplayStyle.None;
        ShowPlayerHUD();
        GameManager.CameraManager.GetChild(1).gameObject.SetActive(false);
        InputManager.Instance.canPauseGame = true;
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

    // Credits
    public void ShowCredits() => creditsRoot.style.display = DisplayStyle.Flex;
    public void HideCredits() => creditsRoot.style.display = DisplayStyle.None;
    void OnCreditsButton(ClickEvent evt) {
        Time.timeScale = 1;
        SceneTransition.Instance.LoadLevel(0);
    }
    void OnCreditsButton(NavigationSubmitEvent evt) {
        Time.timeScale = 1;
        SceneTransition.Instance.LoadLevel(0);
    }
}