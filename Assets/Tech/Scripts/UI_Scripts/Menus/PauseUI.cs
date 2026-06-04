using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour {
    UI_Manager UI;
    List<Button> pauseButtons = new List<Button>();
    List<Button> settingsButtons = new List<Button>();
    List<Button> warningButtons = new List<Button>();

    private void Awake() {
        UI = UI_Manager.Instance;
        pauseButtons = UI.pauseRoot.Query<Button>().ToList();
        foreach (var btn in pauseButtons) {
            btn.RegisterCallback<ClickEvent>(OnPauseButton);
            btn.RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        }

        settingsButtons = UI.settingsRoot.Query<Button>().ToList();
        foreach (var btn in settingsButtons) {
            btn.RegisterCallback<ClickEvent>(OnSettingsButton);
            btn.RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        }
        warningButtons = UI.warningRoot.Query<Button>().ToList();
        foreach (var btn in warningButtons) {
            btn.RegisterCallback<ClickEvent>(OnWarningButton);
            btn.RegisterCallback<MouseEnterEvent>(OnHoverSettings);
        }
    }

    void OnPauseButton(ClickEvent evt) {
        Button btn = evt.target as Button;

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
    void OnSettingsButton(ClickEvent evt) {
        Button btn = (Button)evt.target;

        switch (btn.name) {
            case "Back":
                UI.HideSettingsMenuUI();
                UI.ShowPauseMenuUI();
                SFXManager.PlayEffect("BackButton");
                break;
            
            case "Language":
                // Change text on button
                if (btn.text == "Dansk") {
                    SFXManager.PlayEffect("BackButton");
                } else {
                    SFXManager.PlayEffect("ClickButton");
                }

                // Get the ISO 639 Language Code
                string ISO_639 = btn.text switch {
                    "English" => "DA",
                    "Dansk" => "EN",
                    _ => "EN"
                };

                Localization.ChangeLanguage(ISO_639);
                break;
        }
    }

    void OnWarningButton(ClickEvent evt) {
        Button btn = (Button)evt.target;

        UI.HideWARNING();

        if (btn.name == "YES") SceneTransition.Instance.LoadLevel(0);
    }
}