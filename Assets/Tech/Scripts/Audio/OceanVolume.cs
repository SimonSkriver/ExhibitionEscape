using UnityEngine;

public class OceanVolume : MonoBehaviour
{
    AudioSource audioSource;
    Transform playertransform;
    float islandRadius = 125f;
    Transform audiosourceTransform;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject playerObj = GameObject.FindWithTag("Player");
        audiosourceTransform = transform;
        if(playerObj != null) playertransform = playerObj.transform;
        else Debug.Log("player not found");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerFlat = new Vector3(playertransform.position.x, 0, playertransform.position.z);
        Vector3 centerFlat = new Vector3(audiosourceTransform.position.x, 0, audiosourceTransform.position.z);

        float dist = Vector3.Distance(playerFlat, centerFlat);
        
        float t = Mathf.InverseLerp(0, islandRadius, dist);

        
        float volume = Mathf.Lerp(0.1f, 0.45f, t);

        audioSource.volume = volume;
    }
}
