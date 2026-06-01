using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("Stats")]
    public int Health { get; private set; }
    public float MovementSpeed;
    public float SprintSpeed;
    public float JumpPower;
    public bool canTakeFallDamage = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Health = 100;
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
        SFXManager.PlayEffect("HurtSound");
        PlayerHUD.Instance.UpdateHealthUI(Health);
        Debug.Log(Health);
        if (Health <= 0)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        }
    }
}