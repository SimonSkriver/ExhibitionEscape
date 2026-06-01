using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Cinemachine;

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
                SFXManager.PlayEffect("ConfirmColor");
                UI_Manager.Instance.HidePlayerCustomizeUI();
                break;
        }
    }

    void OnCustomizationSlider(ChangeEvent<float> evt) {
        Slider sld = (Slider)evt.target;

        switch (sld.tooltip) {

            case "Head Size":
                // Change scale of head (neckBone)
                neckBone.localScale = new Vector3(sld.value, sld.value, sld.value);;

                // Find slider value in percent
                float sldValPercent = (sld.value - sld.lowValue) / (sld.highValue - sld.lowValue);

                // Lerp between given numbers
                float xScreenPos = Mathf.Lerp(-0.1f, 0f, sldValPercent);
                float yScreenPos = Mathf.Lerp(-0.15f, 0.08f, sldValPercent);
                float targetOffset = Mathf.Lerp(-0.35f, 1.5f, sldValPercent);

                // Set Camera Target Offset
                GameManager.customCam.GetComponent<CinemachineOrbitalFollow>().TargetOffset.z = targetOffset;

                // Set Camera Screen Position
                CinemachineRotationComposer crc = GameManager.customCam.GetComponent<CinemachineRotationComposer>();
                crc.Composition.ScreenPosition.x = xScreenPos;
                crc.Composition.ScreenPosition.y = yScreenPos;
                break;

            case "Neck Length":
                Vector3 neckPos = neckBone.localPosition;
                neckPos.y = sld.value;
                neckBone.localPosition = neckPos;
                break;
        }
    }
}
