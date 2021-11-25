using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector3 = UnityEngine.Vector3;

namespace Script
{
    public class PlayerUtilities : MonoBehaviour
    {
        [HideInInspector] public bool isAlive = true;

        private Tilemap _borderTilemap;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;
        
        private Tilemap _changeLineTilemap;

        private float _moveToX;
        private float _previousmMoveToX;
        private bool _isSwitchLineCooldown;
        
        private Animator _animator;

        private readonly int _hashIsMoving = Animator.StringToHash("IsMoving");
        private readonly int _hashDir = Animator.StringToHash("Direction");
        



        private void Start()
        {
            _levelUtilities = GetComponent<LevelUtilities>();
            _borderTilemap = _levelUtilities.GetLevelBorders();
            _changeLineTilemap = _levelUtilities.GetLevelLineSwitches();
            _mainGrid = _levelUtilities.GetLevelGrid();
            _animator = GetComponent<Animator>();
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
        }

        void Update()
        {
            if (isAlive)
            {
                transform.position = new Vector3(Mathf.Lerp(transform.position.x, _moveToX, Time.deltaTime * GameConst.PlayerSpeedX),
                    transform.position.y + GameConst.PlayerSpeedY * Time.deltaTime, 0);

            }
        }
        
        public IEnumerator SwitchLineWithCooldown(float newMoveX)
        {
            if (_isSwitchLineCooldown)
                yield break;

            SetMoveToX(newMoveX);
            
            _isSwitchLineCooldown = true;
            
            yield return new WaitForSeconds(GameConst.SwitchLineCooldownSec);

            _isSwitchLineCooldown = false;
        }
        
        public void MoveOnSwipe(int direction)
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

            if (!_borderTilemap.HasTile(moveToCell) && !(_isSwitchLineCooldown && _changeLineTilemap.HasTile(moveToCell)))
            //if (!_borderTilemap.HasTile(moveToCell) && !_isSwitchLineCooldown)
            {
                _previousmMoveToX = _moveToX; 
                _moveToX = _mainGrid.GetCellCenterWorld(new Vector3Int(moveToCell.x, 0, 0)).x;
            }
        }
        
        
        public void SetMoveToX(float moveToX)
        {
            _previousmMoveToX = _moveToX;
            _moveToX = moveToX;
        }

        public float GetPreviousmMoveToX()
        {
            return _previousmMoveToX;
        }

        public void Respawn()
        {
            _animator.SetInteger(_hashDir, 1);
            _animator.SetBool(_hashIsMoving, true);
            transform.position = _mainGrid.GetCellCenterWorld(_levelUtilities.GetPlayerGridStartPos());
            _moveToX = _mainGrid.GetCellCenterWorld(_levelUtilities.GetPlayerGridStartPos()).x;
            _previousmMoveToX = _moveToX; 
            _isSwitchLineCooldown = false;
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
