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
    [SerializeField] private PlayerUse playerUse;

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
        playerUse = player.GetComponent<PlayerUse>();

        moveAction = playerInput.actions.FindAction("Move");

        playerInput.actions.FindAction("Jump").performed += ctx => playerMovement.Jump();
        playerInput.actions.FindAction("Sprint").performed += ctx => playerMovement.Sprint();
        playerInput.actions.FindAction("Sprint").canceled += ctx => playerMovement.Sprint();
        playerInput.actions.FindAction("Use").performed += ctx => playerUse.Use();
        playerInput.actions.FindAction("Interact").performed += ctx => playerInteract.Interact();
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }
}