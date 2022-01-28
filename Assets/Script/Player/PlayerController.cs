using Script.Input;
using UnityEngine;

namespace Script.Player
{
    [RequireComponent(typeof(PlayerEditor))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(LevelUtilities))]
    [RequireComponent(typeof(PlayerVisual))]
    [RequireComponent(typeof(ScreenInputController))]
    [RequireComponent(typeof(PlayerCharacter))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovement _playerMovement;

        private LevelUtilities _levelUtilities;
        private PlayerEditor _playerEditor;
        private PlayerVisual _playerVisual;
        private PlayerCharacter _playerCharacter;
        private ScreenInputController _screenInputController;

        private UIController _uiController;
        private PetController _petController;

        private bool _isAiming;

        private void Start()
        {
            Application.targetFrameRate = 60;

            _playerEditor = GetComponent<PlayerEditor>();
            _playerMovement = GetComponent<PlayerMovement>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _screenInputController = GetComponent<ScreenInputController>();
            _playerVisual = GetComponent<PlayerVisual>();
            _playerCharacter = GetComponent<PlayerCharacter>();

            _uiController = _playerEditor.GetUIController();
            _petController = _playerEditor.GetPetController();

            _levelUtilities.RestartLevel();
        }

        private void Update()
        {
            if (_playerCharacter.IsAlive())
            {
                if (_isAiming)
                {
                    if (_screenInputController.IsDoubleTap())
                    {
                        _isAiming = false;
                        ExitAimingMode();
                        _screenInputController.EnterMovingMode();
                    }
                }
                else
                {
                    if (_screenInputController.IsHorizontalSwipe())
                    {
                        _playerMovement.MoveOnSwipe(_screenInputController.GetHorizontalSwipeDirection());
                    }

                    else if (_screenInputController.IsDoubleTap())
                    {
                        _isAiming = true;
                        EnterAimingMode();
                        _screenInputController.EnterAimingMode();
                    }
                }
            }
            else
            {
                if (_playerMovement.IsMoving())
                {
                    _playerVisual.StartIdleAnimation();
                    _playerMovement.StopMoving();
                }
                
                if (_isAiming)
                {
                    _isAiming = false;
                    ExitAimingMode();
                    _screenInputController.EnterMovingMode();
                }

                if (_screenInputController.IsHorizontalSwipe())
                {
                    _levelUtilities.RestartLevel();
                }
            }
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
            _uiController.StartBlackout();
            _petController.Spawn();
            _levelUtilities.SlowDownTime();
        }

        private void ExitAimingMode()
        {
            _uiController.StopBlackout();
            _petController.Despawn();
            _levelUtilities.NormalizeTime();
        }

        private void Aim()
        {
            // TODO aim by two points: tap from SIC and pet pos from petController 
        }
    }
}