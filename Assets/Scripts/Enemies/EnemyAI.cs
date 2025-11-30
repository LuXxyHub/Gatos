using UnityEngine;

namespace CosmicYarnCat.Enemies
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    public class EnemyAI : MonoBehaviour
    {
        [Header("AI Settings")]
        public float DetectionRange = 10f;
        public float AttackRange = 2f;
        public float MoveSpeed = 3f;
        public int AttackDamage = 10;
        public float AttackCooldown = 1f;

        protected Transform Player;
        protected EnemyState CurrentState;
        private float _lastAttackTime;

        protected virtual void Start()
        {
            // Find player by tag or singleton
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                Player = playerObj.transform;
            
            CurrentState = EnemyState.Idle;
        }

        protected virtual void Update()
        {
            if (Player == null) return;

            float distance = Vector3.Distance(transform.position, Player.position);

            switch (CurrentState)
            {
                case EnemyState.Idle:
                    if (distance < DetectionRange)
                        CurrentState = EnemyState.Chase;
                    break;

                case EnemyState.Chase:
                    if (distance > DetectionRange * 1.5f)
                        CurrentState = EnemyState.Idle;
                    else if (distance < AttackRange)
                        CurrentState = EnemyState.Attack;
                    else
                        MoveTowardsPlayer();
                    break;

                case EnemyState.Attack:
                    if (distance > AttackRange)
                        CurrentState = EnemyState.Chase;
                    else
                        PerformAttack();
                    break;
            }
        }

        protected virtual void MoveTowardsPlayer()
        {
            Vector3 direction = (Player.position - transform.position).normalized;
            transform.position += direction * MoveSpeed * Time.deltaTime;
            transform.LookAt(new Vector3(Player.position.x, transform.position.y, Player.position.z));
        }

        protected virtual void PerformAttack()
        {
            if (Time.time < _lastAttackTime + AttackCooldown) return;

            // Try to damage player
            if (Player != null)
            {
                var playerStats = Player.GetComponent<CosmicYarnCat.Player.PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(AttackDamage);
                    _lastAttackTime = Time.time;
                    Debug.Log($"{gameObject.name} attacked player for {AttackDamage} damage");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Alternative: Damage player via trigger collision
            if (other.CompareTag("Player"))
            {
                var playerStats = other.GetComponent<CosmicYarnCat.Player.PlayerStats>();
                if (playerStats != null && Time.time >= _lastAttackTime + AttackCooldown)
                {
                    playerStats.TakeDamage(AttackDamage);
                    _lastAttackTime = Time.time;
                    Debug.Log($"{gameObject.name} hit player for {AttackDamage} damage (trigger)");
                }
            }
        }
    }
}
