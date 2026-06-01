using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.Rendering;
public class BoarFootsteps : MonoBehaviour
{
    List<AudioClip> footStepSounds = new List<AudioClip>();
    private string currentLayer;
    public FootStepSO[] footStepSOs;
    AudioSource audioSource;
    [SerializeField] float maxPitch = 1.2f;
    [SerializeField] float minPitch = 0.8f;
    float currentVolume = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
        audioSource = transform.Find("FootStepAS").GetComponent<AudioSource>();
    }
    public void CheckLayers()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 5f))
        {
            if (hit.transform.GetComponent<Terrain>()!= null)
            {
                Terrain t = hit.transform.GetComponent<Terrain>();
                if(currentLayer != GetLayerName(transform.position, t))
                {
                    currentLayer = GetLayerName(transform.position, t);
                    foreach(FootStepSO footstepso in footStepSOs)
                    {
                        if(currentLayer == footstepso.terrainLayer.name)
                        {
                            SwapFootSteps(footstepso);
                        }
                    }
                }
            }
        }
    }
    public void SwapFootSteps(FootStepSO footstepso)
    {
        footStepSounds.Clear();
        for (int i = 0; i < footstepso.footstepClips.Count; i++)
        {
            footStepSounds.Add(footstepso.footstepClips[i]);
        }
        currentVolume = footstepso.volume;
    }
    private float[] GetTextureMix(Vector3 playerPos, Terrain t)
    {
        Vector3 tPos = t.transform.position;
        TerrainData tData = t.terrainData;
        int mapX = Mathf.RoundToInt((playerPos.x - tPos.x) / tData.size.x * tData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((playerPos.z - tPos.z) / tData.size.z * tData.alphamapHeight);
        float[,,] splatMapData = tData.GetAlphamaps(mapX,mapZ, 1,1);
        float[] cellmix = new float[splatMapData.GetUpperBound(2) + 1];
        for (int i = 0; i < cellmix.Length; i++)
        {
            cellmix[i] = splatMapData[0,0,i];
        }
        return cellmix;
    }

    public string GetLayerName(Vector3 playerPos, Terrain t)
    {
        float[] cellMix = GetTextureMix(playerPos, t);
        float strongestTexture = 0;
        int maxIndex = 0;
        for (int i = 0; i < cellMix.Length; i++)
        {
            if(cellMix[i] > strongestTexture)
            {
                maxIndex = i;
                strongestTexture = cellMix[i];
            }
        }
        return t.terrainData.terrainLayers[maxIndex].name;
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
  
    public void PlayFootStep()
    {
       CheckLayers();
       if(footStepSounds.Count == 0)
        {
            return;
        }
        AudioClip randomFootstep = footStepSounds[Random.Range(0,footStepSounds.Count)];
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = currentVolume;
            audioSource.PlayOneShot(randomFootstep);
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
