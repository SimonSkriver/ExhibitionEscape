using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public event Action Boat;
    public void StartBoatEvent() => Boat?.Invoke();

    public event Action CapHat;
    public void GiveCapHat() => CapHat?.Invoke();
}