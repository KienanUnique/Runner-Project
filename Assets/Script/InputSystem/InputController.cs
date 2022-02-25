using System.Collections;
using UnityEngine;

namespace Script.InputSystem
{
    [DefaultExecutionOrder(-1)]
    public class InputController : MonoBehaviour
    {
        [SerializeField] private float minimumSwipeDistance = .9f;
        [SerializeField] private float maximumSwipeTime = 1f;
        [SerializeField, Range(0f, 1f)] private float swipeDirectionThreshold = .9f;

        #region Events

        public delegate void SwipeHorizontal(int direction);

        public event SwipeHorizontal OnHorizontalSwipe;
        

        #endregion


        private InputManager _inputManager;
        private Vector2 _startPosition;
        private float _startTime;
        private Vector2 _endPosition;
        private float _endTime;

        private void Awake()
        {
            _inputManager = InputManager.Instance;
        }

        private void OnEnable()
        {
            _inputManager.OnStartTouch += SwipeStart;
            _inputManager.OnEndTouch += SwipeEnd;
        }

        private void OnDisable()
        {
            _inputManager.OnStartTouch -= SwipeStart;
            _inputManager.OnEndTouch -= SwipeEnd;
        }

        private void SwipeStart(Vector2 position, float time)
        {
            _startPosition = position;
            _startTime = time;
        }

        private void SwipeEnd(Vector2 position, float time)
        {
            _endPosition = position;
            _endTime = time;
            ProcessTouch();
        }

        private void ProcessTouch()
        {
            if (Vector3.Distance(_startPosition, _endPosition) >= minimumSwipeDistance &&
                (_endTime - _startTime) <= maximumSwipeTime)
            {
                var direction = (_endPosition - _startPosition).normalized;
                CalculateSwipeDirection(direction);
            }
        }

        private void CalculateSwipeDirection(Vector2 direction)
        {
            if (Vector2.Dot(Vector2.left, direction) >= swipeDirectionThreshold)
            {
                OnHorizontalSwipe?.Invoke(HorizontalSwipeDirection.SwipeLeft);
            }
            else if (Vector2.Dot(Vector2.right, direction) >= swipeDirectionThreshold)
            {
                OnHorizontalSwipe?.Invoke(HorizontalSwipeDirection.SwipeRight);
            }
        }

        public Vector2 GetCurrentScreenTouchPosition()
        {
            return _inputManager.PrimaryPosition();
        }
    }
}