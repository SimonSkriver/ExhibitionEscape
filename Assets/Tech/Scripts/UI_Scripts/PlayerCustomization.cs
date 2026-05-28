using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCustomization : MonoBehaviour
{
    TemplateContainer root;
    Material m_skin, m_shorts;
    List<Button> customButtons = new List<Button>();
    List<Slider> customSliders = new List<Slider>();
    Transform neckBone;

    private void Awake() {
        root = UI_Manager.Instance.customizeRoot;
        customButtons = root.Query<Button>().ToList();
        foreach (var btn in customButtons) {
            btn.RegisterCallback<ClickEvent>(OnColorButton);
        }

        customSliders = root.Query<Slider>().ToList();
        foreach (var sld in customSliders) {
            sld.RegisterCallback<ChangeEvent<float>>(OnCustomizationSlider);
        }

        m_skin = Resources.Load<Material>("Art/Materials/PlayerSkin");
        m_shorts = Resources.Load<Material>("Art/Materials/PlayerShorts");
        neckBone = GameObject.FindGameObjectWithTag("Player").transform.Find("ShortsMan/Main/Bone.001/Bone.002/Bone.003/Chest/Neck");
    }

    void OnColorButton(ClickEvent evt) {
        Button btn = (Button)evt.target;

        switch (btn.name) {
            case "SkinColor":
                m_skin.color = btn.resolvedStyle.backgroundColor;
                break;
            case "ShortsColor":
                m_shorts.color = btn.resolvedStyle.backgroundColor;
                break;
            case "ConfirmPlayerCustomization":
                UI_Manager.Instance.HidePlayerCustomizeUI();
                break;
        }
    }

    void OnCustomizationSlider(ChangeEvent<float> evt) {
        Slider sld = (Slider)evt.target;

        switch (sld.tooltip) {
            case "Head Size":
                neckBone.localScale = new Vector3(sld.value, sld.value, sld.value);;
                break;
            case "Neck Length":
                Vector3 neckPos = neckBone.localPosition;
                neckPos.y = sld.value;
                neckBone.localPosition = neckPos;
                break;
        }
    }
}
