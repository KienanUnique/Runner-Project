using System.Collections;
using Script.Camera;
using Script.Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Triggers
{
    public class LineSwitchTrigger : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;
        private Tilemap _transitionsTilemap;
        private Tilemap _bordersTilemap;
        private CameraFollow _cameraCf;
        private bool _isSwitchLineCooldown;
        private Vector3Int _positionToMove;

        [SerializeField] private float switchLineCooldownSec = 0.5f;
        [SerializeField] private Tile twoSidedLineSwitchTile;
        [SerializeField] private Tile leftLineSwitchTile;
        [SerializeField] private Tile rightLineSwitchTile;


        private void Start()
        {
            _playerMovement = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerMovement>();
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
            _transitionsTilemap = GetComponent<Tilemap>();
            _bordersTilemap = _levelUtilities.GetBorderTilemap();
            _mainGrid = _levelUtilities.GetMainGrid();
            _isSwitchLineCooldown = false;
            if (UnityEngine.Camera.main is { })
                _cameraCf = UnityEngine.Camera.main.gameObject.GetComponent<CameraFollow>();
        }

        private IEnumerator SwitchLineWithCooldown()
        {
            if (_isSwitchLineCooldown)
                yield break;

            _isSwitchLineCooldown = true;

            yield return new WaitForSeconds(switchLineCooldownSec);

            _isSwitchLineCooldown = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            if (!_isSwitchLineCooldown)
            {
                _positionToMove = _mainGrid.WorldToCell(other.gameObject.transform.position);
                var deltaCheckX = new Vector3Int(1, 0, 0);
                if (other.bounds.center.x > _playerMovement.GetPreviousMoveToX() &&
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
                else if (other.bounds.center.x <= _playerMovement.GetPreviousMoveToX() &&
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

            _playerMovement.SetMoveToX(_mainGrid.GetCellCenterWorld(_positionToMove).x);
            StartCoroutine(SwitchLineWithCooldown());
        }
    }
}
