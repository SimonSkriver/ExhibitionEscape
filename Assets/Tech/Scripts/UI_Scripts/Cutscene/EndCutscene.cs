using UnityEngine;

public class EndCutscene : MonoBehaviour
{
    void OnEnable()
    {
        UI_Manager.Instance.ShowCredits();
        Time.timeScale = 0;
    }
}
