using UnityEngine;

public class DestroyableLog : MonoBehaviour, IInteractable
{
 public void Interact()
    {
        DestroyLog();
    }
    
    public void DestroyLog()
    {
        Destroy(gameObject);
    }
}