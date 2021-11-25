using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

namespace Script.Triggers
{
    public class LineSwitchTrigger : MonoBehaviour
    {
        private PlayerUtilities _playerUtilities;
        private LevelUtilities _levelUtilities;
        private Grid _mainGrid;
        private Tilemap _transitionsTilemap;

        private void Start()
        {
            _playerUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<PlayerUtilities>();
            _levelUtilities = GameObject.Find(GameConst.PlayerGameObjName).GetComponent<LevelUtilities>();
            _transitionsTilemap = GetComponent<Tilemap>();
            _mainGrid = _levelUtilities.GetLevelGrid();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            var positionToMove = _mainGrid.WorldToCell(other.gameObject.transform.position);
            if (other.bounds.center.x > _playerUtilities.GetPreviousmMoveToX())
            {
                positionToMove.x++;
                while (_transitionsTilemap.HasTile(positionToMove))
                {
                    positionToMove.x++;
                }
            }
            else
            {
                positionToMove.x--;
                while (_transitionsTilemap.HasTile(positionToMove))
                {
                    positionToMove.x--;
                }
            }
            StartCoroutine(_playerUtilities.SwitchLineWithCooldown(_mainGrid.GetCellCenterWorld(positionToMove).x));
        }
    }
}
