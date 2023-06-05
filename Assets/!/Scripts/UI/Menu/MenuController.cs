using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
  public GameObject endAnimation;
  public void PlayGame()
  {
    // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    endAnimation.SetActive(true);
  }

  public void QuitGame()
  {
    Application.Quit();
  }
}
