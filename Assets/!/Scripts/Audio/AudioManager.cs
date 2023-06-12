using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public IntVariableSO NOZombies;
  public AudioClip[] Zombie1Audios;
  public AudioClip[] Zombie2Audios;
  public AudioClip[] Zombie3Audios;
  private AudioSource[] audioSources;
  // Start is called before the first frame update
  void Start()
  {
    audioSources = GetComponents<AudioSource>();
    var nOAudioSources = NOZombies.Value > 2 ? 3 : NOZombies.Value;
    StartCoroutine(PlaySoundsLoop(nOAudioSources));
  }

  IEnumerator PlaySoundsLoop(int nOAudioSources)
  {
    while (NOZombies)
    {
      for (int i = 0; i < nOAudioSources; i++)
      {
        AudioClip randomClip = Zombie1Audios[UnityEngine.Random.Range(0, Zombie1Audios.Length)];
        float sourceDelay = UnityEngine.Random.Range(0.5f, 1.5f);
        audioSources[i].clip = randomClip;
        audioSources[i].Play();
        yield return new WaitForSeconds(sourceDelay);
      }
      float interval = UnityEngine.Random.Range(1, 2);
      yield return new WaitForSeconds(interval);
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
