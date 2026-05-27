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
        }
        settingsButtons = UI.settingsRoot.Query<Button>().ToList();
        foreach (var btn in settingsButtons) {
            btn.RegisterCallback<ClickEvent>(OnSettingsButton);
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
                break;
            case "BUG_REPORT":
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
                break;
            case "Language":
                TextAsset asset = null;
                switch (btn.text) {
                    case "English":
                        asset = Resources.Load<TextAsset>("Dialogue/DANISH/DAN_main");
                        btn.text = "Danish";
                        break;
                    case "Danish":
                        asset = Resources.Load<TextAsset>("Dialogue/ENGLISH/ENG_main");
                        btn.text = "English";
                        break;
                }
                Debug.Log(asset);
                DialogueManager.Instance.SetInkJSON(asset);
                break;
        }
    }
}