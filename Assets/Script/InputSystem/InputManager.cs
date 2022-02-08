using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.InputSystem
{
    [DefaultExecutionOrder(-2)]
    public class InputManager : Singleton<InputManager>
    {
        private PlayerInputActions _playerInputActions;

        #region Events

        public delegate void StartTouch(Vector2 position, float time);

        public event StartTouch OnStartTouch;

        public delegate void EndTouch(Vector2 position, float time);

        public event EndTouch OnEndTouch;

        #endregion

        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _playerInputActions.Enable();
        }

        private void OnDisable()
        {
            _playerInputActions.Disable();
        }

        private void Start()
        {
            _playerInputActions.Touch.PrimaryContact.started += context => StartTouchPrimary(context);
            _playerInputActions.Touch.PrimaryContact.canceled += context => EndTouchPrimary(context);
        }

        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            OnStartTouch?.Invoke(_playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>(),
                (float)context.startTime);
        }

        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            OnEndTouch?.Invoke(_playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.time);
        }

        public Vector2 PrimaryPosition()
        {
            return _playerInputActions.Touch.PrimaryPosition.ReadValue<Vector2>();
        }
    }
}