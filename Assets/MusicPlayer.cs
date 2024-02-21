using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] List<AudioClip> clipList;
    int currentSongIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource= GetComponent<AudioSource>();
        StartCoroutine(PlayMusic());
    }
    IEnumerator PlayMusic()
    {
        audioSource.clip= clipList[currentSongIndex];
        audioSource.volume = 0.2f;
        audioSource.Play();
        yield return new WaitForSeconds(clipList[currentSongIndex].length + 1f);
        currentSongIndex++;
        if(currentSongIndex >= clipList.Count)
        {
            currentSongIndex= 0;
        }
        StartCoroutine(PlayMusic());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
