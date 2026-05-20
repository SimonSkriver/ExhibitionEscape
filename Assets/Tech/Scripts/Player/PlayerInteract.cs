using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private Transform eyes;

    [Header ("Settings")]
    [SerializeField] private float reach = 5f;

    private IInteractable obj;
    private LayerMask layerMask;
    public MeshRenderer[] outlines;

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

Debug.DrawRay(eyes.position, eyes.forward * reach, Color.blue);

    }

    public void Interact()
    {
        if (obj == null) return;
        obj.Interact();
    }        
}