using UnityEngine;
using System.Collections.Generic;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    public List<SFXclip> sFXclips;
    private AudioSource audioSource;
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
                Instance.audioSource.PlayOneShot(sfxclip.audioClips[randomClip]);
                return;
            }
        }
    }
}
