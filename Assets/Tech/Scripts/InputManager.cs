using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header ("Info")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerController playerMovement;

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

        moveAction = playerInput.actions.FindAction("Move");

        playerInput.actions.FindAction("Jump").performed += ctx => playerMovement.Jump();
        playerInput.actions.FindAction("Attack").performed += ctx => playerMovement.Attack();
        playerInput.actions.FindAction("Interact").performed += ctx => playerMovement.Interact();
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