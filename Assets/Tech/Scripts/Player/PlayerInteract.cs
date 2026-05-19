using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private Transform eyes;

    [Header ("Settings")]
    [SerializeField] private float reach = 5f;

    private IInteractable obj;
    public MeshRenderer[] outlines;

    void Awake()
    {
        eyes = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    void Update()
    {
        Ray ray = new Ray(eyes.position, eyes.forward);
        if (Physics.SphereCast(ray, 0.2f, out RaycastHit hit, reach) && hit.collider.TryGetComponent(out IInteractable currentObj))
        {
            obj = currentObj;
            
            outlines = hit.collider.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in outlines)
            {
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }
        }
        else
        {
            obj = null;
        }
    }

    public void Interact()
    {
        if (obj == null) return;
        obj.Interact();
    }
}