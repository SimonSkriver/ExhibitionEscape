using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Info")]
    [SerializeField] private CharacterController controller;

    [Header ("Settings")]
    [SerializeField] private float gravity;
    
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
    }

    public void Jump()
    {
        if (isGrounded)
        {
            float jumpPower = PlayerStats.Instance.JumpPower;
            playerVelocity.y = Mathf.Sqrt(jumpPower * -3f * gravity);
        }
    }

    public void Attack()
    {
        
    }

    public void Interact()
    {
        
    }

    void Move()
    {
        Vector2 moveInput = InputManager.Instance.moveInput;
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        float moveSpeed = PlayerStats.Instance.MovementSpeed;
        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}