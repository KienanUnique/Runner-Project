using System.Collections;
using UnityEngine;

namespace Script.InputSystem
{
    [DefaultExecutionOrder(-1)]
    public class InputController : MonoBehaviour
    {
        [SerializeField] private float minimumStepDistance =  0.03f;
        [SerializeField, Range(0f, 1f)] private float swipeDirectionThreshold = .9f;

        #region Events

        public delegate void SwipeHorizontal(int direction);

        public event SwipeHorizontal OnHorizontalSwipe;

        public delegate void TouchStart();

        public event TouchStart OnTouchStart;

        public delegate void TouchEnd();

        public event TouchEnd OnTouchEnd;

        #endregion


        private InputManager _inputManager;
        private Vector2 _startPosition;
        private IEnumerator _updateTouchPositionForUI;

        private void Awake()
        {
            _inputManager = InputManager.Instance;
            _updateTouchPositionForUI = UpdateTouchPositionForUI();
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

        private void SwipeStart()
        {
            OnTouchStart?.Invoke();
            _startPosition = GetNormalizedSwipePosition();
            StartCoroutine(_updateTouchPositionForUI);
        }

        private void SwipeEnd()
        {
            OnTouchEnd?.Invoke();
            StopCoroutine(_updateTouchPositionForUI);
        }

        private void CheckSwipeLength(Vector2 startPosition, Vector2 currentPosition)
        {
            var swipeVector = currentPosition - startPosition;
            if (swipeVector.magnitude < minimumStepDistance)
            {
                return;
            }
            InvokeSwipeByDirection(swipeVector.normalized);
            UpdateStartPosition(currentPosition);
        }

        private void UpdateStartPosition(Vector2 newStartPosition)
        {
            _startPosition = newStartPosition;
        }

        private void InvokeSwipeByDirection(Vector2 direction)
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
        
        private IEnumerator UpdateTouchPositionForUI()
        {
            while (true)
            {
                CheckSwipeLength(_startPosition, GetNormalizedSwipePosition());
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private Vector2 GetNormalizedSwipePosition()
        {
            var nonNormalVector = _inputManager.PrimaryPosition();
            nonNormalVector.x /= Screen.width;
            nonNormalVector.y /= Screen.height;
            return nonNormalVector;
        }
    }
}