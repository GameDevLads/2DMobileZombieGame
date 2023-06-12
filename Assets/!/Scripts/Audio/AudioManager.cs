using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public IntVariableSO NOAudioSources;
  public AudioClipType[] AudioClipTypes;

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
    var hasSources = true;
    while (hasSources)
    {
      var randomClipType = new RandomOptionSelector(AudioClipTypes.Length);
      foreach (var audioSource in AudioSources)
      {
        //select clip type. I.E. Zombie1, Zombie2
        var selectedIndexClipType = randomClipType.GetRandomOption();
        AudioClipType selectedClipType = AudioClipTypes[selectedIndexClipType];
        //select random clip from random clip type
        var randomClipPerType = new RandomOptionSelector(selectedClipType.AudioClips.Length);
        var randomClipPerTypeNumber = randomClipPerType.GetRandomOption();
        audioSource.clip = selectedClipType.AudioClips[randomClipPerTypeNumber];
        float sourceDelay = UnityEngine.Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(sourceDelay);
        audioSource.Play();
      }
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}

[Serializable]
public class AudioClipType
{
  public string Name;
  public IntVariableSO NOAudioSources;
  public IntVariableSO NOCurrentSources;
  public AudioClip[] AudioClips;
}
