using UnityEngine;

public class PlayerUse : MonoBehaviour
{
    public void Use()
    {
        GetComponent<Animator>().SetTrigger("PICK_UP");
        Debug.Log("USE");

        //GetComponent<Animator>().ResetTrigger("PICK_UP");

        // if (TryGetComponent<IUsable>(out IUsable item)) { item.Use() } // Bare pseudo kode til at kommunikere ide
    }
}
