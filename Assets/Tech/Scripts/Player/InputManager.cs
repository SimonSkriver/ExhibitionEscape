using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header ("Info")]
    [SerializeField] private PlayerInput playerInput;
    public bool playerCanMove;

    [Header ("Scripts")]
    [SerializeField] private PlayerController playerMovement;


    void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }

        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerController>();

        playerInput.actions.FindAction("Jump").performed += ctx => playerMovement.Move();
        playerInput.actions.FindAction("Attack").performed += ctx => playerMovement.Attack();
        playerInput.actions.FindAction("Interact").performed += ctx => playerMovement.Interact();
    }

    void Update()
    {
        if (!playerCanMove) return;
        
    }
}