using Script.Input;
using UnityEngine;

namespace Script.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private float slowdownFactor = 0.3f;

        private bool _isAiming;

        private PlayerEffects _playerEffects;

        private ScreenInputController _screenInputController;

            void Start()
        {
            _playerEffects = GetComponent<PlayerEffects>();
            _screenInputController = GetComponent<ScreenInputController>();
            _isAiming = false;
        }

        public void EnterAimingMode()
        {
            _isAiming = true;
            _playerEffects.SetBlackout(true);
            _screenInputController.SwitchIntoAimingInputMode();
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        
        // TODO AimToTheDirection(Vector2 aimDirection)

        public void ExitAimingMode()
        {
            _playerEffects.SetBlackout(false);
            _screenInputController.SwitchIntoMovingInputMode();
            Time.timeScale = 1f;
            _isAiming = false;
        }

        public bool IsInAimingMode()
        {
            return _isAiming;
        }
    }
}
