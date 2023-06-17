using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public List<BackgroundAudioInstance> BGAudioInstances;
  [MinMaxRange(0.1f, 5f)]
  public RangedFloat SourceDelay;
  private Coroutine SoundsRoutine;

  void Awake()
  {
    BGAudioInstances = new List<BackgroundAudioInstance>();
  }

  // Start is called before the first frame update
  void Start()
  {
    SoundsRoutine = StartCoroutine(PlaySoundsLoop());
  }

  public void StopSounds()
  {
    StopCoroutine(SoundsRoutine);
  }

  IEnumerator PlaySoundsLoop()
  {
    while (true)
    {
      Debug.Log(BGAudioInstances.Count);
      var interval = UnityEngine.Random.Range(SourceDelay.minValue, SourceDelay.maxValue);
      yield return new WaitForSeconds(interval);
      if (BGAudioInstances.Count == 0) continue;
      var random = UnityEngine.Random.Range(0, BGAudioInstances.Count);
      BGAudioInstances[random].Play();
    }
  }
}
