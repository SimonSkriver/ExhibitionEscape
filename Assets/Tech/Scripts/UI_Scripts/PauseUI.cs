using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseUI : MonoBehaviour {
    UI_Manager UI;
    List<Button> pauseButtons = new List<Button>();
    List<Button> settingsButtons = new List<Button>();

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
    }
    public void OnHoverSettings(MouseEnterEvent pointerEnterEvent) => SFXManager.PlayEffect("HoverSettings");

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
        }
    }



    void OnSettingsButton(ClickEvent evt) {
        Button btn = (Button)evt.target;

        switch (btn.name) {
            case "Back":
                UI.HideSettingsMenuUI();
                UI.ShowPauseMenuUI();
                SFXManager.PlayEffect("BackButton");
                break;
            case "Language":
                switch (btn.text) {
                    case "English":
                        btn.text = "Danish";
                        SFXManager.PlayEffect("ClickButton");
                        break;
                    case "Danish":
                        btn.text = "English";
                        SFXManager.PlayEffect("BackButton");
                        break;
                }

                // Get the ISO 639 Language Code
                string ISO_639 = "";
                foreach (char c in btn.text.ToCharArray(0, 2)) {
                    ISO_639 += c.ToString().ToUpper();
                }

                TextAsset asset = Resources.Load<TextAsset>($"Dialogue/{btn.text.ToUpper()}/{ISO_639}_main");
                DialogueManager.Instance.SetInkJSON(asset);
                break;
        }
    }
}