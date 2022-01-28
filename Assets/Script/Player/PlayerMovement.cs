using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Tilemap _borderTilemap;
        private PlayerEditor _playerEditor;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;

        private float _moveToX;
        private float _previousMoveToX;

        private Vector2 _velocity = Vector2.zero;

        private bool _isMoving;

        private void Start()
        {
            _playerEditor = GetComponent<PlayerEditor>();
            _levelUtilities = GetComponent<LevelUtilities>();
            _borderTilemap = _levelUtilities.GetBorderTilemap();
            _mainGrid = _levelUtilities.GetMainGrid();
        }

        void Update()
        {
            if (!_isMoving) return;

            var curPosX = Vector2.SmoothDamp(transform.position, new Vector2(_moveToX, transform.position.y),
                ref _velocity, _playerEditor.GetPlayerDefaultSmoothTimeX()).x;
            var curPosY = transform.position.y + _playerEditor.GetPlayerDefaultSpeedY() * Time.deltaTime;
            transform.position = new Vector3(curPosX, curPosY, 0);
        }

        public void MoveOnSwipe(int direction)
        {
            var moveToCell = _mainGrid.WorldToCell(transform.position);
            switch (direction)
            {
                case InputEvent.SwipeLeft:
                    moveToCell.x -= 1;
                    break;
                case InputEvent.SwipeRight:
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

        public bool IsMoving()
        {
            return _isMoving;
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
            transform.position = _mainGrid.GetCellCenterWorld(_playerEditor.GetPlayerStartPosition());
            _moveToX = transform.position.x;
            _previousMoveToX = _moveToX;
        }
    }
}