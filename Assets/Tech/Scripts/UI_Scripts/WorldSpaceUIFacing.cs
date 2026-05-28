using UnityEngine;

public class WorldSpaceUIFacing : MonoBehaviour
{
    Transform cam;
    void Start() => cam = Camera.main.transform;
    void Update() => transform.rotation = Quaternion.LookRotation(cam.forward, cam.up);
}
