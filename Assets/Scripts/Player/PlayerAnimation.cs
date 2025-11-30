using UnityEngine;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator _animator;
        private PlayerController _controller;
        private Health _health;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<PlayerController>();
            _health = GetComponent<Health>();
        }

        private void Start()
        {
            if (_health != null)
            {
                _health.OnDamage += OnTakeDamage;
                _health.OnDeath += OnDeath;
            }
            
            // Subscribe to Input events if possible, or check in Update
        }

        private void Update()
        {
            // Skip if no Animator Controller is assigned
            if (_animator == null || _animator.runtimeAnimatorController == null)
                return;

            // Update Movement parameters
            // Assuming PlayerController exposes velocity or we check InputManager
            Vector2 input = InputManager.Instance.MovementInput;
            bool isMoving = input.magnitude > 0.1f;
            _animator.SetBool("IsRunning", isMoving);
            
            // Attack - Check if attacking (this is now handled by PlayerCombat)
            // We don't need to set it here since PlayerCombat handles the animation
            // Just keep the IsRunning parameter
        }

        private void OnTakeDamage(int amount)
        {
            _animator.SetTrigger("Hit");
        }

        private void OnDeath()
        {
            _animator.SetBool("IsDead", true);
            _animator.SetTrigger("Die");
        }
    }
}
