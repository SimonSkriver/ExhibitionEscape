using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour {
    VisualElement root;
    List<Button> pauseButtons = new List<Button>();

    private void Awake() {
        root = GetComponent<UIDocument>().rootVisualElement;
        pauseButtons = root.Q<TemplateContainer>("PauseMenu").Query<Button>().ToList();
        foreach (var btn in pauseButtons) {
            btn.RegisterCallback<ClickEvent>(OnPauseButton);
        }
    }

    void OnPauseButton(ClickEvent evt) {
        Button btn = evt.target as Button;

        switch (btn.name) {
            case "Continue":
                UI_Manager.Instance.PauseMenuUI();
                break;
            case "BUG_REPORT":
                Application.OpenURL("https://tally.so/r/GxQRbZ");
                break;
        }
    }
}