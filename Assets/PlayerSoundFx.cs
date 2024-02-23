using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSoundFx : MonoBehaviour
{
    [SerializeField] List<AudioClip> voiceLines;

    private List<AudioClip> voiceLinesToPlayClips;
    [SerializeField] UnityEvent onAbility;
    bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        voiceLinesToPlayClips = new List<AudioClip>(voiceLines);
        StartCoroutine(PlayFx());
    }

    // Update is called once per frame

    IEnumerator PlayFx()
    {
        yield return new WaitForSeconds(5);
        float delay = Random.Range(15, 25);
        int randomIndex = Random.Range(0, voiceLinesToPlayClips.Count);
        AudioFxManager.instance.PlayPlayerFX(voiceLinesToPlayClips[randomIndex],1f);
        yield return new WaitForSeconds(voiceLinesToPlayClips[randomIndex].length + delay);
        StartCoroutine(PlayFx());

    }
}
