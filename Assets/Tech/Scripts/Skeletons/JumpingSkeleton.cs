using UnityEngine;

public class JumpingSkeleton : MonoBehaviour
{
    AudioSource audioSource;
    float minPitch = 0.8f;
    float maxPitch = 1.2f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
   public void PlayJumpSFX()
    {
        float randomPitch = Random.Range(minPitch,maxPitch);
        audioSource.pitch = randomPitch;
        audioSource.Play();
    }
}
