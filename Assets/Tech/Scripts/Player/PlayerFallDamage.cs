using UnityEngine;

public class PlayerFallDamage : MonoBehaviour
{
    [Header ("Settings")]
    [Tooltip ("The minimum velocity before fall damage is applied")]
    [SerializeField] private float threshold = 3.5f;
    [SerializeField] private float dmgMultiplier = 2.5f;
    
    [Header ("Info")]
    [SerializeField] private float velocity;

    private PlayerController player;
    private bool wasGrounded;

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!wasGrounded && player.isGrounded && !player.isSliding) //If player wasn't grounded last frame, but now is grounded
        {
            if (velocity < threshold) return;
            CalculateFallDamage(velocity);
        }
        
        velocity = -player.playerVelocity.y;
    }

    void LateUpdate()
    {
        wasGrounded = player.isGrounded; //Set wasGrounded to true, if player was grounded by the end of last update loop
    }

    void CalculateFallDamage(float fallSpeed)
    {
        //Calculate
        int fallDamage = Mathf.RoundToInt(fallSpeed * dmgMultiplier);
        //Apply
        PlayerStats.Instance.RemoveHealth(fallDamage);
    }
}