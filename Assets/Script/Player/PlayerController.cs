using Script.InputSystem;
using UnityEngine;

namespace Script.Player
{
    [RequireComponent(typeof(PlayerEditor))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(LevelUtilities))]
    [RequireComponent(typeof(PlayerVisual))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(InputController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private UIController uiController;
        private InputController _inputController;
        private PlayerMovement _playerMovement;
        private LevelUtilities _levelUtilities;
        private PlayerVisual _playerVisual;
        private Character _playerCharacter;

        private void Start()
        {
            Application.targetFrameRate = 60;

            GetComponent<PlayerEditor>();
            _playerMovement = GetComponent<PlayerMovement>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _playerVisual = GetComponent<PlayerVisual>();

            _levelUtilities.RestartLevel();
        }

        private void OnEnable()
        {
            if (_inputController == null)
            {
                _inputController = GetComponent<InputController>();
            }

            if (_playerCharacter == null)
            {
                _playerCharacter = GetComponent<Character>();
            }

            _inputController.OnHorizontalSwipe += OnHorizontalSwipe;
            _playerCharacter.OnDying += OnDie;
            uiController.OnRestartButtonPressed += OnRestartButtonPressed;
            uiController.OnAttackButtonPressed += OnAttackButtonPressed;
        }

        private static void OnAttackButtonPressed()
        {
            Debug.Log("Attack");
        }

        private void OnDisable()
        {
            _inputController.OnHorizontalSwipe -= OnHorizontalSwipe;
            _playerCharacter.OnDying -= OnDie;
            uiController.OnRestartButtonPressed -= OnRestartButtonPressed;
        }

        private void OnHorizontalSwipe(int direction)
        {
            if (_playerCharacter.IsAlive())
            {
                _playerMovement.MoveOnSwipe(direction);
            }
        }

        private void OnDie()
        {
            _playerVisual.StartIdleAnimation();
            _playerMovement.StopMoving();
            uiController.ShowRestartScreen();
        }

        private void OnRestartButtonPressed()
        {
            _levelUtilities.RestartLevel();
        }

        public void Respawn()
        {
            _playerCharacter.RestoreStates();
            _playerVisual.StartMovingAnimation();
            _playerMovement.RestorePosition();
            _playerMovement.StartMoving();
        }
    }
}