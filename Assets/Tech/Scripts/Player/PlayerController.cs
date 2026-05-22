using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform orientation;

    [Header("Animation")]
    public Animator A { get; private set; }
    

    [Header ("Settings")]
    [SerializeField] private float gravity = -10f; 
    [SerializeField] private float turnSpeed = 5f; 
    
    private bool isGrounded;
    private bool isSprinting;
    bool isMoving;
    private float moveSpeed;
    private Vector3 playerVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        orientation = GameObject.FindWithTag("Orientation").GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        A = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        Move();
        HandleGravity();
    }

    public void Jump()
    {
        if (isGrounded)
        {
            float jumpPower = PlayerStats.Instance.JumpPower;
            playerVelocity.y = Mathf.Sqrt(jumpPower * -1f * gravity);
            A.SetFloat("playerVelocity", playerVelocity.y);
        }
    }

    void HandleGravity()
    {
        //Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;

        //Keep player grounded by applying slight negative force
        if (isGrounded && playerVelocity.y < 0) 
        {
            playerVelocity.y = -2f;
            A.SetFloat("playerVelocity", playerVelocity.y);
        }
    }

    void Move()
    {
        //Read input and move speed
        moveSpeed = isSprinting ? PlayerStats.Instance.SprintSpeed : PlayerStats.Instance.MovementSpeed; 
        Vector2 moveInput = InputManager.Instance.moveInput;

        //Save moveinput in a Vector3 to combine with vertical velocity
        Vector3 horizontalInput = orientation.right * moveInput.x + orientation.forward * moveInput.y;

        //Combine horizontal and vertical movement and apply movement
        Vector3 moveDirection = horizontalInput + (playerVelocity.y * Vector3.up);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        //If there's horizontal input, lerp from current rotation to the input value rotation
        if (horizontalInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalInput);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        
        // Walking or Idle animation
        if (moveDirection != new Vector3(0, playerVelocity.y, 0)) {
            A.SetBool("isWalking", true);
            isMoving = true;
        } else {
            A.SetBool("isWalking", false);
            isMoving = false;
        }
    }

    public void Sprint()
    {
        isSprinting = !isSprinting;

        // Sprint Animation
        if (isMoving) {
            A.SetBool("isRunning", isSprinting);
        } else {
            A.SetBool("isRunning", false);
        }
    }
}