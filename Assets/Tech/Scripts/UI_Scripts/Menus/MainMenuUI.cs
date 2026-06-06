using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Perameters")]
    VisualElement root;
    TemplateContainer lvlSelectRoot;
    Label islandTitle;
    Button btnSelect;
    List<RadioButton> languageButtons = new List<RadioButton>();

    [Header("Localization")]
    LocalizedString titleBinding;
    IntVariable islandID;

    [Header("Cameras")]
    Transform camParent;
    List<GameObject> islandCams = new List<GameObject>();
    int previousCamID, currentCamID, maxCamID;

    [Header("SFX")]
    bool clickButtonPlayed;

    private void Awake() {
        // Cameras
        camParent = GameObject.Find("Cameras/IslandCams").transform;

        for (int i = 0; i < camParent.childCount; ++i) {
            GameObject cam = camParent.GetChild(i).gameObject;
            islandCams.Add(cam);
            cam.SetActive(false);
            cam.name = "LOCKED_" + cam.name;
        }
        islandCams[0].SetActive(true);
        islandCams[0].name = "UN" + islandCams[0].name;
        maxCamID = islandCams.Count - 1;

        // UI
        root = GetComponent<UIDocument>().rootVisualElement;
        lvlSelectRoot = root.Q<TemplateContainer>("LevelSelect");
        btnSelect = lvlSelectRoot.Q<Button>("SELECT");
        islandTitle = lvlSelectRoot.Q<Label>("Island_Title");
        languageButtons = root.Query<RadioButton>().ToList();

        // Localization
        titleBinding = (LocalizedString)islandTitle.GetBinding("text");
        islandID = (IntVariable)titleBinding["islandID"];

        // Register Callbacks
        btnSelect.RegisterCallback<ClickEvent>(OnLevelSelect);
        lvlSelectRoot.Q<Button>("PREVIOUS").RegisterCallback<ClickEvent>(OnLevelSelect);
        lvlSelectRoot.Q<Button>("NEXT").RegisterCallback<ClickEvent>(OnLevelSelect);

        btnSelect.RegisterCallback<NavigationSubmitEvent>(OnLevelSelect);
        lvlSelectRoot.Q<Button>("PREVIOUS").RegisterCallback<NavigationSubmitEvent>(OnLevelSelect);
        lvlSelectRoot.Q<Button>("NEXT").RegisterCallback<NavigationSubmitEvent>(OnLevelSelect);

        btnSelect.RegisterCallback<PointerEnterEvent>(OnHover);
        lvlSelectRoot.Q<Button>("PREVIOUS").RegisterCallback<PointerEnterEvent>(OnHover);
        lvlSelectRoot.Q<Button>("NEXT").RegisterCallback<PointerEnterEvent>(OnHover);

        foreach (var btn in languageButtons) {
            btn.RegisterCallback<ClickEvent>(evt => {
                var rb = (RadioButton)evt.target;
                Localization.ChangeLanguage(rb.name);
                SFXManager.PlayEffect((clickButtonPlayed = !clickButtonPlayed) ? "BackButton" : "ClickButton");
            });
            btn.RegisterCallback<NavigationSubmitEvent>(evt => {
                var rb = (RadioButton)evt.target;
                Localization.ChangeLanguage(rb.name);
                SFXManager.PlayEffect((clickButtonPlayed = !clickButtonPlayed) ? "BackButton" : "ClickButton");
            });
            btn.RegisterCallback<PointerEnterEvent>(OnHover);
        }
    }

    public void OnHover(PointerEnterEvent pointerEnterEvent) => SFXManager.PlayEffect("HoverSettings");

    void OnLevelSelect(ClickEvent evt) => LevelSelect((Button)evt.target);
    void OnLevelSelect(NavigationSubmitEvent evt) => LevelSelect((Button)evt.target);
    void LevelSelect(Button btn) {
        // Set CamID
        previousCamID = currentCamID;
        switch (btn.name) {
            case "PREVIOUS":
                if (currentCamID == 0) { currentCamID = maxCamID; }
                else { currentCamID--; }
                SFXManager.PlayEffect("BackButton");
                break;
            case "NEXT":
                if (currentCamID == maxCamID) { currentCamID = 0; }
                else { currentCamID++; }
                SFXManager.PlayEffect("ClickButton");
                break;
            case "SELECT":
                StartCoroutine(LoadLevel());
                SFXManager.PlayEffect("ConfirmColor");
                return;
        }

        // Change Localization variable & disable btnSelect if island isn't unlocked
        if (islandCams[currentCamID].name.Contains("UNLOCKED")) {
            islandID.Value = currentCamID;
            btnSelect.SetEnabled(true);
        } else {
            islandID.Value = -1; // Make the title ???
            btnSelect.SetEnabled(false);
        }
        
        // Change Camera
        islandCams[currentCamID].SetActive(true);
        islandCams[previousCamID].SetActive(false);
    }

    IEnumerator LoadLevel() {
        SceneTransition.Instance.LoadLevel(currentCamID + 1);
        float fov = 60;
        while (fov > 40) {
            fov--;
            camParent.GetComponent<CinemachineFollowZoom>().FovRange.x = fov;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
