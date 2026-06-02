using UnityEngine;
using System.Collections;
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

    [Header("Take Damage Effect")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float flickerAmount = 3;
    [SerializeField] private float flickerDelay = 0.1f;
    public Color savedColor;
    public Color redColor;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Health = 100;
        }

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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
        StartCoroutine(ColorFlicker());
        if (Health <= 0)
        {
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        }
    }

    public IEnumerator ColorFlicker()
    {
        for (int i = 0; i < flickerAmount; i++)
        {
            skinnedMeshRenderer.material.color = redColor;
            yield return new WaitForSeconds(flickerDelay);
            skinnedMeshRenderer.material.color = savedColor;
            yield return new WaitForSeconds(flickerDelay);
        }
    }
}