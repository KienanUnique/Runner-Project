using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script
{
    public class PlayerUtilities : MonoBehaviour
    {
        [HideInInspector] public bool isAlive = true;

        private Tilemap _borderTilemap;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;

        private float _moveToX;
        private Animator _animator;

        private readonly int _hashIsMoving = Animator.StringToHash("IsMoving");
        private readonly int _hashDir = Animator.StringToHash("Direction");

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
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, _moveToX, Time.deltaTime * 8),
                    transform.position.y + GameConst.PlayerSpeed * Time.deltaTime, 0);
            }
        }

        public void Move(int direction)
        {
            var moveToCell = _mainGrid.WorldToCell(transform.position);
            if (direction == GameConst.LeftDirNum)
            {
                moveToCell.x -= 1;
            }
            else if (direction == GameConst.RightDirNum)
            {
                moveToCell.x += 1;
            }

            if (!_borderTilemap.HasTile(moveToCell))
            {
                _moveToX = _mainGrid.GetCellCenterWorld(new Vector3Int(moveToCell.x, 0, 0)).x;
            }
        }

        public void MoveToX(float moveToX)
        {
            _moveToX = moveToX;
        }

        public void Respawn()
        {
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
            transform.position = _mainGrid.GetCellCenterWorld((Vector3Int)GameConst.PlayerStartPos);
            _moveToX = transform.position.x;
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
