using UnityEngine;

public class DisableplayerCutscene : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("Disabling player input");
        InputManager.Instance.DisablePlayer();
    }

    void OnDisable()
    {
        InputManager.Instance.EnablePlayer();
    }
}