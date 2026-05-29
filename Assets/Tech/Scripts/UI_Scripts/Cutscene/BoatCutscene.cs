using UnityEngine;
using UnityEngine.Playables;

public class BoatCutscene : MonoBehaviour, IInteractable
{
    [SerializeField] PlayableDirector cutscene;
    public bool inCutscene { get; private set; }

    public void Interact()
    {
        cutscene.Play();
        inCutscene = true;
    }

    public bool ShowOutline()
    {
        return true;
    }
}