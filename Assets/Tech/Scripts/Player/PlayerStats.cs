using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Stats")]
    int Health;
    public float Saturation;
    public float MovementSpeed;
    public float SprintSpeed;
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
    
    public void AddHealth(int health) {
        Debug.Log("Previous Player Health: " + Health);
        Health += health;
        if (Health > 100) { Health = 100; }
        PlayerHUD.Instance.UpdateHealthUI(Health);
    }

    public void RemoveHealth(int health) {
        Health -= health;
        PlayerHUD.Instance.UpdateHealthUI(Health);
    }
}
