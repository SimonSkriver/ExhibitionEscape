using UnityEngine;

public class InteractTester : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("You just interacted with a thing");
    }
}
