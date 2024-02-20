using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFxManager : MonoBehaviour
{
    public static AudioFxManager instance;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] AudioSource PlayerSource;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    private void Update()
    {
        PlayerSource.transform.position = Hammer.instance.transform.position;
    }

    public void PlayPlayerFX(AudioClip clip,float volume, bool isAbility = false)
    {
        if (!isAbility && PlayerSource.isPlaying) return;
        PlayerSource.clip = clip;
        PlayerSource.volume = volume;
        PlayerSource.Play();
    }
    
    public void PlaySoundEffect(AudioClip clip,Transform spawnTransform,float volume,bool isVoiceLine = false)
    {

        AudioSource audioSource = Instantiate(audioSourcePrefab,spawnTransform.position,Quaternion.identity);

        audioSource.clip = clip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        Destroy(audioSource,clipLength);
    }
}
