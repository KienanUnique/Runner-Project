using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Triggers
{
    public class LineSwitchTrigger : MonoBehaviour
    {
        private PlayerUtilities _playerUtilities;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;
        private Tilemap _transitionsTilemap;
        private Tilemap _bordersTilemap;
        private CameraFollow _cameraCf;
        private bool _isSwitchLineCooldown;
        private Vector3Int _positionToMove;
        
        [SerializeField] private Tile twoSidedLineSwitchTile;
        [SerializeField] private Tile leftLineSwitchTile;
        [SerializeField] private Tile rightLineSwitchTile;
        

        private void Start()
        {
            _playerUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
            _transitionsTilemap = GetComponent<Tilemap>();
            _bordersTilemap = _levelUtilities.GetLevelBorders();
            _mainGrid = _levelUtilities.GetLevelGrid();
            _isSwitchLineCooldown = false;
            if (Camera.main is { }) _cameraCf = Camera.main.gameObject.GetComponent<CameraFollow>();
        }
        
        private IEnumerator SwitchLineWithCooldown()
        {
            if (_isSwitchLineCooldown)
                yield break;

            _isSwitchLineCooldown = true;

            yield return new WaitForSeconds(GameConst.SwitchLineCooldownSec);

            _isSwitchLineCooldown = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            if(!_isSwitchLineCooldown)
            {
                _positionToMove = _mainGrid.WorldToCell(other.gameObject.transform.position);
                var deltaCheckX = new Vector3Int(1, 0, 0);
                if (other.bounds.center.x > _playerUtilities.GetPreviousMoveToX() &&
                    (_transitionsTilemap.GetTile<Tile>(_positionToMove + deltaCheckX) == rightLineSwitchTile ||
                     _transitionsTilemap.GetTile<Tile>(_positionToMove + deltaCheckX) == twoSidedLineSwitchTile))
                {
                    _positionToMove.x++;
                    {
                        while (_transitionsTilemap.HasTile(_positionToMove))
                        {
                            _positionToMove.x++;
                        }

                        var checkingPos = _positionToMove;
                        while (!_bordersTilemap.HasTile(checkingPos))
                        {
                            checkingPos.x++;
                        }

                        checkingPos.x--;
                        _cameraCf.SetCameraXBetweenCellsCenters(_positionToMove.x, checkingPos.x);
                    }
                }
                else if (other.bounds.center.x <= _playerUtilities.GetPreviousMoveToX() &&
                         (_transitionsTilemap.GetTile<Tile>(_positionToMove - deltaCheckX) == leftLineSwitchTile ||
                          _transitionsTilemap.GetTile<Tile>(_positionToMove - deltaCheckX) == twoSidedLineSwitchTile))
                {
                    _positionToMove.x--;
                    while (_transitionsTilemap.HasTile(_positionToMove))
                    {
                        _positionToMove.x--;
                    }

                    var checkingPos = _positionToMove;
                    while (!_bordersTilemap.HasTile(checkingPos))
                    {
                        checkingPos.x--;
                    }

                    checkingPos.x++;
                    _cameraCf.SetCameraXBetweenCellsCenters(checkingPos.x, _positionToMove.x);
                }
            }
            _playerUtilities.SetMoveToX(_mainGrid.GetCellCenterWorld(_positionToMove).x);
            StartCoroutine(SwitchLineWithCooldown());
        }
    }
}
