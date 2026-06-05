using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering.Universal;

public class LocalizationDecal : MonoBehaviour
{
    [SerializeField] string decalName;
    Material decalMat;

    private void Start() => ChangeDecal();
    private void OnEnable() => LocalizationSettings.SelectedLocaleChanged += loc => ChangeDecal();
    private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= loc => ChangeDecal();

    void ChangeDecal() {
        string ISO_639 = LocalizationSettings.SelectedLocale.SortOrder switch {
            0 => "EN",
            2 => "DA",
            3 => "SV",
            _ => "EN"
        };

        decalMat = Resources.Load<Material>($"Art/UI/Sprites/Decals/{ISO_639}_{decalName}");

        gameObject.GetComponent<DecalProjector>().material = decalMat;
    }
}
