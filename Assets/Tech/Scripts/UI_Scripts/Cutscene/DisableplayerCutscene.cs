using UnityEngine;

public class DisableplayerCutscene : MonoBehaviour
{
    public bool inCutscene { get; private set; }
    private Transform player;
    [SerializeField] Transform targetPosition;
    [SerializeField] BoxCollider boatCollider;

    void OnEnable()
    {
        InputManager.Instance.DisablePlayer();
        boatCollider.enabled = false;
        player = GameObject.FindWithTag("Player").transform;   
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<PlayerInteract>().Clear();
        player.GetComponent<PlayerInteract>().enabled = false;
        player.SetPositionAndRotation(targetPosition.position, targetPosition.rotation);
        player.SetParent(targetPosition);
        UI_Manager.Instance.HidePlayerHUD();
        inCutscene = true;
    }

    void OnDisable()
    {
        InputManager.Instance.EnablePlayer();
        inCutscene = false;
    }
}