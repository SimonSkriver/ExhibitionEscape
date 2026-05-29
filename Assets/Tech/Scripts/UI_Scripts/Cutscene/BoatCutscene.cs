using UnityEngine;
using UnityEngine.Playables;

public class BoatCutscene : MonoBehaviour, IInteractable
{
    [SerializeField] PlayableDirector cutscene;

    public void Interact()
    {
        cutscene.Play();
    }

    public bool ShowOutline()
    {
        return true;
    }
}