using UnityEngine;

public class JumpFlower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerController controller = other.gameObject.GetComponent<PlayerController>();
            controller.canMove = false;

            

            float time = 100f;
            float jumpForce = 0.03f;
            float previousHeight = 0f;
            
            while (time > 0f) {
                time -= Time.deltaTime;
                float currentHeight = Mathf.Sin(time * Mathf.PI) * jumpForce;
                float verticalMove = currentHeight - previousHeight;
                previousHeight = currentHeight;

                other.gameObject.GetComponent<CharacterController>().Move(jumpForce * Vector3.up);
            }

            controller.canMove = true;
        }
    }
}
