using System.Collections;
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
        private PlayerEditor _playerEditor;
        private PlayerVisual _playerVisual;
        private PlayerCharacter _playerCharacter;

        private UIController _uiController;
        private PetController _petController;
        private InputController _inputController;

        private bool _isAimingInputMode;
        private bool _isAimLineDrawing;

        private IEnumerator _updateTouchPositionForUI;

        private void Start()
        {
            Application.targetFrameRate = 60;

            _playerEditor = GetComponent<PlayerEditor>();
            _playerMovement = GetComponent<PlayerMovement>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _playerVisual = GetComponent<PlayerVisual>();

            _uiController = _playerEditor.GetUIController();
            _petController = _playerEditor.GetPetController();

            _updateTouchPositionForUI = UpdateTouchPositionForUI();

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
                _playerCharacter = GetComponent<PlayerCharacter>();
            }

            if (_isAimingInputMode)
            {
                _inputController.OnTouchStart += OnAimingTouchStart;
                _inputController.OnTouchEnd += OnAimingTouchEnd;
            }
            else
            {
                _inputController.OnHorizontalSwipe += OnHorizontalSwipe;
            }

            _inputController.OnDoubleTap += OnDoubleTap;
            _playerCharacter.OnPlayerKill += OnPlayerKill;
        }

        private void OnDisable()
        {
            if (_isAimingInputMode)
            {
                _inputController.OnTouchStart -= OnAimingTouchStart;
                _inputController.OnTouchEnd -= OnAimingTouchEnd;
            }
            else
            {
                _inputController.OnHorizontalSwipe -= OnHorizontalSwipe;
            }

            _inputController.OnDoubleTap -= OnDoubleTap;
            _playerCharacter.OnPlayerKill -= OnPlayerKill;
        }

        private void OnHorizontalSwipe(int direction)
        {
            if (_isAimingInputMode) return;
            if (_playerCharacter.IsAlive())
            {
                _playerMovement.MoveOnSwipe(direction);
            }
            else
            {
                RestartLevel();
            }
        }

        private void OnDoubleTap()
        {
            if (!_playerCharacter.IsAlive()) return;
            if (_isAimingInputMode)
            {
                ExitAimingMode();
            }
            else
            {
                EnterAimingMode();
            }
        }

        private void OnPlayerKill()
        {
            _playerVisual.StartIdleAnimation();
            _playerMovement.StopMoving();
            if (_isAimingInputMode)
            {
                ExitAimingMode();
            }
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

        private void EnterAimingMode()
        {
            _isAimingInputMode = true;
            _inputController.OnHorizontalSwipe -= OnHorizontalSwipe;
            _inputController.OnTouchStart += OnAimingTouchStart;
            _inputController.OnTouchEnd += OnAimingTouchEnd;
            _uiController.StartBlackout();
            _petController.Spawn();
            _levelUtilities.SlowDownTime();
        }

        private void ExitAimingMode()
        {
            _isAimingInputMode = false;
            if (_isAimLineDrawing)
            {
                StopDrawingAimingLine();
            }

            _inputController.OnTouchStart -= OnAimingTouchStart;
            _inputController.OnTouchEnd -= OnAimingTouchEnd;
            _inputController.OnHorizontalSwipe += OnHorizontalSwipe;
            _uiController.StopBlackout();
            _petController.Despawn();
            _levelUtilities.NormalizeTime();
        }

        private void OnAimingTouchStart()
        {
            StartDrawingAimingLine();
        }

        private void OnAimingTouchEnd()
        {
            ExitAimingMode();
        }

        private void StartDrawingAimingLine()
        {
            StartCoroutine(_updateTouchPositionForUI);
            _uiController.StartDrawingAimiLines(CreateScreenAimingPositionList());
            _isAimLineDrawing = true;
        }

        private void StopDrawingAimingLine()
        {
            StopCoroutine(_updateTouchPositionForUI);
            _uiController.StopDrawingAimiLines();
            _isAimLineDrawing = false;
        }

        private IEnumerator UpdateTouchPositionForUI()
        {
            while (true)
            {
                _uiController.UpdateAimingTouchPosition(CreateScreenAimingPositionList());
                yield return null;
            }
        }

        private Vector2[] CreateScreenAimingPositionList()
        {
            return new[]
            {
                _inputController.GetCurrentScreenTouchPosition(),
                _petController.GetScreenPetPosition()
            };
        }
    }
}