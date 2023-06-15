using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public IntVariableSO NOAudioSources;
  public AudioEvent[] audioEvents;

  private AudioSource[] AudioSources;
  // Start is called before the first frame update
  void Start()
  {
    //create audiosources
    AudioSources = new AudioSource[NOAudioSources.Value];
    for (int i = 0; i < AudioSources.Length; i++)
    {
      AudioSources[i] = gameObject.AddComponent<AudioSource>(); 
    }
    StartCoroutine(PlaySoundsLoop(NOAudioSources.Value));
  }

  IEnumerator PlaySoundsLoop(int nOAudioSources)
  {
    while (true)
    {
      var randomAudioEvent = new RandomOptionSelector(audioEvents.Length);
      foreach (var audioSource in AudioSources)
      {
        //select clip type. I.E. Zombie1, Zombie2
        var selectedIndexClipType = randomAudioEvent.GetRandomOption();
        AudioEvent selectedAudioEvent = audioEvents[selectedIndexClipType];
        //select random clip from random clip type
        float sourceDelay = UnityEngine.Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(sourceDelay);
        selectedAudioEvent.Play(audioSource);
      }
    }
  }
}
