using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Transform
        CameraManager,
        customCam;

    private void Awake() {
        CameraManager = transform.Find("CameraManager");
        customCam = CameraManager.GetChild(1);
    }
}
