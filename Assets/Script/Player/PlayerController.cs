using Script.Player;
using UnityEngine;

namespace Script
{
    public class PlayerController : MonoBehaviour
    {

        private PlayerMovement _playerClass;
        private LevelUtilities _levelUtilities;

        private void Start()
        {
            Application.targetFrameRate = 60;
            _playerClass = GetComponent<PlayerMovement>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _levelUtilities.StartLevel();
        }
        
        private void Update()
        {
            if (_playerClass.isAlive)
            {
                if (SwipeInput.SwipedLeft || SwipeInput.SwipedRight)
                {
                    _playerClass.MoveOnSwipe(SwipeInput.SwipedLeft ? GameConst.LeftDirNum : GameConst.RightDirNum);
                }
                else if (SwipeInput.DoubleTap)
                {
                    Debug.Log("DoubleTap");
                }
            }
            else if (!_playerClass.isAlive && SwipeInput.SwipedLeft || SwipeInput.SwipedRight)
            {
                _levelUtilities.StartLevel();
            }
        }
    }
}