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
    private InputAction scrollAction;
    public Vector2 moveInput { get; private set; }

    InputActionMap playerMap;
    GameObject playerCam;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        player = GameObject.FindWithTag("Player");
        playerInput = GetComponent<PlayerInput>();
        playerUse = player.GetComponent<PlayerUse>();
        playerMovement = player.GetComponent<PlayerController>();
        playerInteract = player.GetComponent<PlayerInteract>();
        playerUse = player.GetComponent<PlayerUse>();

        playerMap = InputSystem.actions.FindActionMap("Player");
        playerCam = player.transform.GetChild(3).gameObject;

        moveAction = playerInput.actions.FindAction("Move");
        scrollAction = playerInput.actions.FindAction("Scroll");

        playerInput.actions.FindAction("Jump").performed += ctx => playerMovement.Jump();
        playerInput.actions.FindAction("Sprint").performed += ctx => playerMovement.Sprint();
        playerInput.actions.FindAction("Sprint").canceled += ctx => playerMovement.Sprint();
        playerInput.actions.FindAction("Use").performed += ctx => playerUse.Use();
        playerInput.actions.FindAction("Interact").performed += ctx => playerInteract.Interact();

        playerInput.actions.FindAction("Slot1").performed += ctx => InventoryManager.Instance.SelectSlot(0);
        playerInput.actions.FindAction("Slot2").performed += ctx => InventoryManager.Instance.SelectSlot(1);
        playerInput.actions.FindAction("Slot3").performed += ctx => InventoryManager.Instance.SelectSlot(2);
        playerInput.actions.FindAction("Slot4").performed += ctx => InventoryManager.Instance.SelectSlot(3);
        playerInput.actions.FindAction("Slot5").performed += ctx => InventoryManager.Instance.SelectSlot(4);

        scrollAction.performed += ctx =>
        {
            Vector2 scrollValue = ctx.ReadValue<Vector2>();

            if (scrollValue.y != 0)
            {
                InventoryManager.Instance.ScrollSelect(scrollValue.y);
            }
        };

        playerInput.actions.FindAction("Pause").performed += ctx => UI_Manager.Instance.PauseMenuUI();

    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void OnEnable() {
        playerMap.Enable();
        playerInput.actions.FindAction("Pause").Enable();
    }
    private void OnDisable() {
        playerMap.Disable();
        playerInput.actions.FindAction("Pause").Disable();
    }

    public void EnablePlayer() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        playerMap.Enable();
        playerCam.SetActive(true);
    }
    public void DisablePlayer() {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        playerMap.Disable();
        playerCam.SetActive(false);
    }
}