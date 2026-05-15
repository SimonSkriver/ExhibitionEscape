using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private CharacterController controller;

    [Header ("Settings")]
    [SerializeField] private float gravity = -10f; 
    
    private bool isGrounded;
    private bool isSprinting;
    private Vector3 playerVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
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
        }
    }

    public void Attack()
    {
        
    }

    public void Interact()
    {
        
    }

    void HandleGravity()
    {
        //Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;

        //Keep player grounded by applying slight negative force
        if (isGrounded && playerVelocity.y < 0) 
        {
            playerVelocity.y = -2f;
        }
    }

    void Move()
    {
        //Read input and move speed
        float moveSpeed = PlayerStats.Instance.MovementSpeed;
        Vector2 moveInput = InputManager.Instance.moveInput;

        //Save moveinput in a Vector3 to combine with vertical velocity
        Vector3 horizontalInput = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;

        //Combine horizontal and vertical movement and apply movement
        Vector3 moveDirection = horizontalInput + (playerVelocity.y * Vector3.up);
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}