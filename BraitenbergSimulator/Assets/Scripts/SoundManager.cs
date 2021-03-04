using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip selectObjectSound;
    [SerializeField] private AudioClip deselectObjectSound;
    [SerializeField] private AudioClip placeObjectSound;
    [SerializeField] private AudioClip deleteObjectSound;

    // Singleton pattern for SoundManager
    #region singleton
    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public void PlaySelectObjectSound()
    {
        PlaySound(selectObjectSound, 0.5f, 1f, 1.1f);
    }
    public void PlayDeselectObjectSound()
    {
        PlaySound(deselectObjectSound, 0.2f, 1f, 1.1f);
    }

    public void PlayPlaceObjectSound()
    {
        PlaySound(placeObjectSound, 0.5f, 1f, 1.5f);
    }

    public void PlayDeleteObjectSound()
    {
        PlaySound(deleteObjectSound, 0.2f, 1f, 1.1f);
    }

    private void PlaySound(AudioClip sound, float volume, float minPitch, float maxPitch)
    {
        // Create gameobject and add audiosource component
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        // Set volume
        audioSource.volume = volume;

        // Set pitch to random to increase variety
        audioSource.pitch = Random.Range(minPitch, maxPitch);

        // Play audio
        audioSource.PlayOneShot(sound);

        // Remove sound game object using coroutine
        StartCoroutine(RemoveSoundObject(soundGameObject));
    }

    private IEnumerator RemoveSoundObject(GameObject soundObject)
    {
        yield return new WaitForSeconds(3);
        Destroy(soundObject);
    }
}
