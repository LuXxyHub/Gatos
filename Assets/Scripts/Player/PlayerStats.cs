using UnityEngine;
using System;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerStats : MonoBehaviour
    {
        [Header("Stats")]
        public int MaxEnergy = 50;
        public int CurrentEnergy { get; private set; }

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            CurrentEnergy = MaxEnergy;
            
            // Subscribe to Health events if needed for UI
            // _health.OnHealthChanged += ... (UIManager handles this directly now)
            _health.OnDeath += HandleDeath;
        }

        public void TakeDamage(int amount)
        {
            // Check if player is blocking
            var combat = GetComponent<PlayerCombat>();
            if (combat != null && combat.IsBlocking)
            {
                amount = Mathf.RoundToInt(amount * combat.GetDamageMultiplier());
                Debug.Log($"Player blocked! Reduced damage to {amount}");
            }

            _health.TakeDamage(amount);
        }

        public void Heal(int amount)
        {
            _health.Heal(amount);
        }

        public void ConsumeEnergy(int amount)
        {
            CurrentEnergy -= amount;
            if (CurrentEnergy < 0) CurrentEnergy = 0;
        }

        private void HandleDeath()
        {
            Debug.Log("Player Died!");
            
            // Disable player controls
            var controller = GetComponent<PlayerController>();
            if (controller != null)
                controller.enabled = false;

            var combat = GetComponent<PlayerCombat>();
            if (combat != null)
                combat.enabled = false;

            // Pause the game
            Time.timeScale = 0f;
            Debug.Log("Game paused due to player death");

            // Trigger defeat screen if MoonLevelController exists
            if (MoonLevelController.Instance != null)
            {
                MoonLevelController.Instance.TriggerDefeat();
            }
            else
            {
                Debug.LogWarning("No MoonLevelController found - cannot show defeat screen");
            }
        }
    }
}
