using Unity.Cinemachine;
using UnityEngine;

public class FOVChanger : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private PlayerController player;
    [SerializeField] private CinemachineCamera cam;

    [Header ("Settings")]
    [Tooltip ("How fast the camera zooms in and out")]
    [SerializeField] float lerpSpeed = 5f;
    [SerializeField] private float walkFOV = 60;
    [SerializeField] private float sprintFOV = 80;


    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        cam = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        float currentFOV = cam.Lens.FieldOfView;

        if (player.isSprinting)
        {
            cam.Lens.FieldOfView = Mathf.Lerp(currentFOV, sprintFOV, Time.deltaTime * lerpSpeed);
        }
        else
        {
            cam.Lens.FieldOfView = Mathf.Lerp(currentFOV, walkFOV, Time.deltaTime * lerpSpeed);
        }
    }
}