using UnityEngine;

namespace CosmicYarnCat.Enemies
{
    public class YarnUFO : EnemyAI
    {
        [Header("UFO Settings")]
        public GameObject LaserPrefab;
        public float FireRate = 2f;
        private float _lastFireTime;

        protected override void PerformAttack()
        {
            if (Time.time >= _lastFireTime + FireRate)
            {
                FireLaser();
                _lastFireTime = Time.time;
            }
        }

        private void FireLaser()
        {
            if (LaserPrefab != null)
            {
                Instantiate(LaserPrefab, transform.position, transform.rotation);
                // Laser logic would handle movement and damage
            }
            else
            {
                Debug.LogWarning("YarnUFO: LaserPrefab is missing!");
            }
        }
    }
}
