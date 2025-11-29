using UnityEngine;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Enemies
{
    public class BossBase : EnemyBase
    {
        [Header("Boss Settings")]
        public string BossName;
        public int Phases = 3;
        
        public override void TakeDamage(int amount)
        {
            base.TakeDamage(amount);
            CheckPhaseTransition();
        }

        private void CheckPhaseTransition()
        {
            float healthPercent = (float)_health.CurrentHealth / _health.MaxHealth;
            // Example logic
            if (healthPercent < 0.5f)
            {
                EnterPhase(2);
            }
        }


        protected virtual void EnterPhase(int phaseIndex)
        {
            Debug.Log($"{BossName} entering Phase {phaseIndex}!");
            // Enrage, spawn minions, etc.
        }

        protected override void Die()
        {
            // Boss Death Sequence
            Debug.Log($"{BossName} Defeated!");
            
            // Trigger Victory
            if (MoonLevelController.Instance != null)
            {
                MoonLevelController.Instance.TriggerVictory();
            }

            base.Die();
        }
    }
}
