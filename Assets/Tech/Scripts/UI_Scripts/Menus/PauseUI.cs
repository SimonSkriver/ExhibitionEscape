using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour {
    UI_Manager UI;
    List<Button> pauseButtons = new List<Button>();
    List<RadioButton> settingsButtons = new List<RadioButton>();
    List<Button> warningButtons = new List<Button>();

    private void Awake() {
        UI = UI_Manager.Instance;
        pauseButtons = UI.pauseRoot.Query<Button>().ToList();
        foreach (var btn in pauseButtons) {
            btn.RegisterCallback<ClickEvent>(OnPauseButton);
            btn.RegisterCallback<NavigationSubmitEvent>(OnPauseButton);
            btn.RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        }

        // Settings Buttons
        UI.settingsRoot.Q<Button>("Back").RegisterCallback<ClickEvent>(OnSettingsButton);
        UI.settingsRoot.Q<Button>("Back").RegisterCallback<NavigationSubmitEvent>(OnSettingsButton);
        UI.settingsRoot.Q<Button>("Back").RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        settingsButtons = UI.settingsRoot.Query<RadioButton>().ToList();
        foreach (var btn in settingsButtons) {
            btn.RegisterCallback<ClickEvent>(OnLanguageButton);
            btn.RegisterCallback<NavigationSubmitEvent>(OnLanguageButton);
            btn.RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        }

        warningButtons = UI.warningRoot.Query<Button>().ToList();
        foreach (var btn in warningButtons) {
            btn.RegisterCallback<ClickEvent>(OnWarningButton);
            btn.RegisterCallback<NavigationSubmitEvent>(OnWarningButton);
            btn.RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        }
    }

    // Callbacks
    void OnPauseButton(ClickEvent evt) => PauseButton((Button)evt.target);
    void OnSettingsButton(ClickEvent evt) => SettingsButton((Button)evt.target);
    void OnLanguageButton(ClickEvent evt) => LanguageButton((RadioButton)evt.target);
    void OnWarningButton(ClickEvent evt) => WarningButton((Button)evt.target);
    void OnPauseButton(NavigationSubmitEvent evt) => PauseButton((Button)evt.target);
    void OnSettingsButton(NavigationSubmitEvent evt) => SettingsButton((Button)evt.target);
    void OnLanguageButton(NavigationSubmitEvent evt) => LanguageButton((RadioButton)evt.target);
    void OnWarningButton(NavigationSubmitEvent evt) => WarningButton((Button)evt.target);


    void PauseButton(Button btn) {
        switch (btn.name) {
            case "Continue":
                InputManager.Instance.Pause();
                break;
            case "Settings":
                UI.HidePauseMenuUI();
                UI.ShowSettingsMenuUI();
                SFXManager.PlayEffect("ClickButton");
                break;
            case "BUG_REPORT":
                SFXManager.PlayEffect("ClickButton");
                Application.OpenURL("https://tally.so/r/GxQRbZ");
                break;
            case "Leave_Island":
                UI.ShowWARNING();
                break;
        }
    }

    public void OnHoverSettings(MouseEnterEvent pointerEnterEvent) => SFXManager.PlayEffect("HoverSettings");
    void SettingsButton(Button btn) {
        UI.HideSettingsMenuUI();
        UI.ShowPauseMenuUI();
        SFXManager.PlayEffect("BackButton");
    }

    void LanguageButton(RadioButton rBtn) {
        SFXManager.PlayEffect("ClickButton");        
        Localization.ChangeLanguage(rBtn.name);
    }

    void WarningButton(Button btn) {
        UI.HideWARNING();
        if (btn.name == "YES") SceneTransition.Instance.LoadLevel(0);
    }
}