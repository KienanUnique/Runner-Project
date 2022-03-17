using System.Collections;
using Script.Camera;
using Script.Player;
using UnityEngine;

namespace Script.Triggers
{
    public class LineSwitchTrigger : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private LevelUtilities _levelUtilities;
        private CameraFollow _cameraCf;
        private bool _isSwitchLineCooldown;
        private Vector3Int _positionToMove;

        [SerializeField] private float switchLineCooldownSec = 0.5f;


        private void Start()
        {
            _playerMovement = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerMovement>();
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
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
                _positionToMove = _levelUtilities.ConvertWorldToCellPosition(other.gameObject.transform.position);
                var deltaCheckX = new Vector3Int(1, 0, 0);
                if (other.bounds.center.x > _playerMovement.GetPreviousMoveToX() && 
                    _levelUtilities.IsCellRightOrTwoSidedLineSwitch(_positionToMove + deltaCheckX))
                {
                    _positionToMove.x++;
                    {
                        while (_levelUtilities.IsCellTransition(_positionToMove))
                        {
                            _positionToMove.x++;
                        }

                        var checkingPos = _positionToMove;
                        while (!_levelUtilities.IsCellBorder(checkingPos))
                        {
                            checkingPos.x++;
                        }

                        checkingPos.x--;
                        _cameraCf.SetCameraXBetweenCellsCenters(_positionToMove.x, checkingPos.x);
                    }
                }
                else if (other.bounds.center.x <= _playerMovement.GetPreviousMoveToX() && 
                         _levelUtilities.IsCellLeftOrTwoSidedLineSwitch(_positionToMove - deltaCheckX))
                {
                    _positionToMove.x--;
                    while (_levelUtilities.IsCellTransition(_positionToMove))
                    {
                        _positionToMove.x--;
                    }

                    var checkingPos = _positionToMove;
                    while (!_levelUtilities.IsCellBorder(checkingPos))
                    {
                        checkingPos.x--;
                    }

                    checkingPos.x++;
                    _cameraCf.SetCameraXBetweenCellsCenters(checkingPos.x, _positionToMove.x);
                }
            }
            _playerMovement.SetMoveToX(_levelUtilities.ConvertCellToWorldPosition(_positionToMove).x);
            StartCoroutine(SwitchLineWithCooldown());
        }
    }
}
