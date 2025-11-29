using UnityEngine;
using UnityEngine.UI;
using TMPro; // Assuming TextMeshPro is used
using CosmicYarnCat.Player;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD Elements")]
        public Image HealthBar;
        public Image EnergyBar;
        public TextMeshProUGUI ThreadCountText;
        public TextMeshProUGUI FragmentCountText;

        [Header("Feedback")]
        public Image DamageFlashImage;
        public float FlashSpeed = 2f;

        private void Start()
        {
            // Subscribe to events
            var playerStats = FindFirstObjectByType<PlayerStats>();
            if (playerStats != null)
            {
                // PlayerStats now uses a Health component internally, but doesn't expose the event directly
                // We should get the Health component from the player
                var playerHealth = playerStats.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.OnHealthChanged += UpdateHealth;
                    playerHealth.OnDamage += TriggerDamageFlash;
                }
            }

            ResourceManager.Instance.OnThreadChanged += UpdateThreadCount;
            ResourceManager.Instance.OnFragmentsChanged += UpdateFragmentCount;
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
