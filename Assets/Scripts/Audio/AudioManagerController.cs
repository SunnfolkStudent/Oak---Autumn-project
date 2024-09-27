using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManagerController : MonoBehaviour
{
    public static AudioManagerController instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one AudioManagerController found");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    public EventInstance CreatInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
