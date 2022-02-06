using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Vector2Int playerStartPosition = new Vector2Int(-1, -5);
        [SerializeField] private int playerDefaultSpeedY = 4;
        [SerializeField] private float playerDefaultSmoothTimeX = 0.05f;
        
        private Tilemap _borderTilemap;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;

        private float _moveToX;
        private float _previousMoveToX;

        private Vector2 _velocity = Vector2.zero;

        private bool _isMoving;

        private void Start()
        {
            _levelUtilities = GetComponent<LevelUtilities>();
            _borderTilemap = _levelUtilities.GetBorderTilemap();
            _mainGrid = _levelUtilities.GetMainGrid();
        }

        private void Update()
        {
            if (!_isMoving) return; // TODO Ieminator (while true, return null)

            var position = transform.position;
            var curPosX = Vector2.SmoothDamp(position, new Vector2(_moveToX, position.y),
                ref _velocity, playerDefaultSmoothTimeX).x;
            var curPosY = position.y + playerDefaultSpeedY * Time.deltaTime;
            position = new Vector3(curPosX, curPosY, 0);
            transform.position = position;
        }

        public void MoveOnSwipe(int direction)
        {
            var moveToCell = _mainGrid.WorldToCell(transform.position);
            switch (direction)
            {
                case HorizontalSwipeDirection.SwipeLeft:
                    moveToCell.x -= 1;
                    break;
                case HorizontalSwipeDirection.SwipeRight:
                    moveToCell.x += 1;
                    break;
            }

            if (!_borderTilemap.HasTile(moveToCell))
            {
                _previousMoveToX = _moveToX;
                _moveToX = _mainGrid.GetCellCenterWorld(new Vector3Int(moveToCell.x, 0, 0)).x;
            }
        }

        public void StartMoving()
        {
            _isMoving = true;
        }

        public void StopMoving()
        {
            _isMoving = false;
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
            transform.position = _mainGrid.GetCellCenterWorld((Vector3Int)playerStartPosition);
            _moveToX = transform.position.x;
            _previousMoveToX = _moveToX;
        }
    }
}