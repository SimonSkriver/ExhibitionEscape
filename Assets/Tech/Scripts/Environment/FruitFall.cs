using UnityEngine;

public class FruitFall : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasLanded;
    [SerializeField] private bool isCoconut;
    [SerializeField] private GameObject fullCoconut;
    [SerializeField] private GameObject splitCoconut;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        if (isCoconut)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                if (!hasLanded) PlayerStats.Instance.RemoveHealth(25);
                hasLanded = true;
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            SplitFruit();
        }
    }

    private void SplitFruit()
    {
        if(isCoconut)
            {
                if(fullCoconut != null && splitCoconut != null)
                {
                    fullCoconut.SetActive(false);
                    splitCoconut.SetActive(true);
                    splitCoconut.transform.SetParent(null);
                    Destroy(gameObject);
                }
            }
    }
}