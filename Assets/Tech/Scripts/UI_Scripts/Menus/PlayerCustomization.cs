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
    Transform neckBone, hatAnchor;
    float nextSFXtime;

    private void Awake() {
        root = UI_Manager.Instance.customizeRoot;
        customButtons = root.Query<Button>().ToList();
        foreach (var btn in customButtons) {
            btn.RegisterCallback<ClickEvent>(OnColorButton);
            btn.RegisterCallback<NavigationSubmitEvent>(OnColorButton);
            btn.RegisterCallback<PointerEnterEvent>(OnHoverSettings);
        }

        customSliders = root.Query<Slider>("CustomizationSlider").ToList();
        foreach (var sld in customSliders) {
            sld.RegisterCallback<ChangeEvent<float>>(OnCustomizationSlider);
        }

        m_skin = Resources.Load<Material>("Art/Materials/PlayerSkin");
        m_shorts = Resources.Load<Material>("Art/Materials/PlayerShorts");

        neckBone = GameObject.FindGameObjectWithTag("Player").transform.Find("ShortsMan/Main/Bone.001/Bone.002/Bone.003/Chest/Neck");
        hatAnchor = neckBone.Find("Head/Head_end/HatAnchor");
    }
    public void OnHoverSettings(PointerEnterEvent pointerEnterEvent) => SFXManager.PlayEffect("HoverSettings");
    public void PlaySliderSound()
    {
        if(nextSFXtime < Time.unscaledTime)
        {
           SFXManager.PlayEffect("SliderSound");
           nextSFXtime = Time.unscaledTime + 0.03f; 
        }
    }

    // Mouse Click
    void OnColorButton(ClickEvent evt) => ColorButton((Button)evt.target);

    // Controller Click
    void OnColorButton(NavigationSubmitEvent evt) => ColorButton((Button)evt.target);

    void ColorButton(Button btn) {
        switch (btn.name) {
            case "SkinColor":
                SFXManager.PlayEffect("ClickButton");
                m_skin.color = btn.resolvedStyle.backgroundColor;
                break;
            case "ShortsColor":
                SFXManager.PlayEffect("ClickButton");
                m_shorts.color = btn.resolvedStyle.backgroundColor;
                break;
            case "ConfirmPlayerCustomization":
                SFXManager.PlayEffect("ConfirmColor");
                UI_Manager.Instance.HidePlayerCustomizeUI();
                PlayerStats.Instance.savedColor = m_skin.color;
                break;
        }
    }

    void OnCustomizationSlider(ChangeEvent<float> evt) {
        Slider sld = (Slider)evt.target;
        PlaySliderSound();

        switch (sld.tooltip) {

            case "Head Size":
                // Change scale of head (neckBone)
                neckBone.localScale = new Vector3(sld.value, sld.value, sld.value);;

                // Find slider value in percent
                float sldValPercent = (sld.value - sld.lowValue) / (sld.highValue - sld.lowValue);

                // Lerp between given numbers
                float xScreenPos = Mathf.Lerp(-0.1f, 0.04f, sldValPercent);
                float yScreenPos = Mathf.Lerp(-0.15f, 0.07f, sldValPercent);
                float targetOffset = Mathf.Lerp(0f, -1.71f, sldValPercent);
                
                // Sets the hat position better :)
                if (sldValPercent >= 0.1f) sldValPercent += 0.5f;
                
                float hatPosX = Mathf.Lerp(-0.08f, 0.11f, sldValPercent);
                float hatPosY = Mathf.Lerp(-0.8f, -0.025f, sldValPercent);
                
                // Set Hat Position
                hatAnchor.localPosition = new Vector3(hatPosX, hatPosY, 0);
                Debug.Log(hatAnchor.localPosition);

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
