using UnityEngine;
using UnityEngine.Localization.Settings;

public class Localization : MonoBehaviour
{
    public static void ChangeLanguage(string ISO_639) {
        // Get the locale index
        int i = ISO_639 switch {
            "EN" => 0,
            "DA" => 2,
            _ => 0
        };

        // Change the Locale (language)
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];

        // Change Dialogue language
        TextAsset asset = Resources.Load<TextAsset>($"Dialogue/{ISO_639}/{ISO_639}_main");
        DialogueManager.Instance.SetInkJSON(asset);
    }
}
