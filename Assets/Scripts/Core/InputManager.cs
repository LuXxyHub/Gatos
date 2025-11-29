using UnityEngine;
using UnityEngine.InputSystem;

namespace CosmicYarnCat.Core
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public Vector2 MovementInput { get; private set; }
        public bool JumpTriggered { get; private set; }
        public bool AttackTriggered { get; private set; }
        public bool DashTriggered { get; private set; }

        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _dashAction;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput == null)
            {
                Debug.LogError("PlayerInput component missing on InputManager!");
                return;
            }

            SetupInputActions();
        }

        private void SetupInputActions()
        {
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _attackAction = _playerInput.actions["Attack"];
            _dashAction = _playerInput.actions["Dash"];
        }

        private void Update()
        {
            if (_moveAction != null)
                MovementInput = _moveAction.ReadValue<Vector2>();

            JumpTriggered = _jumpAction != null && _jumpAction.WasPressedThisFrame();
            AttackTriggered = _attackAction != null && _attackAction.WasPressedThisFrame();
            DashTriggered = _dashAction != null && _dashAction.WasPressedThisFrame();
        }
    }
}
