using UnityEngine;

namespace Assets.Scripts
{

    public class PauseManager : MonoBehaviour
    {
        public bool IsPaused = false;

        public static PauseManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void TogglePause()
        {
            IsPaused = !IsPaused;

            if (IsPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}