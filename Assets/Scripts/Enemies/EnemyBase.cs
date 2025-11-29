using UnityEngine;
using System;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Enemies
{
    [RequireComponent(typeof(Health))]
    public class EnemyBase : MonoBehaviour
    {
        [Header("Stats")]
        public int ExperienceValue = 10;
        public GameObject HitVFX;
        public GameObject DeathVFX;
        public AudioClip HitSound;
        public AudioClip DeathSound;

        protected Health _health;

        protected virtual void Awake()
        {
            _health = GetComponent<Health>();
        }

        protected virtual void Start()
        {
            _health.OnDamage += OnDamageTaken;
            _health.OnDeath += Die;
        }

        public virtual void TakeDamage(int amount)
        {
            if (_health == null)
            {
                Debug.LogError($"{gameObject.name} is missing Health component! Cannot take damage.");
                return;
            }

            Debug.Log($"{gameObject.name} taking {amount} damage. Current HP: {_health.CurrentHealth}/{_health.MaxHealth}");
            _health.TakeDamage(amount);
        }

        protected virtual void OnDamageTaken(int amount)
        {
            // Visual Feedback
            if (HitVFX != null) VFXManager.Instance.SpawnVFX(HitVFX, transform.position);
            if (HitSound != null) AudioManager.Instance.PlaySFX(HitSound);
            
            // Flash White (requires material setup, skipping for now or using simple color change)
        }

        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} died!");

            // Visual Feedback
            if (DeathVFX != null) VFXManager.Instance.SpawnVFX(DeathVFX, transform.position);
            if (DeathSound != null) AudioManager.Instance.PlaySFX(DeathSound);

            // Drop loot
            CosmicYarnCat.Core.ResourceManager.Instance.AddThread(ExperienceValue);

            Destroy(gameObject);
        }
    }
}
