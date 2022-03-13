using System;
using Script.InputSystem;
using UnityEngine;

namespace Script.Player
{
    [RequireComponent(typeof(PlayerEditor))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(LevelUtilities))]
    [RequireComponent(typeof(PlayerVisual))]
    [RequireComponent(typeof(PlayerCharacter))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(InputController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        private LevelUtilities _levelUtilities;
        private PlayerVisual _playerVisual;
        private PlayerCharacter _playerCharacter;

        private InputController _inputController;

        private void Start()
        {
            Application.targetFrameRate = 60;

            GetComponent<PlayerEditor>();
            _playerMovement = GetComponent<PlayerMovement>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _playerVisual = GetComponent<PlayerVisual>();

            _levelUtilities.RestartLevel();
        }

        private void Update()
        {
            _playerMovement.MoveOnSwipe(_levelUtilities.GetLineNumByWorldCoordinate(
                UnityEngine.Camera.main.ScreenToWorldPoint(_inputController.GetTouchPosition())));
        }

        private void OnEnable()
        {
            if (_inputController == null)
            {
                _inputController = GetComponent<InputController>();
            }

            if (_playerCharacter == null)
            {
                _playerCharacter = GetComponent<PlayerCharacter>();
            }

            _inputController.OnHorizontalSwipe += OnHorizontalSwipe;
            _playerCharacter.OnPlayerKill += OnPlayerKill;
        }

        private void OnDisable()
        {
            _inputController.OnHorizontalSwipe -= OnHorizontalSwipe;
            _playerCharacter.OnPlayerKill -= OnPlayerKill;
        }

        private void OnHorizontalSwipe(int direction)
        {
            if (!_playerCharacter.IsAlive())
            {
                RestartLevel();
            }
        }

        private void OnPlayerKill()
        {
            _playerVisual.StartIdleAnimation();
            _playerMovement.StopMoving();
        }

        private void RestartLevel()
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