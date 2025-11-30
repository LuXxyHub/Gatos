using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CosmicYarnCat.Player;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("HUD Elements")]
        public Image HealthBar;
        public Image EnergyBar;
        public TextMeshProUGUI ThreadCountText;
        public TextMeshProUGUI FragmentCountText;

        [Header("Feedback")]
        public Image DamageFlashImage;
        public float FlashSpeed = 2f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Subscribe to events
            SubscribeToPlayer();

            ResourceManager.Instance.OnThreadChanged += UpdateThreadCount;
            ResourceManager.Instance.OnFragmentsChanged += UpdateFragmentCount;
            
            // Re-subscribe when scene changes (since Player is destroyed/recreated)
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            SubscribeToPlayer();
        }

        private void SubscribeToPlayer()
        {
            var playerStats = FindFirstObjectByType<PlayerStats>();
            if (playerStats != null)
            {
                var playerHealth = playerStats.GetComponent<Health>();
                if (playerHealth != null)
                {
                    // Unsubscribe first to avoid duplicates
                    playerHealth.OnHealthChanged -= UpdateHealth;
                    playerHealth.OnDamage -= TriggerDamageFlash;
                    
                    playerHealth.OnHealthChanged += UpdateHealth;
                    playerHealth.OnDamage += TriggerDamageFlash;
                    
                    // Force update
                    UpdateHealth(playerHealth.CurrentHealth, playerHealth.MaxHealth);
                }
            }
        }

        private void UpdateHealth(int current, int max)
        {
            if (HealthBar != null)
            {
                HealthBar.fillAmount = (float)current / max;
            }
        }

        private void TriggerDamageFlash(int amount)
        {
            if (DamageFlashImage != null)
            {
                StartCoroutine(FlashRoutine());
            }
        }

        private System.Collections.IEnumerator FlashRoutine()
        {
            Color color = DamageFlashImage.color;
            color.a = 0.5f;
            DamageFlashImage.color = color;

            while (DamageFlashImage.color.a > 0)
            {
                color.a -= Time.deltaTime * FlashSpeed;
                DamageFlashImage.color = color;
                yield return null;
            }
        }

        private void UpdateThreadCount(int amount)
        {
            if (ThreadCountText != null)
            {
                ThreadCountText.text = amount.ToString();
            }
        }

        private void UpdateFragmentCount(int amount)
        {
            if (FragmentCountText != null)
            {
                FragmentCountText.text = $"{amount}/9";
            }
        }
    }
}
