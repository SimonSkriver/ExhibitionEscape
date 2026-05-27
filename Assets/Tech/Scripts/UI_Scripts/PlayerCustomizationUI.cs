using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCustomizationUI : MonoBehaviour
{
    TemplateContainer root;
    public Material m_skin, m_shorts;
    List<Button> customButtons = new List<Button>();

    private void Awake() {
        root = UI_Manager.Instance.customizeRoot;
        customButtons = root.Query<Button>().ToList();
        foreach (var btn in customButtons) {
            btn.RegisterCallback<ClickEvent>(OnPlayerCustomize);
        }

        m_skin = Resources.Load<Material>("Art/Materials/PlayerSkin");
        m_shorts = Resources.Load<Material>("Art/Materials/PlayerShorts");
    }

    void OnPlayerCustomize(ClickEvent evt) {
        Button btn = (Button)evt.target;
        int index = btn.tabIndex;

        switch (btn.name) {
            case "SkinColor":
                m_skin.color = btn.style.backgroundColor.value;
                Debug.Log(btn.style.backgroundColor.value);
                break;
            case "ShortsColor":
                m_shorts.color = btn.style.backgroundColor.value;
                break;
        }
    }
}
