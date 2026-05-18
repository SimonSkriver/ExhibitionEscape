using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header ("Info")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerController playerMovement;
    [SerializeField] private PlayerInteract playerInteract;

    private InputAction moveAction;
    public Vector2 moveInput { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        player = GameObject.FindWithTag("Player");
        playerInput = GetComponent<PlayerInput>();
        playerMovement = player.GetComponent<PlayerController>();
        playerInteract = player.GetComponent<PlayerInteract>();

        moveAction = playerInput.actions.FindAction("Move");

        playerInput.actions.FindAction("Jump").performed += ctx => playerMovement.Jump();
        playerInput.actions.FindAction("Use").performed += ctx => playerInteract.Use();
        playerInput.actions.FindAction("Interact").performed += ctx => playerInteract.Interact();
    }

    void Update()
    {
        ReadMovementInput();
    }

    void ReadMovementInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }
}