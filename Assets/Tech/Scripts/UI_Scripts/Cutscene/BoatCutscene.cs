using UnityEngine;
using UnityEngine.Playables;

public class BoatCutscene : MonoBehaviour, IInteractable
{
    [SerializeField] PlayableDirector cutscene;
    PirateHat hat;
    public bool inCutscene { get; private set; }

    void Awake()
    {
        hat = FindAnyObjectByType<PirateHat>();
    }

    public void Interact()
    {
        if (!hat.hasHat) return;
        cutscene.Play();
        inCutscene = true;
    }

    public bool ShowOutline()
    {
        return true;
    }
}