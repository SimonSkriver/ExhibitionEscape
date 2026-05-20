using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] Transform cameraTransform;

    void Start()
    {
        cameraTransform = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    void LateUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(forward);
        }
    }
}