using UnityEngine;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Attack Settings")]
        public int AttackDamage = 20;
        public float AttackRange = 5f; // Increased from 2f
        public float AttackCooldown = 0.5f;
        public LayerMask EnemyLayer;
        
        [Header("Block Settings")]
        public bool IsBlocking { get; private set; }
        public float BlockDamageReduction = 0.75f; // 75% damage reduction
        public GameObject BlockVFX;

        [Header("AOE Attack")]
        public float AOERadius = 5f;
        public int AOEDamage = 25;
        public float AOECooldown = 5f;
        public int AOEEnergyCost = 20;
        public GameObject AOEVisualPrefab;
        
        private float _lastAttackTime;
        private float _lastAOETime;
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            HandleCombatInput();
        }

        private void HandleCombatInput()
        {
            var mouse = UnityEngine.InputSystem.Mouse.current;
            if (mouse == null) return;

            // Left Click - Attack
            if (mouse.leftButton.wasPressedThisFrame && Time.time >= _lastAttackTime + AttackCooldown)
            {
                PerformAttack();
            }

            // Right Click - Block (hold)
            if (mouse.rightButton.isPressed)
            {
                StartBlocking();
            }
            else if (IsBlocking)
            {
                StopBlocking();
            }

            // Q - AOE Attack
            if (UnityEngine.InputSystem.Keyboard.current.qKey.wasPressedThisFrame) 
            {
                PerformAOEAttack();
            }
        }

        private void PerformAttack()
        {
            _lastAttackTime = Time.time;
            
            // Trigger animation with boolean
            if (_animator != null)
            {
                _animator.SetBool("IsAttacking", true);
                // Reset after a short delay (you can adjust this or use animation events)
                StartCoroutine(ResetAttackAnimation());
            }

            Debug.Log($"Player attacked at position {transform.position}!");

            // Detect ALL colliders in range, then filter for enemies
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRange);
            
            Debug.Log($"Found {hitColliders.Length} colliders in {AttackRange} unit range");

            int enemiesHit = 0;
            foreach (var hit in hitColliders)
            {
                Debug.Log($"Checking collider on: {hit.gameObject.name}");
                
                var enemy = hit.GetComponent<CosmicYarnCat.Enemies.EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(AttackDamage);
                    enemiesHit++;
                    Debug.Log($"Hit {enemy.name} for {AttackDamage} damage");
                }
            }

            if (enemiesHit == 0)
            {
                Debug.LogWarning($"No enemies hit! Checked {hitColliders.Length} colliders.");
            }
        }

        private System.Collections.IEnumerator ResetAttackAnimation()
        {
            yield return new WaitForSeconds(0.1f);
            if (_animator != null)
                _animator.SetBool("IsAttacking", false);
        }

        private void StartBlocking()
        {
            if (!IsBlocking)
            {
                IsBlocking = true;
                if (BlockVFX != null)
                    BlockVFX.SetActive(true);
                
                Debug.Log("Player is blocking");
            }
        }

        private void StopBlocking()
        {
            IsBlocking = false;
            if (BlockVFX != null)
                BlockVFX.SetActive(false);
        }

        public float GetDamageMultiplier()
        {
            return IsBlocking ? BlockDamageReduction : 1f;
        }

        private void PerformAOEAttack()
        {
            if (Time.time < _lastAOETime + AOECooldown) return;
            
            var stats = GetComponent<PlayerStats>();
            if (stats.CurrentEnergy < AOEEnergyCost) return;

            stats.ConsumeEnergy(AOEEnergyCost);
            _lastAOETime = Time.time;

            if (AOEVisualPrefab) Instantiate(AOEVisualPrefab, transform.position, Quaternion.identity);

            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, AOERadius);
            foreach (var hit in hitEnemies)
            {
                var enemy = hit.GetComponent<CosmicYarnCat.Enemies.EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(AOEDamage);
                    Vector3 pushDir = (enemy.transform.position - transform.position).normalized;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRange);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AOERadius);
        }
    }
}
