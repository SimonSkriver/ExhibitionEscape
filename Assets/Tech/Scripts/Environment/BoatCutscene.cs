using UnityEngine;
using UnityEngine.Playables;

public class BoatCutscene : MonoBehaviour, IInteractable
{
    [SerializeField] Transform anchor;
    [SerializeField] Transform player;
    [SerializeField] PlayableDirector cutscene;

    public void Interact()
    {
        Debug.Log("Lets go");
        cutscene.Play();
    }

    public bool ShowOutline()
    {
        return true;
    }
}