using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Transform
        CameraManager;

    private void Awake() {
        CameraManager = transform.Find("CameraManager");
    }
}
