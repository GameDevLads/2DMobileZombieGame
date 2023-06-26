using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudioInstance : MonoBehaviour
{
  public AudioManager AudioManager;
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
    AudioManager.AddAudioInstance(this);
  }

  void OnDisable()
  {
    AudioSource.Stop();
    AudioManager.RemoveAudioInstance(this);
  }

  public void Play()
  {
    AudioEvent.Play(AudioSource);
  }
}
