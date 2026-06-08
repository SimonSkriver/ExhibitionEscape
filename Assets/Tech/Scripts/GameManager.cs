using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Transform
        CameraManager,
        customCam,
        thirdPersonCam;

    private void Awake() {
        CameraManager = transform.Find("CameraManager");
        customCam = CameraManager.GetChild(1);
        thirdPersonCam = CameraManager.GetChild(2);
    }
}
