using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [HideInInspector] public bool isAlive = true;
        [SerializeField] private int playerDefaultSpeedY = 4;
        [SerializeField] private float playerDefaultSmoothTimeX = 0.05f;

        private Tilemap _borderTilemap;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;

        private float _moveToX;
        private float _previousMoveToX;

        private Animator _animator;

        private readonly int _hashIsMoving = Animator.StringToHash("IsMoving");
        private readonly int _hashDir = Animator.StringToHash("Direction");

        private Vector2 _velocity = Vector2.zero;

        private void Start()
        {
            _levelUtilities = GetComponent<LevelUtilities>();
            _borderTilemap = _levelUtilities.GetLevelBorders();
            _mainGrid = _levelUtilities.GetLevelGrid();
            _animator = GetComponent<Animator>();
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
        }

        void Update()
        {
            if (isAlive)
            {
                var curPosX = Vector2.SmoothDamp(transform.position, new Vector2(_moveToX, transform.position.y),
                    ref _velocity, playerDefaultSmoothTimeX).x;
                var curPosY = transform.position.y + playerDefaultSpeedY * Time.deltaTime;
                transform.position = new Vector3(curPosX, curPosY, 0);
            }
        }

        public void MoveOnSwipe(int direction)
        {
            var moveToCell = _mainGrid.WorldToCell(transform.position);
            switch (direction)
            {
                case MovingConst.SwipedLeft:
                    moveToCell.x -= 1;
                    break;
                case MovingConst.SwipedRight:
                    moveToCell.x += 1;
                    break;
            }

            if (!_borderTilemap.HasTile(moveToCell))
            {
                _previousMoveToX = _moveToX;
                _moveToX = _mainGrid.GetCellCenterWorld(new Vector3Int(moveToCell.x, 0, 0)).x;
            }
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

        public void Respawn()
        {
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
            transform.position = _mainGrid.GetCellCenterWorld(_levelUtilities.GetPlayerGridStartPos());
            _moveToX = _mainGrid.GetCellCenterWorld(_levelUtilities.GetPlayerGridStartPos()).x;
            _previousMoveToX = _moveToX;
            isAlive = true;
        }

        public void Kill()
        {
            isAlive = false;
            _animator.SetInteger(_hashDir, 0);
            _animator.SetBool(_hashIsMoving, false);
        }
    }
}