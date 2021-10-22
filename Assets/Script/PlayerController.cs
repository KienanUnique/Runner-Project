using UnityEngine;

namespace Script
{
    public class PlayerController : MonoBehaviour
    {

        private PlayerUtilities _playerClass;
        private LevelUtilities _levelUtilities;

        private void Start()
        {
            Application.targetFrameRate = 60;
            _playerClass = GetComponent<PlayerUtilities>();
            _levelUtilities = GetComponent<LevelUtilities>();
        }
        
        private void Update()
        {
            if (SwipeInput.swipedLeft || SwipeInput.swipedRight)
            {
                if (_playerClass.isAlive)
                {
                    _playerClass.Move(SwipeInput.swipedLeft ? GameConst.s_LeftDir : GameConst.s_RightDir);
                }
                else _levelUtilities.StartLevel();
            }
        }
    }
}