using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public List<SFXclip> sFXclips;
    private AudioSource audioSource;
    private AudioSource audioSource2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource2 = transform.Find("SFXSource2").GetComponent<AudioSource>();
    }

    public static void PlayEffect(string soundName)
    {
        if(soundName == null || Instance == null)
        {
            return;
        }
        foreach(var sfxclip in Instance.sFXclips)
        {
            if(soundName == sfxclip.clipName)
            {
                Instance.audioSource.volume = sfxclip.volume;
                var randomClip = Random.Range(0,sfxclip.audioClips.Length);
                if (!Instance.audioSource.isPlaying)
                {
                    Instance.audioSource.PlayOneShot(sfxclip.audioClips[randomClip]);
                    Debug.Log("play AudioSource 1");
                }
                else
                {
                    Instance.audioSource2.PlayOneShot(sfxclip.audioClips[randomClip]);
                    Debug.Log("play AudioSource 2");

                }
                return;
            }
        }
    }
}
