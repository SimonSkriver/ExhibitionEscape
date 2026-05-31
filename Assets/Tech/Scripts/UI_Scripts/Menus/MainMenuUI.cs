using System.Collections.Generic;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Perameters")]
    VisualElement root;
    TemplateContainer lvlSelectRoot;
    Label islandTitle;
    Button btnSelect;

    [Header("Cameras")]
    Transform camParent;
    List<GameObject> islandCams = new List<GameObject>();
    int previousCamID, currentCamID, maxCamID;

    private void Awake() {
        // Cameras
        camParent = GameObject.Find("Cameras/IslandCams").transform;

        for (int i = 0; i < camParent.childCount; ++i) {
            GameObject cam = camParent.GetChild(i).gameObject;
            islandCams.Add(cam);
            cam.SetActive(false);
        }
        islandCams[0].SetActive(true);
        maxCamID = islandCams.Count - 1;

        // UI
        root = GetComponent<UIDocument>().rootVisualElement;
        lvlSelectRoot = root.Q<TemplateContainer>("LevelSelect");

        btnSelect = lvlSelectRoot.Q<Button>("SELECT");
        btnSelect.RegisterCallback<ClickEvent>(OnLevelSelect);
        lvlSelectRoot.Q<Button>("PREVIOUS").RegisterCallback<ClickEvent>(OnLevelSelect);
        lvlSelectRoot.Q<Button>("NEXT").RegisterCallback<ClickEvent>(OnLevelSelect);
        
        islandTitle = lvlSelectRoot.Q<Label>("Island_Title");


        SetTitle();
    }

    void OnLevelSelect(ClickEvent evt) {
        
        var btn = (Button)evt.target;


        // Set CamID
        previousCamID = currentCamID;
        switch (btn.text) {
            case "<":
                if (currentCamID == 0) { currentCamID = maxCamID; }
                else { currentCamID--; }
                break;
            case ">":
                if (currentCamID == maxCamID) { currentCamID = 0; }
                else { currentCamID++; }
                break;
            case "Select":
                StartCoroutine(LoadLevel());
                return;
        }

        // Change Camera
        islandCams[currentCamID].SetActive(true);
        islandCams[previousCamID].SetActive(false);

        SetTitle();
    }

    void SetTitle() {
        islandTitle.text = currentCamID switch {
            0 => "Pirate",
            _ => "???"
        };

        if (islandTitle.text == "???") {
            btnSelect.SetEnabled(false);
            btnSelect.focusable = false;
        } else {
            btnSelect.SetEnabled(true);
        }
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
