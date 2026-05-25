using System.Collections;
using UnityEngine;

public class PlayerFallDamage : MonoBehaviour
{
    [Header ("Settings")]
    [Tooltip ("How long time the player must spend in the air before taking fall damage")]
    [SerializeField] float dmgThreshold = 1.2f;


    PlayerController player;
    float falltime;
    float counter;
    int fallDamage;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        falltime = player.isGrounded ? falltime = 0f : falltime += counter * Time.deltaTime;
        
    }
}