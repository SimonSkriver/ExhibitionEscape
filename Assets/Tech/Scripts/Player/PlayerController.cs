using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private Transform orientation;
    public CharacterController controller { get; private set; }
    public bool isGrounded { get; private set; }

    [Header("Animation")]
    public Animator A { get; private set; }
    
    [Header ("Settings")]
    [SerializeField] private float gravity = -10f; 
    [SerializeField] private float turnSpeed = 5f; 
    
    [HideInInspector] public Vector3 playerVelocity;
    Vector3 slopeSlideVelocity;
    Vector3 moveDirection;
    public bool canMove { get; set; }
    public bool isSprinting { get; private set; }
    public bool isMoving { get; private set; }
    public bool isSliding { get; private set; }


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        orientation = GameObject.FindWithTag("Orientation").GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        A = GetComponent<Animator>();
        
        canMove = true;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        
        if (canMove)
        {
            Move();
            HandleGravity();  
        }
    }

    public void Jump()
    {
        if (isGrounded && !isSliding)
        {
            SFXManager.PlayEffect("Jump");
            float jumpPower = PlayerStats.Instance.JumpPower;
            playerVelocity.y = Mathf.Sqrt(jumpPower * -1f * gravity);
            A.SetFloat("playerVelocity", playerVelocity.y);
        }
    }

    void HandleGravity()
    {
        //Apply gravity
        playerVelocity.y += gravity * Time.deltaTime;

        SetSlopeSlideVelocity();

        //Keep player grounded by applying slight negative force
        if (isGrounded && playerVelocity.y < 0) 
        {
            if (!isSliding) {
                playerVelocity.y = -2f;
            }
            A.SetFloat("playerVelocity", playerVelocity.y);
        }
    }

    void Move()
    {
        //Read input and move speed
        float moveSpeed = isSprinting ? PlayerStats.Instance.SprintSpeed : PlayerStats.Instance.MovementSpeed; 
        Vector2 moveInput = InputManager.Instance.moveInput;

        //Save moveinput in a Vector3 to combine with vertical velocity
        Vector3 horizontalInput = orientation.right * moveInput.x + orientation.forward * moveInput.y;

        //Combine horizontal and vertical movement and apply movement
            moveDirection = horizontalInput + (playerVelocity.y * Vector3.up);
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (isSliding) {
            Vector3 velocity = slopeSlideVelocity;
            velocity.y = playerVelocity.y;
            controller.Move(velocity * Time.deltaTime);
        }

        //If there's horizontal input, lerp from current rotation to the input value rotation
        if (horizontalInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalInput);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }

        // Walking or Idle animation
        if (moveDirection != new Vector3(0, playerVelocity.y, 0)) 
        {
            A.SetBool("isWalking", true);
            isMoving = true;
            if (isSprinting)
            {
                A.SetBool("isRunning", isSprinting);
            }
            else if (A.GetBool("isRunning") != isSprinting)
            {
                A.SetBool("isRunning", isSprinting);
            }
        } 
        else 
        {
            A.SetBool("isWalking", false);
            isMoving = false;
            isSprinting = false;
        }
    }

    public void Sprint()
    {
        if (!isMoving) return;
        isSprinting = !isSprinting;

        if (isSprinting && isMoving)
        {
            SFXManager.PlayEffect("Sprint");
        }
    }

    void SetSlopeSlideVelocity() 
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5)) 
        {
            // Get angle of the slope
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle >= controller.slopeLimit) 
            {
                isSliding = true;
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, playerVelocity.y, 0), hitInfo.normal);
                return;
            }
        }

        slopeSlideVelocity = Vector3.zero;
        isSliding = false;
    }
}