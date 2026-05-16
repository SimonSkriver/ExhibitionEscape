using System.Collections;
using UnityEngine;

public class Seagull : MonoBehaviour
{
    [SerializeField] Transform orbit;

    [SerializeField] int range = 100;
    [SerializeField] int flySpeed = 10;
    
    Vector3 flyDest;

    void Awake() {
        StartCoroutine(ChangeFlyDestination());
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, flyDest, flySpeed * Time.deltaTime);
        transform.LookAt(flyDest);
    }

    // fly code
    void Fly() {
        float rndX = orbit.position.x + Random.Range(0, range);
        float rndY = orbit.position.y + Random.Range(0, range * 0.2f);
        float rndZ = orbit.position.z + Random.Range(0, range);
        
        flyDest = new Vector3(rndX, rndY, rndZ);
    }

    IEnumerator ChangeFlyDestination() {
        while (true) {
            Fly();
            yield return new WaitUntil(() => Vector3.Distance(transform.position, flyDest) < 1);
        }
    }

    // fly location = Random.Range(midIsland, midIsland + 100)


    // RND = Random Vector3 (if destination is over 100 from midIsland, then multiply by 0.5f;
    // newFlyDestination = transform.position + RND

    // change fly location if destination is reached or Random Timer is hit
}
