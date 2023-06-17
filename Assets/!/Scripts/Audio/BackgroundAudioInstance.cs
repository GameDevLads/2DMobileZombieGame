using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioInstance : MonoBehaviour
{
  [SerializeField]
  private AudioManager AudioManager;
  [SerializeField]
  private AudioEvent AudioEvent;
  private AudioSource AudioSource;
  // Start is called before the first frame update

  void Awake()
  {
    AudioSource = gameObject.AddComponent<AudioSource>();
  }

  void OnEnable()
  {
    AudioManager.BGAudioInstances.Add(this);
  }

  void OnDisable()
  {
    AudioSource.Stop();
    AudioManager.BGAudioInstances.Remove(this);
  }

  public void Play()
  {
    AudioEvent.Play(AudioSource);
  }
}
