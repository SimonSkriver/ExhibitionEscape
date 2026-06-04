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
    BoatCutscene cutscene;

    bool isGamePaused;
    public bool canPauseGame;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        player = GameObject.FindWithTag("Player");
        playerInput = GetComponent<PlayerInput>();

        playerUse = player.GetComponent<PlayerUse>();
        playerMovement = player.GetComponentInParent<PlayerController>();
        playerInteract = player.GetComponentInParent<PlayerInteract>();
        playerUse = player.GetComponentInParent<PlayerUse>();

        playerMap = InputSystem.actions.FindActionMap("Player");
        playerCam = GameObject.FindWithTag("CMcam");

        moveAction = playerInput.actions.FindAction("Move");
        scrollAction = playerInput.actions.FindAction("Scroll");

        playerInput.actions.FindAction("Jump").performed += ctx => playerMovement.Jump();
        playerInput.actions.FindAction("Sprint").performed += ctx => playerMovement.Sprint();
        playerInput.actions.FindAction("Sprint").canceled += ctx => playerMovement.Sprint();
        playerInput.actions.FindAction("Sprint_Toggle").performed += ctx => playerMovement.Sprint();
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

        playerInput.actions.FindAction("Pause").performed += ctx => Pause();
        cutscene = FindAnyObjectByType<BoatCutscene>();
    }



    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void OnEnable() {
        playerInput.actions.FindAction("Pause").Enable();
        playerInput.actions.FindAction("Jump").Enable();
        playerInput.actions.FindAction("Slot1").Enable();
        playerInput.actions.FindAction("Slot2").Enable();
        playerInput.actions.FindAction("Slot3").Enable();
        playerInput.actions.FindAction("Slot4").Enable();
        playerInput.actions.FindAction("Slot5").Enable();
    }
    private void OnDisable() {
        playerInput.actions.FindAction("Pause").Disable();
        playerInput.actions.FindAction("Jump").Disable();
        playerInput.actions.FindAction("Slot1").Disable();
        playerInput.actions.FindAction("Slot2").Disable();
        playerInput.actions.FindAction("Slot3").Disable();
        playerInput.actions.FindAction("Slot4").Disable();
        playerInput.actions.FindAction("Slot5").Disable();
    }

    public void EnablePlayer() {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        
        playerInput.enabled = true;
        playerCam.SetActive(true);
    }
    public void DisablePlayer() {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        playerInput.enabled = false;
        playerCam.SetActive(false);

        if (DialogueManager.Instance.isDialoguePlaying) {
            playerInput.actions.FindAction("Interact").Enable();
        }
    }

    public void Pause() {
        if (!isGamePaused && canPauseGame) {
            isGamePaused = true;
            DisablePlayer();
            Time.timeScale = 0;
            UI_Manager.Instance.ShowPauseMenuUI();
            SFXManager.PlayEffect("PauseMenu");

        } else if (canPauseGame){
            isGamePaused = false;
            SFXManager.PlayEffect("BackButton");
            if (!cutscene.inCutscene) EnablePlayer();
            Time.timeScale = 1;
            UI_Manager.Instance.HidePauseMenuUI();
            UI_Manager.Instance.HideSettingsMenuUI();
        }  
    }
}