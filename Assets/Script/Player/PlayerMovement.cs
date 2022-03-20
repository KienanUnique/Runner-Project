using System.Collections;
using UnityEngine;

namespace Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Vector2Int playerStartPosition = new Vector2Int(-1, -5);
        [SerializeField] private int playerDefaultSpeedY = 4;
        [SerializeField] private float playerDefaultSmoothTimeX = 0.05f;
        
        private LevelUtilities _levelUtilities;
        private Transform _playerTransform;
        private IEnumerator _moveIEnumerator;

        private float _moveToX;
        private float _previousMoveToX;

        private Vector2 _velocity = Vector2.zero;

        private void Start()
        {
            _levelUtilities = GetComponent<LevelUtilities>();
            _playerTransform = GetComponent<Transform>();
            _moveIEnumerator = MoveCharacter();
        }

        public void MoveOnSwipe(int direction)
        {
            var moveToCell = _levelUtilities.ConvertWorldToCellPosition(_playerTransform.position);
            switch (direction)
            {
                case HorizontalSwipeDirection.SwipeLeft:
                    moveToCell.x -= 1;
                    break;
                case HorizontalSwipeDirection.SwipeRight:
                    moveToCell.x += 1;
                    break;
            }

            if (_levelUtilities.IsCellBorder(moveToCell)) return;
            
            _previousMoveToX = _moveToX;
            _moveToX = _levelUtilities.GetLineCenterInWorld(moveToCell.x);
        }

        public void StartMoving()
        {
            StartCoroutine(_moveIEnumerator);
        }

        public void StopMoving()
        {
            StopCoroutine(_moveIEnumerator);
        }

        public void SetMoveToX(float moveToX)
        {
            _previousMoveToX = _moveToX;
            _moveToX = moveToX;
        }

        public float GetPreviousMoveToX()
        {
            return _previousMoveToX;
        }

        public void RestorePosition()
        {
            _playerTransform.position = _levelUtilities.ConvertCellToWorldPosition((Vector3Int)playerStartPosition);
            _moveToX = _playerTransform.position.x;
            _previousMoveToX = _moveToX;
        }
        
        private IEnumerator MoveCharacter()
        {
            while (true)
            {
                var position = _playerTransform.position;
                var curPosX = Vector2.SmoothDamp(position, new Vector2(_moveToX, position.y),
                    ref _velocity, playerDefaultSmoothTimeX).x;
                var curPosY = position.y + playerDefaultSpeedY * Time.deltaTime;
                position = new Vector3(curPosX, curPosY, 0);
                _playerTransform.position = position;
                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}