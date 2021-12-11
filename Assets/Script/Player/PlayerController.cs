using Script.Input;
using UnityEngine;

namespace Script.Player
{
    public class PlayerController : MonoBehaviour
    {

        private PlayerMovement _playerMovement;
        private LevelUtilities _levelUtilities;
        private PlayerAttack _playerAttack;
        private ScreenInputController _screenInputController;

        private void Start()
        {
            Application.targetFrameRate = 60;
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _screenInputController = GetComponent<ScreenInputController>();
            _levelUtilities.StartLevel();
        }
        
        private void Update()
        {
            var curInputState = _screenInputController.GetCurrentInputState();
            if (_playerMovement.isAlive)
            {
                if (curInputState.IsHorizontalSwipe())
                {
                    _playerMovement.MoveOnSwipe(curInputState.GetMoveInput());
                }
                else if (curInputState.IsDoubleTap())
                {
                    if (_playerAttack.IsInAimingMode())
                    {
                        _playerAttack.ExitAimingMode();
                    }
                    else
                    {
                        _playerAttack.EnterAimingMode();
                    }
                }
            }
            else if (!_playerMovement.isAlive && curInputState.IsHorizontalSwipe())
            {
                _levelUtilities.StartLevel();
            }
        }
    }
}