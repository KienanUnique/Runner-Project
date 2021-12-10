using Script.Player;
using UnityEngine;

namespace Script
{
    public class PlayerController : MonoBehaviour
    {

        private PlayerMovement _playerMovement;
        private LevelUtilities _levelUtilities;
        private PlayerAttack _playerAttack;

        private void Start()
        {
            Application.targetFrameRate = 60;
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAttack = GetComponent<PlayerAttack>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _levelUtilities.StartLevel();
        }
        
        private void Update()
        {
            if (_playerMovement.isAlive)
            {
                if (SwipeInput.SwipedLeft || SwipeInput.SwipedRight)
                {
                    _playerMovement.MoveOnSwipe(SwipeInput.SwipedLeft ? GameConst.LeftDirNum : GameConst.RightDirNum);
                }
                else if (SwipeInput.DoubleTap)
                {
                    _playerAttack.SetBlackout(!_playerAttack.GetBlackoutState());
                }
            }
            else if (!_playerMovement.isAlive && SwipeInput.SwipedLeft || SwipeInput.SwipedRight)
            {
                _levelUtilities.StartLevel();
            }
        }
    }
}