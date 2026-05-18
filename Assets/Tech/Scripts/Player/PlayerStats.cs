using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Stats")]
    public float Health;
    public float Saturation;
    public float MovementSpeed;
    public float JumpPower;
    public float Strength;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Health = 100;
            Saturation = 100;
        }
    }
}
