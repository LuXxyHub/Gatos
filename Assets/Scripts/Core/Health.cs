using UnityEngine;
using System;
using System.Collections;

namespace CosmicYarnCat.Core
{
    public class Health : MonoBehaviour
    {
        [Header("Health Settings")]
        public int MaxHealth = 100;
        public int CurrentHealth { get; private set; }
        public bool IsInvincible { get; private set; }
        
        [Header("Invincibility")]
        public float InvincibilityDuration = 1f;
        public bool UseInvincibility = true;

        // Events
        public event Action<int, int> OnHealthChanged; // Current, Max
        public event Action<int> OnDamage; // Amount
        public event Action<int> OnHeal; // Amount
        public event Action OnDeath;
        public event Action OnInvincibilityStart;
        public event Action OnInvincibilityEnd;

        private void Start()
        {
            CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void TakeDamage(int amount)
        {
            if (IsInvincible || CurrentHealth <= 0) return;

            CurrentHealth -= amount;
            if (CurrentHealth < 0) CurrentHealth = 0;

            Debug.Log($"Health took {amount} damage. Current HP: {CurrentHealth}/{MaxHealth}");

            OnDamage?.Invoke(amount);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

            if (CurrentHealth == 0)
            {
                Die();
            }
            else if (UseInvincibility)
            {
                StartCoroutine(InvincibilityRoutine());
            }
        }

        public void Heal(int amount)
        {
            if (CurrentHealth <= 0) return; // Can't heal dead things usually

            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;

            OnHeal?.Invoke(amount);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void Kill()
        {
            if (CurrentHealth > 0)
            {
                CurrentHealth = 0;
                OnHealthChanged?.Invoke(0, MaxHealth);
                Die();
            }
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
            IsInvincible = false;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        private void Die()
        {
            Debug.Log($"Health.Die() called on {gameObject.name}");
            OnDeath?.Invoke();
        }

        private IEnumerator InvincibilityRoutine()
        {
            IsInvincible = true;
            OnInvincibilityStart?.Invoke();

            yield return new WaitForSeconds(InvincibilityDuration);

            IsInvincible = false;
            OnInvincibilityEnd?.Invoke();
        }
    }
}
