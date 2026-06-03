using System.Collections;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    [SerializeField] Transform orbit;

    [SerializeField] int range = 70;
    [SerializeField] int flySpeed = 10;
    Vector3 flyDest;


    void Awake() => StartCoroutine(ChangeFlyDestination());

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, flyDest, flySpeed * Time.deltaTime);
        transform.LookAt(flyDest);
    }

    IEnumerator ChangeFlyDestination() {
        StartCoroutine(Lifetime());
        while (true) {
            // Change destination point
            float rndX = orbit.position.x + Random.Range(0, range);
            float rndY = orbit.position.y + Random.Range(0, range * 0.2f);
            float rndZ = orbit.position.z + Random.Range(0, range);
            flyDest = new Vector3(rndX, rndY, rndZ);

            // beak animation
            Animator A = GetComponent<Animator>();
            A.SetBool("openBeak", true);
            yield return new WaitForSeconds(1);
            A.SetBool("openBeak", false);
            
            yield return new WaitUntil(() => Vector3.Distance(transform.position, flyDest) < 1);
        }
    }

    IEnumerator Lifetime() {
        int time = Random.Range(10, range);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.aquamarine;
        Gizmos.DrawLine(transform.position, flyDest);
        Gizmos.color = Color.mediumAquamarine;
        Gizmos.DrawSphere(flyDest, 0.3f);
        Gizmos.DrawSphere(orbit.position, range);
    }
}
