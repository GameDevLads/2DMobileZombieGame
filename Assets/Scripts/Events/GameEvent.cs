using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Game Event")]
public class GameEvent : ScriptableObject
{
  /// <summary>
  /// The list of listeners that this event will notify if it is raised.
  /// </summary>
  private readonly List<GameEventListener> eventListeners =
      new List<GameEventListener>();

  public void Raise()
  {
    for (int i = eventListeners.Count - 1; i >= 0; i--)
      eventListeners[i].OnEventRaised();
  }

  [HideInInspector]
  public void RegisterListener(GameEventListener listener)
  {
    if (!eventListeners.Contains(listener))
      eventListeners.Add(listener);
  }

  [HideInInspector]
  public void UnregisterListener(GameEventListener listener)
  {
    if (eventListeners.Contains(listener))
      eventListeners.Remove(listener);
  }
}
