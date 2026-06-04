using UnityEngine;
using UnityEngine.Playables;

public class BoatCutscene : MonoBehaviour
{
    [SerializeField] PlayableDirector cutscene;
    //PirateHat hat;
    public bool inCutscene { get; private set; }

    void Awake()
    {
        //hat = FindAnyObjectByType<PirateHat>();

        // Bind the cutscene to the Boat event
        // When Boat event is triggered, TryStartCutscene() will run
        EventManager.Instance.Boat += TryStartCutscene;
    }

    public void TryStartCutscene()
    {
        //if (!hat.hasHat) return;
        cutscene.Play();
        inCutscene = true;
        UI_Manager.Instance.HidePlayerHUD();
    }
}