using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private Transform eyes;

    [Header ("Settings")]
    [SerializeField] private float reach = 5f;

    private IInteractable interactableObject;
    private LayerMask layerMask;
    private Outline[] currentOutline;

    void Awake()
    {
        eyes = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        layerMask = LayerMask.GetMask("Player");
    }

    void Update()
    {
        Ray ray = new Ray(eyes.position, eyes.forward);
        if (Physics.SphereCast(ray, 0.2f, out RaycastHit hit, reach, ~layerMask) && hit.collider.TryGetComponent(out IInteractable currentObj))
        {
            if (currentObj != interactableObject) // Ensure we only assign the current object once
            {
                Clear(); // Ensure no other outlines are drawn and variables are clear before drawing and assigning
                interactableObject = currentObj;
                currentOutline = hit.collider.transform.GetComponentsInChildren<Outline>();
                DrawOutline(currentOutline);
            }
        }
        else
        {
            Clear();
        }

        Debug.DrawRay(eyes.position, eyes.forward * reach, Color.blue);
    }

    void DrawOutline(Outline[] outline)
    {
        foreach (Outline outlineComponent in outline)
        {
            outlineComponent.enabled = true;
        }
    }

    void Clear()
    {
        if (currentOutline != null)
        {
            foreach (Outline outlines in currentOutline)
            {
                outlines.enabled = false;
            }
        }
        currentOutline = null;
        interactableObject = null;
    }

    public void Interact()
    {
        if (interactableObject == null) return;

        interactableObject.Interact();
        
        GetComponent<Animator>().SetTrigger("PICK_UP");
    }        
}