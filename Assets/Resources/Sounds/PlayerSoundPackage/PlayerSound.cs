using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.Rendering;
public class PlayerSound : MonoBehaviour
{
    [SerializeField] List<AudioClip> footStepSounds = new List<AudioClip>();
    private CharacterController pCon;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        pCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    /*void Update()
    {
        if (tc.moveDirection.magnitude > 0.1f)
        {
            float interval = tc.isSprinting ? sprintInterval : walkInterval;
            footstepTimer -= Time.deltaTime;
            if(footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = interval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
       
    }*/
    [System.Serializable]
    private struct FootstepSound
    {
        public List<AudioClip> clip;
        public string surfaceType;
        public float volume;
    }
    void PlayFootStep()
    {
        RaycastHit hit;
        if(!Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            return;
        }

    }
    /*
    void PlayFootstepSound(){
        //Ray ray = new Ray(transform.position, Vector3.down);
        
        //InvokeRepeating(nameof(audioSource.Play), 0f, footstepInterval);
        string surface = GetCurrentSurface();
        AudioClip clip = GetFootClip(surface);
        audioSource.volume = GetVolume(surface);
        if(clip == null)
        {
            return;
        }
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip);
    }
    private string GetCurrentSurface()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            SurfaceType surface = hit.collider.GetComponent<SurfaceType>();
            if(surface != null)
            {
                return surface.surfaceType;
            }
        }
        return null;
    }
    private AudioClip GetFootClip(string surfaceName)
    {
        if(surfaceName == null)
        {
            return null;
        }
        foreach(var footclip in footstepClips)
        {
            if(footclip.surfaceType == surfaceName)
            {
                int clipindex = Random.Range(0, footclip.clip.Count);
                return footclip.clip[clipindex];
            }
        }
        return null;
    }
    private float GetVolume(string surfaceName)
    {
        if(surfaceName == null)
        {
            return 0f;
        }
        foreach(var vol in footstepClips)
        {
            if(vol.surfaceType == surfaceName)
            {
                return vol.volume;
            }
        }
        return 0f;
    }
*/
}
