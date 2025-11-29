using UnityEngine;
using System.Collections;

namespace CosmicYarnCat.Enemies
{
    public class NeedleSentinel : EnemyAI
    {
        [Header("Sentinel Settings")]
        public float DashSpeed = 10f;
        public float DashDuration = 0.5f;
        public int DashDamage = 15;
        public float DashCooldown = 3f;

        private float _lastDashTime;
        private bool _isDashing;

        protected override void PerformAttack()
        {
            if (Time.time >= _lastDashTime + DashCooldown && !_isDashing)
            {
                StartCoroutine(DashAttack());
            }
        }

        private IEnumerator DashAttack()
        {
            _isDashing = true;
            _lastDashTime = Time.time;

            Vector3 dashDirection = (Player.position - transform.position).normalized;
            float startTime = Time.time;

            while (Time.time < startTime + DashDuration)
            {
                transform.position += dashDirection * DashSpeed * Time.deltaTime;
                
                // Simple collision check
                if (Vector3.Distance(transform.position, Player.position) < 1f)
                {
                    // Deal damage
                    var playerStats = Player.GetComponent<CosmicYarnCat.Player.PlayerStats>();
                    if (playerStats != null)
                    {
                        playerStats.TakeDamage(DashDamage);
                    }
                    // Stop dashing on hit?
                    // break; 
                }

                yield return null;
            }

            _isDashing = false;
        }
    }
}
