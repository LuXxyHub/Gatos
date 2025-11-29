using UnityEngine;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float MoveSpeed = 8f;
        public float RotationSpeed = 10f;
        public float Gravity = -20f;

        [Header("Jump Settings")]
        public float JumpHeight = 2f;
        public float BoostedJumpMultiplier = 1.5f;
        public int MaxJumps = 2; // Double jump allowed

        private CharacterController _controller;
        private Vector3 _velocity;
        private int _jumpsRemaining;
        private bool _isGrounded;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _jumpsRemaining = MaxJumps;
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState != GameState.Exploration && 
                GameManager.Instance.CurrentState != GameState.Combat)
                return;

            HandleMovement();
            HandleJump();
            ApplyGravity();
        }

        private void HandleMovement()
        {
            Vector2 input = InputManager.Instance.MovementInput;
            Vector3 move = new Vector3(input.x, 0, input.y);

            // Convert to Isometric direction (assuming camera is rotated 45 degrees Y)
            // A simple approximation is rotating the input vector by 45 degrees.
            move = Quaternion.Euler(0, 45, 0) * move;

            if (move.magnitude > 0.1f)
            {
                // Move
                _controller.Move(move * MoveSpeed * Time.deltaTime);

                // Rotate
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            }
        }

        private void HandleJump()
        {
            _isGrounded = _controller.isGrounded;

            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // Stick to ground
                _jumpsRemaining = MaxJumps;
            }

            if (InputManager.Instance.JumpTriggered && _jumpsRemaining > 0)
            {
                float height = JumpHeight;
                
                // Boosted jump logic (e.g. if holding a button or standing on elastic yarn)
                // For now, let's say the second jump is always boosted
                if (_jumpsRemaining < MaxJumps)
                {
                    height *= BoostedJumpMultiplier;
                    // Play boost effect here
                }

                _velocity.y = Mathf.Sqrt(height * -2f * Gravity);
                _jumpsRemaining--;
            }
        }

        private void ApplyGravity()
        {
            _velocity.y += Gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}
