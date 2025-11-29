using UnityEngine;
using UnityEngine.InputSystem;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PausePanel;

        private void Update()
        {
            // Check for Escape or Start button
            // Ideally this should be an Input Action "Pause"
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            if (GameManager.Instance.CurrentState == GameState.Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            GameManager.Instance.PauseGame();
            PausePanel.SetActive(true);
        }

        public void Resume()
        {
            GameManager.Instance.PauseGame();
            PausePanel.SetActive(false);
        }

        public void QuitToMain()
        {
            Time.timeScale = 1f; // Ensure time is running
            LevelManager.Instance.ReturnToHub();
        }
    }
}
