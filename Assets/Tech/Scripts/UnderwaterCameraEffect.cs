using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterCameraEffect : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] float surfacelevel = 14.82f;

    [Header ("Info")]
    [Tooltip ("Is set automatically")]
    [SerializeField] Volume postProcessing;
    
    [Header ("Volume Profiles")]
    [SerializeField] VolumeProfile surfaceProfile;
    [SerializeField] VolumeProfile underwaterProfile;

    void Awake()
    {
        postProcessing = GameObject.FindWithTag("PostProcessing").GetComponent<Volume>();
    }

    void Update()
    {
        if (transform.position.y < surfacelevel)
        {
            EnableEffects(true);
        }
        else
        {
            EnableEffects(false);
        }
    }

    void EnableEffects(bool active)
    {
        if (active)
        {
            postProcessing.profile = underwaterProfile;
        }
        else
        {
            postProcessing.profile = surfaceProfile;
        }
    }
}