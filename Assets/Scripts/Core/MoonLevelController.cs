using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using CosmicYarnCat.UI;

namespace CosmicYarnCat.Core
{
    public class MoonLevelController : MonoBehaviour
    {
        public static MoonLevelController Instance { get; private set; }

        [Header("Level Settings")]
        public MoonData CurrentMoon;
        public Transform PlayerSpawnPoint;
        public Transform BossSpawnPoint;

        [Header("UI References")]
        public GameObject IntroPanel;
        public TextMeshProUGUI IntroTitle;
        public TextMeshProUGUI IntroDescription;
        public GameObject VictoryPanel;
        public GameObject DefeatPanel;
        public Image FadeImage;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartCoroutine(LevelStartSequence());
        }

        private IEnumerator LevelStartSequence()
        {
            // Setup UI
            if (CurrentMoon != null)
            {
                IntroTitle.text = CurrentMoon.MoonName;
                IntroDescription.text = CurrentMoon.Description;
            }
            
            if (IntroPanel != null)
            {
                IntroPanel.SetActive(true);
            }

            if (FadeImage != null)
            {
                FadeImage.color = Color.black;
                
                // Fade In
                float t = 0;
                while (t < 1)
                {
                    t += Time.deltaTime;
                    FadeImage.color = new Color(0, 0, 0, 1 - t);
                    yield return null;
                }
            }
            else
            {
                yield return null; // Wait a frame if no fade
            }

            yield return new WaitForSeconds(2f); // Show title for 2 seconds
            
            if (IntroPanel != null)
            {
                IntroPanel.SetActive(false);
            }
            
            GameManager.Instance.SetState(GameState.Exploration);
        }

        public void TriggerVictory()
        {
            StartCoroutine(VictorySequence());
        }

        private IEnumerator VictorySequence()
        {
            GameManager.Instance.SetState(GameState.Dialogue); // Pause inputs
            
            // Slow motion
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(2f);
            Time.timeScale = 1f;

            // Mark moon as complete
            if (CurrentMoon != null)
            {
                LevelManager.Instance.CompleteMoon(CurrentMoon);
            }

            VictoryPanel.SetActive(true);
        }

        public void TriggerDefeat()
        {
            StartCoroutine(DefeatSequence());
        }

        private IEnumerator DefeatSequence()
        {
            GameManager.Instance.SetState(GameState.Dialogue);
            
            // Fade to black
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime;
                FadeImage.color = new Color(0, 0, 0, t);
                yield return null;
            }

            DefeatPanel.SetActive(true);
        }

        // Called by UI Buttons
        public void RestartLevel()
        {
            Time.timeScale = 1f; // Unpause
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public void ReturnToHub()
        {
            Time.timeScale = 1f; // Unpause
            LevelManager.Instance.ReturnToHub();
        }
    }
}
