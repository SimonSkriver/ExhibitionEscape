using UnityEngine;

public class DestroyTree_Dialogue : MonoBehaviour
{
    private void OnDestroy() {
        Debug.Log("Palm Destroyed");

        DialogueManager.Instance.ChangeInkVariable("isPalmTreeDestroyed", "true");
    }
}
