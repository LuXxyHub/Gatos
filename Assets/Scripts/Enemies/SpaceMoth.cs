using UnityEngine;

namespace CosmicYarnCat.Enemies
{
    public class SpaceMoth : EnemyAI
    {
        [Header("Moth Settings")]
        public float CorrosionDuration = 5f;
        public int CorrosionDamagePerTick = 2;
        public float TickRate = 1f;

        protected override void PerformAttack()
        {
            // Moths might fly over and drop dust, or touch to corrode
            // For simplicity, let's say it's a touch attack that applies a debuff
            
            // Assuming we are in range (checked by EnemyAI)
            ApplyCorrosion();
        }

        private void ApplyCorrosion()
        {
            Debug.Log("Space Moth applied Corrosion!");
            // In a real system, we'd add a StatusEffect component to the player
            // Player.GetComponent<StatusEffectManager>().Apply(EffectType.Corrosion, ...);
        }
    }
}
