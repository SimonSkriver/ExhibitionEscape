using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Stats")]
    public float MaxStats { get; set; }
    public float Health { get; set; }
    public float Saturation { get; set; }
    public float Hunger { get; set; }
    public float AttackDamage { get; set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MaxStats = 100;
            Health = 100;
            Saturation = 100;
            Hunger = 100;
            AttackDamage = 5;
        }
    }
}
