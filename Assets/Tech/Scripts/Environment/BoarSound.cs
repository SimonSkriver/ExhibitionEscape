using UnityEngine;
using System.Collections.Generic;

public class BoarSound : MonoBehaviour
{
    public List<BoarClip> boarClips;
    AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    [System.Serializable]
    public struct BoarClip
    {
        public AudioClip audioClip;
        public string clipName;
    }

    public void PlayBoarClip(string clipname)
    {
        foreach(var boarclip in boarClips)
        {
            if(clipname == boarclip.clipName)
            {
                var audioclip = boarclip.audioClip;
                audioSource.PlayOneShot(audioclip);
                return;
            }
        }
    }
}
