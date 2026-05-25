using Unity.Cinemachine;
using UnityEngine;

public class FOVChanger : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private PlayerController player;

    [Header ("Settings")]
    [Tooltip ("How much the FOV should increase when sprinting")]
    [SerializeField] float fovIncrease = 10f;
    [SerializeField] float lerpSpeed = 5f;

    private float walkFOV = 60;
    private float sprintFOV;
    float FOV;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        FOV = GetComponent<CinemachineCamera>().Lens.FieldOfView;
    }

    void Start()
    {
        sprintFOV = walkFOV + fovIncrease;
    }

    void Update()
    {
        if (player.isSprinting)
        {
            FOV = sprintFOV;
        }
        else
        {
            FOV = walkFOV;
        }
    }
}