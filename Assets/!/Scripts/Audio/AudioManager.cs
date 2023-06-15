using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public int AudioSourcesLimit;
  public AudioEvent[] AudioEvents;
  [MinMaxRange(0.1f, 5f)]
  public RangedFloat SourceDelay;

  private AudioSource[] AudioSources;
  // Start is called before the first frame update
  void Start()
  {
    //create audiosources
    AudioSources = new AudioSource[AudioSourcesLimit];
    for (int i = 0; i < AudioSources.Length; i++)
    {
      AudioSources[i] = gameObject.AddComponent<AudioSource>(); 
    }
    StartCoroutine(PlaySoundsLoop(AudioSourcesLimit));
  }

  IEnumerator PlaySoundsLoop(int nOAudioSources)
  {
    while (true)
    {
      var randomAudioEventSelector = new RandomOptionSelector(AudioEvents.Length);
      foreach (var audioSource in AudioSources)
      {
        //select clip type. I.E. Zombie1, Zombie2
        var selectedIndexClipType = randomAudioEventSelector.GetRandomOption();
        AudioEvent selectedAudioEvent = AudioEvents[selectedIndexClipType];
        //select random clip from random clip type
        float sourceDelay = UnityEngine.Random.Range(SourceDelay.minValue, SourceDelay.maxValue);
        yield return new WaitForSeconds(sourceDelay);
        selectedAudioEvent.Play(audioSource);
      }
    }
  }
}
